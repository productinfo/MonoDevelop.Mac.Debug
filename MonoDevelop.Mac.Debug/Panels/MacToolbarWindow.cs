﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using System.Linq;
using Foundation;
using MonoDevelop.Mac.Debug.Services;

namespace MonoDevelop.Mac.Debug
{
	class MacToolbarWindow : MacWindowWrapper, IToolbarWindow
	{
		public event EventHandler<bool> KeyViewLoop;
		public event EventHandler<bool> NextKeyViewLoop;
		public event EventHandler<bool> PreviousKeyViewLoop;

		public event EventHandler<bool> ThemeChanged;

		public event EventHandler ItemDeleted;
		public event EventHandler ItemImageChanged;
		public event EventHandler<FontData> FontChanged;

		const int MenuItemSeparation = 3;
		const int LeftPadding = 5;

		readonly NSStackView stackView;

		readonly InspectorManager inspectorManager;

		public MacToolbarWindow (InspectorManager inspectorManager)
		{
			this.inspectorManager = inspectorManager;

			//BackgroundColor = NSColor.Clear;
			IsOpaque = false;
			StyleMask = NSWindowStyle.Titled | NSWindowStyle.FullSizeContentView;
			TitlebarAppearsTransparent = true;
			TitleVisibility = NSWindowTitleVisibility.Hidden;
			ShowsToolbarButton = false;
			MovableByWindowBackground = false;

			stackView = NativeViewHelper.CreateHorizontalStackView (MenuItemSeparation);
			ContentView.AddSubview (stackView);
			stackView.CenterYAnchor.ConstraintEqualToAnchor (ContentView.CenterYAnchor, 0).Active = true;
			stackView.LeftAnchor.ConstraintEqualToAnchor (ContentView.LeftAnchor, LeftPadding).Active = true;

			//stackView.RightAnchor.ConstraintEqualToAnchor(ContentView.RightAnchor, -LeftPadding).Active = true;

			//Visual issues view
			var keyViewLoopButton = new ToggleButton(ResourceService.GetNSImage("overlay-actual.png"));
			AddButton (keyViewLoopButton);
			keyViewLoopButton.Activated += (s, e) => {
				KeyViewLoop?.Invoke(this, keyViewLoopButton.IsToggled);
			};

			var prevKeyViewLoopButton = new ToggleButton(ResourceService.GetNSImage("overlay-previous.png"));
			AddButton (prevKeyViewLoopButton);
			prevKeyViewLoopButton.Activated += (s, e) => {
				PreviousKeyViewLoop?.Invoke(this, prevKeyViewLoopButton.IsToggled);
			};

			var nextKeyViewLoopButton = new ToggleButton(ResourceService.GetNSImage("overlay-next.png"));
			AddButton (nextKeyViewLoopButton);
			nextKeyViewLoopButton.Activated += (s, e) => {
				NextKeyViewLoop?.Invoke(this, nextKeyViewLoopButton.IsToggled);
			};

			AddSeparator ();

			var themeButton = new ToggleButton (ResourceService.GetNSImage("style-16.png"));
			AddButton (themeButton);
			themeButton.Activated += ThemeButton_Activated;

			AddSeparator ();

			deleteButton = new ImageButton(ResourceService.GetNSImage("delete-16.png"));

			AddButton(deleteButton);
			deleteButton.Activated += (s,e) =>
			{
				ItemDeleted?.Invoke(this, EventArgs.Empty);
			};

			changeImage = new ImageButton(ResourceService.GetNSImage("image-16.png"));

			AddButton(changeImage);

			changeImage.Activated += (s, e) =>
			{
				ItemImageChanged?.Invoke(this, EventArgs.Empty);
			};

			fontsCombobox = new NSComboBox() { TranslatesAutoresizingMaskIntoConstraints = false };
			fonts = NSFontManager.SharedFontManager.AvailableFonts
				.Select (s => new NSString(s))
				.ToArray ();

			fontsCombobox.Add(fonts);

			stackView.AddArrangedSubview(fontsCombobox);
			fontsCombobox.WidthAnchor.ConstraintEqualToConstant(220).Active = true;
		
			fontSizeTextView = new NSTextField() { TranslatesAutoresizingMaskIntoConstraints = false };
			stackView.AddArrangedSubview(fontSizeTextView);
			fontSizeTextView.WidthAnchor.ConstraintEqualToConstant(40).Active = true;

			fontsCombobox.SelectionChanged += (s, e) => {
				OnFontChanged();
			};

			fontSizeTextView.Activated += (s, e) => {
				OnFontChanged();
			};
			//AddSeparator();

			inspectorManager.FocusedViewChanged += (sender, view) =>
			{
				bool showImage = false;
				bool showFont = false;
				//NSPopUpButton
				var fontData = inspectorManager.Delegate.GetFont(view); 
				if (fontData?.Font != null)
				{
					var currentFontName = fontData.Font.FontName;
					if (currentFontName == ".AppleSystemUIFont")
					{
						currentFontName = "HelveticaNeue";
					}
					var name = fonts.FirstOrDefault(s => s.ToString() == currentFontName);
					fontsCombobox.Select(name);

					fontSizeTextView.IntValue = (int)fontData.Size;
					showFont = true;
				}

				if (view.Content is NSImageView || view.Content is NSButton)
				{
					showImage = true;
				}

				imageButtonVisible = showImage;
				fontButtonsVisible = showFont;
			};

			stackView.AddArrangedSubview(new NSView() { TranslatesAutoresizingMaskIntoConstraints = false });
		}

		bool fontButtonsVisible
		{
			get => stackView.Subviews.Contains(fontsCombobox);
			set
			{
				if (fontButtonsVisible == value)
				{
					return;
				}

				if (value)
				{
					stackView.AddArrangedSubview(fontsCombobox);
					stackView.AddArrangedSubview(fontSizeTextView);
				}
				else
				{
					fontSizeTextView.RemoveFromSuperview();
					fontsCombobox.RemoveFromSuperview();
				}
			}
		}

		bool imageButtonVisible
		{
			get => stackView.Subviews.Contains(changeImage);
			set
			{
				if (imageButtonVisible == value)
				{
					return;
				}

				if (value)
				{
					stackView.AddArrangedSubview(changeImage);
				}
				else
				{
					changeImage.RemoveFromSuperview();
				}
			}
		}

		void OnFontChanged ()
		{
			var currentIndex = (int)fontsCombobox.SelectedIndex;
			if (currentIndex >= -1)
			{
				var selected = fonts[currentIndex].ToString();
				var fontSize = fontSizeTextView.IntValue;
				FontChanged?.Invoke(this, new FontData (selected, fontSize));
			}
		}

		NSString[] fonts;
		NSComboBox fontsCombobox;
		NSTextField fontSizeTextView;
		//public override bool CanBecomeKeyWindow => false;
		//public override bool CanBecomeMainWindow => false;

		ImageButton deleteButton, changeImage;

		public bool ImageChangedEnabled
		{
			get => changeImage.Enabled;
			set => changeImage.Enabled = value;
		}

		void ThemeButton_Activated (object sender, EventArgs e)
		{
			if (sender is ToggleButton btn) {
				ThemeChanged?.Invoke (this, btn.IsToggled);
			}
		}

		void AddSeparator () => stackView.AddArrangedSubview (new VerticalSeparator ());

		void AddButton (NSButton view)
		{
			stackView.AddArrangedSubview (view);
			view.WidthAnchor.ConstraintEqualToConstant (InspectorWindow.ButtonWidth).Active = true;
		}

	}
}