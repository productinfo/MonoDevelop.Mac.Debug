﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using CoreGraphics;
using AppKit;
using System.Collections.Generic;
using System.Linq;
using Xamarin.PropertyEditing.Mac;
using Xamarin.PropertyEditing.Themes;
using MonoDevelop.Mac.Debug.Services;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MonoDevelop.Mac.Debug
{
	class InspectorManager : IDisposable
	{
		const string Name = "Accessibility .NET Inspector";
		const int ToolbarWindowWidth = 500;
		const int ToolbarWindowHeight = 30;
		const int WindowMargin = 10;

		NSWindow inspectedWindow => selectedWindow as NSWindow;
		IViewWrapper view, nextKeyView, previousKeyView;
		IWindowWrapper selectedWindow;

		readonly MacBorderedWindow debugOverlayWindow;
		readonly MacBorderedWindow debugNextOverlayWindow;
		readonly MacBorderedWindow debugPreviousOverlayWindow;
		readonly InspectorWindow inspectorWindow;
		readonly AccessibilityWindow accessibilityWindow;
		readonly NSFirstResponderWatcher watcher;

	 	readonly List<NSMenuItem> menuItems = new List<NSMenuItem>();

		ToolbarWindow toolbarWindow;

		NSMenuItem inspectorMenuItem, firstOverlayMenuItem, nextOverlayMenuItem, previousOverlayMenuItem;

		readonly AccessibilityService accessibilityService;
		List<MacBorderedWindow> detectedErrors = new List<MacBorderedWindow> ();

		#region Properties

		bool isNextResponderOverlayVisible;
		bool IsNextResponderOverlayVisible {
			get => isNextResponderOverlayVisible;
			set {
				isNextResponderOverlayVisible = value;

				if (debugNextOverlayWindow != null) {
					debugNextOverlayWindow.ParentWindow = inspectedWindow;
					debugNextOverlayWindow.Visible = value;

					if (nextKeyView != null) {
						debugNextOverlayWindow.AlignWith (nextKeyView);
					}
					debugNextOverlayWindow.OrderFront (null);
				}
			}
		}

		bool isPreviousResponderOverlayVisible;
		bool IsPreviousResponderOverlayVisible {
			get => isPreviousResponderOverlayVisible;
			set {
				isPreviousResponderOverlayVisible = value;
				if (debugPreviousOverlayWindow != null) {
					debugPreviousOverlayWindow.ParentWindow = inspectedWindow;
					debugPreviousOverlayWindow.Visible = value;

					if (previousKeyView != null) {
						debugPreviousOverlayWindow.AlignWith (previousKeyView);
					}
					debugNextOverlayWindow.OrderFront (null);
				}
			}
		}

		bool isFirstResponderOverlayVisible;
		bool IsFirstResponderOverlayVisible {
			get => isFirstResponderOverlayVisible;
			set {
				isFirstResponderOverlayVisible = value;

				if (debugOverlayWindow != null) {
					debugOverlayWindow.ParentWindow = inspectedWindow;
					debugOverlayWindow.Visible = value;
					if (view != null) {
						debugOverlayWindow.AlignWith (view);
					}
					debugOverlayWindow.OrderFront (null);
				}
			}
		}

		bool IsStatusWindowVisible {
			get => inspectorWindow.ParentWindow != null;
			set => ShowStatusWindow (value);
		}

		NSMenu Submenu {
			get {
				var shared = NSApplication.SharedApplication;
				if (shared.Menu == null) {
					shared.Menu = new NSMenu();
				}

				NSMenuItem item;
				if (shared.Menu.Count == 0)
				{
					item = new NSMenuItem("Inspector");
					shared.Menu.AddItem(item);
				}
				else
				{
					item = shared.Menu.ItemAt(0);
				}

				if (item.Submenu == null) { 
					item.Submenu = new NSMenu();
				}
				return item.Submenu;
			}
		}

		#endregion

		#region Error Detector

		bool showDetectedErrors;
		internal bool ShowDetectedErrors 
		{
			get => showDetectedErrors;
			set
			{
				if (showDetectedErrors == value)
				{
					return;
				}
				ShowErrors(value);
			}
		}

		void ShowErrors (bool value)
		{
			showDetectedErrors = value;
			foreach (var item in detectedErrors)
			{
				item.AlignWindowWithContentView();
				item.Visible = value;
			}
		}

		#endregion

		static void RemoveAllErrorWindows (NSWindow window)
		{
			var childWindro = window.ChildWindows.OfType<MacBorderedWindow> ();
			foreach (var item in childWindro) {
				item.Close ();
			}
		}

		public void SetWindow (IWindowWrapper selectedWindow)
		{
			var needsReattach = selectedWindow != this.selectedWindow;

			if (this.selectedWindow != null) {
				RemoveAllErrorWindows (this.inspectedWindow);
			}

			if (this.selectedWindow != null)
			{
				this.selectedWindow.ResizeRequested -= OnRespositionViews;
				this.selectedWindow.MovedRequested -= OnRespositionViews;
			}

			PopulateSubmenu();

			this.selectedWindow = selectedWindow;
			if (this.selectedWindow == null || this.inspectedWindow == null) {
				return;
			}

			RefreshOverlaysVisibility ();

			AccessibilityService.Current.ScanErrors (Delegate, selectedWindow);

			this.selectedWindow.ResizeRequested += OnRespositionViews;
			this.selectedWindow.MovedRequested += OnRespositionViews;
			this.selectedWindow.LostFocus += OnRespositionViews;
		}

		void RefreshOverlaysVisibility ()
		{
			IsPreviousResponderOverlayVisible = IsPreviousResponderOverlayVisible;
			IsNextResponderOverlayVisible = IsNextResponderOverlayVisible;
			IsFirstResponderOverlayVisible = IsFirstResponderOverlayVisible;
		}

		internal IInspectDelegate Delegate;

		public NSWindow BackgroundWindow { get; set; }

		public InspectorManager (IInspectDelegate inspectorDelegate)
		{
			Delegate = inspectorDelegate;
			accessibilityService = AccessibilityService.Current;
			accessibilityService.ScanFinished += (s, e) => {

				RemoveAllErrorWindows (inspectedWindow);
				detectedErrors.Clear();

				foreach (var error in accessibilityService.DetectedErrors) {
					var borderer = new MacBorderedWindow(error.View, NSColor.Red);
					detectedErrors.Add(borderer);
					inspectedWindow.AddChildWindow (borderer, NSWindowOrderingMode.Above);
				}

				if (showDetectedErrors)
					ShowErrors(true);

				inspectorWindow.GenerateTree(selectedWindow);
				inspectedWindow.RecalculateKeyViewLoop();
			};

			debugOverlayWindow = new MacBorderedWindow (CGRect.Empty, NSColor.Green);
			debugNextOverlayWindow = new MacBorderedWindow (CGRect.Empty, NSColor.Red);
			debugPreviousOverlayWindow = new MacBorderedWindow (CGRect.Empty, NSColor.Blue);

			accessibilityWindow = new AccessibilityWindow(new CGRect(10, 10, 600, 700));
			accessibilityWindow.Title = "Accessibility Panel";
			accessibilityWindow.ShowErrorsRequested += (sender, e) => {
				ShowDetectedErrors = !ShowDetectedErrors;
			};

			accessibilityWindow.AuditRequested += (sender, e) => accessibilityService.ScanErrors(inspectorDelegate, selectedWindow);

			inspectorWindow = new InspectorWindow (inspectorDelegate, new CGRect(10, 10, 600, 700));
			inspectorWindow.Title = "Inspector Panel";
			inspectorWindow.RaiseFirstResponder += (s, e) => {
				if (inspectedWindow.ChildWindows.Contains (debugOverlayWindow))
					debugOverlayWindow.Close ();
				inspectedWindow.AddChildWindow(debugOverlayWindow, NSWindowOrderingMode.Above);

				//IsFirstResponderOverlayVisible = true;
				ChangeFocusedView(e);
			};
			inspectorWindow.RaiseDeleteItem += (s, e) =>
			{
				  RemoveView(e);
			};

			toolbarWindow = new ToolbarWindow (this);


			toolbarWindow.SetContentSize(new CGSize(ToolbarWindowWidth, ToolbarWindowHeight));
		
			toolbarWindow.ThemeChanged += (sender, pressed) => {
				if (pressed) {
					PropertyEditorPanel.ThemeManager.Theme = PropertyEditorTheme.Dark;
					accessibilityWindow.Appearance = inspectorWindow.Appearance = toolbarWindow.Appearance = inspectedWindow.Appearance = NSAppearance.GetAppearance (NSAppearance.NameVibrantDark);
				} else {
					PropertyEditorPanel.ThemeManager.Theme = PropertyEditorTheme.Light;
					accessibilityWindow.Appearance = inspectorWindow.Appearance = toolbarWindow.Appearance = inspectedWindow.Appearance = NSAppearance.GetAppearance (NSAppearance.NameVibrantLight);
				}
			};

			toolbarWindow.ItemDeleted += (sender, e) =>
			{
				RemoveView(view);
			};

			toolbarWindow.FontChanged += (sender, e) =>
			{
				Delegate.SetFont(view, e.Font);
				//NativeViewHelper.SetFont(view, e.Font);
			};

			toolbarWindow.ItemImageChanged += async (sender, e) =>
			{
				if (view.Content is NSImageView imageView)
				{
					var image = await OpenDialogSelectImage();
					if (image != null) {
						imageView.Image = image;
					}
				} else if (view.Content is NSButton btn) {
					var image = await OpenDialogSelectImage();
					if (image != null) {
						btn.Image = image;
					}
				}
			};

			toolbarWindow.KeyViewLoop += (sender, e) => {
				IsFirstResponderOverlayVisible = e;
				ChangeFocusedView (selectedWindow.FirstResponder);
			};

			toolbarWindow.NextKeyViewLoop += (sender, e) => {
				IsNextResponderOverlayVisible = e;
				ChangeFocusedView (selectedWindow.FirstResponder);
			};

			toolbarWindow.PreviousKeyViewLoop += (sender, e) => {
				IsPreviousResponderOverlayVisible = e;
				ChangeFocusedView (selectedWindow.FirstResponder);
			};

			watcher = new NSFirstResponderWatcher (inspectedWindow);
			watcher.Changed += (sender, e) => {
				ChangeFocusedView (e as IViewWrapper);
			};

			accessibilityWindow.RaiseAccessibilityIssueSelected += (s, e) =>
			{
				if (e == null)
				{
					return;
				}
				IsFirstResponderOverlayVisible = true;
				ChangeFocusedView(e);
			};
		}

		async Task<NSImage> OpenDialogSelectImage ()
		{
			var panel = new NSOpenPanel();
			panel.AllowedFileTypes = new[] { "png" };
			panel.Prompt = "Select a image";
			NSImage rtrn = null;
			processingCompletion = new TaskCompletionSource<object>();

			panel.BeginSheet (inspectedWindow, result => {
				if (result == 1 && panel.Url != null)
				{
					rtrn = new NSImage(panel.Url.Path);

				}
				processingCompletion.TrySetResult(null);
			});
			await processingCompletion.Task;
			return rtrn;
		}

		TaskCompletionSource<object> processingCompletion = new TaskCompletionSource<object>();

		void RemoveView (IViewWrapper toRemove)
		{
			var parent = toRemove?.PreviousValidKeyView;
			toRemove.RemoveFromSuperview();
			ChangeFocusedView(parent);
		}

		void OnRespositionViews (object sender, EventArgs e)
		{
			inspectorWindow.AlignRight (inspectedWindow, WindowMargin);
			accessibilityWindow.AlignLeft(inspectedWindow, WindowMargin);
			toolbarWindow.AlignTop (inspectedWindow, WindowMargin);
			RefreshOverlaysVisibility ();
		}

		void ShowStatusWindow (bool value)
		{
			if (value) {
				if (!IsStatusWindowVisible) {
					inspectedWindow.AddChildWindow(accessibilityWindow, NSWindowOrderingMode.Above);
					inspectedWindow.AddChildWindow (inspectorWindow, NSWindowOrderingMode.Above);
					inspectedWindow.AddChildWindow(toolbarWindow, NSWindowOrderingMode.Above);
					RefreshStatusWindow ();
				}
			}
			else {

				accessibilityWindow?.Close();
				toolbarWindow?.Close();
				inspectorWindow?.Close ();
			}
		}

		void RefreshStatusWindow ()
		{
			toolbarWindow.AlignTop(inspectedWindow, WindowMargin);
			inspectorWindow.AlignRight(inspectedWindow, WindowMargin);
			accessibilityWindow.AlignLeft (inspectedWindow, WindowMargin);
			var anyFocusedView = view != null;
			if (!anyFocusedView)
				return;

			inspectorWindow.GenerateStatusView (view, Delegate);
		}

		void PopulateSubmenu ()
		{
			var submenu = Submenu;
			if (submenu == null) {
				using (EventLog eventLog = new EventLog("Application"))
				{
					eventLog.Source = "Application";
					eventLog.WriteEntry("Submenu is null in Accessibility Inspector", EventLogEntryType.Error, 101, 1);
				}
				return;
			}

			ClearSubmenuItems(submenu);
			menuItems.Clear();
			submenu.AutoEnablesItems = false;

			int menuCount = 0;
			menuItems.Add(new NSMenuItem(string.Format("{0} v{1}", Name, GetAssemblyVersion()), ShowHideDetailDebuggerWindow) { Enabled = false });
			inspectorMenuItem = new NSMenuItem ($"Show Window", ShowHideDetailDebuggerWindow);
			inspectorMenuItem.KeyEquivalentModifierMask = NSEventModifierMask.CommandKeyMask | NSEventModifierMask.ShiftKeyMask;
			inspectorMenuItem.KeyEquivalent = "D";
			menuItems.Add (inspectorMenuItem);
			menuItems.Add(NSMenuItem.SeparatorItem);

			foreach (var item in menuItems) {
				submenu.InsertItem (item, menuCount++);
			}
		}

		void ClearSubmenuItems(NSMenu submenu)
		{
			foreach (var item in menuItems)
			{
				submenu.RemoveItem(item);
			}
		}

		void ShowHideDetailDebuggerWindow (object sender, EventArgs e)
		{
			IsStatusWindowVisible = !IsStatusWindowVisible;
			inspectorMenuItem.Title = string.Format ("{0} Window", ToMenuAction (!IsStatusWindowVisible));
		}

		string ToMenuAction (bool value) => value ? "Show" : "Hide";

		public event EventHandler<IViewWrapper> FocusedViewChanged;

		internal void ChangeFocusedView (IViewWrapper nextView)
		{
			if (inspectedWindow == null || nextView == null || view == nextView) {
				//FocusedViewChanged?.Invoke(this, nextView);
				return;
			}

			view = nextView;
			nextKeyView = view?.NextValidKeyView;
			previousKeyView = view?.PreviousValidKeyView;

		

			RefreshStatusWindow ();

			FocusedViewChanged?.Invoke(this, nextView);

			RefreshOverlaysVisibility ();
		}

		internal void StartWatcher ()
		{
			watcher.Start ();
		}

		string GetAssemblyVersion ()
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly ();
			var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo (assembly.Location);
			return fileVersionInfo.ProductVersion;
		}

		public void Dispose ()
		{
			ClearSubmenuItems (Submenu);
			debugOverlayWindow?.Close ();
			debugNextOverlayWindow?.Close ();
			debugPreviousOverlayWindow?.Close ();
			inspectorWindow?.Close ();
			accessibilityWindow?.Close();
			watcher.Dispose ();
		}
	}
}
