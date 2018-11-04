﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using CoreGraphics;
using AppKit;
using System.Text;
using Foundation;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Inspector.Mac
{
	public class ComboBoxWrapper : TextFieldViewWrapper
	{
		NSComboBox combo;

		public bool ButtonBordered
		{
			get => combo.ButtonBordered;
			set => combo.ButtonBordered = value;
		}

		public string ComboItems
		{
			get
			{
				var items = string.Join (",", combo.Values.Select(s => s.ToString()));
				return string.Format("{0} items. ({1})", combo.Values.Length, items);
			}
		}

		public ComboBoxWrapper(NSComboBox view) : base(view)
		{
			this.combo = view;
		}
	}

	public class TextViewWrapper : ViewWrapper
	{
		readonly NSTextView textView;
		public TextViewWrapper(NSTextView view) : base(view)
		{
			textView = view;
		}

		public string Value
		{
			get => textView.Value;
			set => textView.Value = value;
		}
		public NSFont Font
		{
			get => textView.Font;
			set => textView.Font = value;
		}

		public NSColor TextColor
		{
			get => textView.TextColor;
			set => textView.TextColor = value;
		}
		public NSColor BackgroundColor
		{
			get => textView.BackgroundColor;
			set => textView.BackgroundColor = value;
		}

		public NSAttributedString AttributedStringValue
		{
			get => textView.AttributedString;
		}
	}

	public class TextFieldViewWrapper : ViewWrapper
	{
		readonly NSTextField textView;

		public TextFieldViewWrapper (NSTextField view) : base(view)
		{
			textView = view;
		}

		public string StringValue
		{
			get => textView.StringValue;
			set => textView.StringValue = value;
		}

		public NSFont Font
		{
			get => textView.Font;
			set => textView.Font = value;
		}

		public NSColor TextColor
		{
			get => textView.TextColor;
			set => textView.TextColor = value;
		}
		public NSColor BackgroundColor
		{
			get => textView.BackgroundColor;
			set => textView.BackgroundColor = value;
		}

		public NSAttributedString AttributedStringValue
		{
			get => textView.AttributedStringValue;
			set => textView.AttributedStringValue = value;
		}
	}

	public class ButtonViewWrapper : ViewWrapper
	{
		readonly NSButton buttonView;

		#region Image

		public NSImage Image
		{
			get => buttonView.Image;
			set => buttonView.Image = value;
		}

		public NSImage AlternateImage
		{
			get => buttonView.AlternateImage;
			set => buttonView.AlternateImage = value;
		}

		#endregion

		#region Text

		public string Title
		{
			get => buttonView.Title;
			set => buttonView.Title = value;
		}

		public NSFont Font
		{
			get => buttonView.Font;
			set => buttonView.Font = value;
		}

		public string AlternateTitle
		{
			get => buttonView.AlternateTitle;
			set => buttonView.AlternateTitle = value;
		}

		public NSTextAlignment Alignment
		{
			get => buttonView.Alignment;
			set => buttonView.Alignment = value;
		}

		#endregion

		public bool Bordered
		{
			get => buttonView.Bordered;
			set => buttonView.Bordered = value;
		}

		NSButtonType buttonType;
		public NSButtonType ButtonType
		{
			get => buttonType;
			set
			{
				buttonType = value;
				buttonView.SetButtonType(buttonType);
			}
		}

		public ButtonViewWrapper(NSButton view) : base(view)
		{
			buttonView = view;
		}

		public bool IsSpringLoaded
		{
			get => buttonView.IsSpringLoaded;
			set => buttonView.IsSpringLoaded = value;
		}

		public bool AllowsMixedState
		{
			get => buttonView.AllowsMixedState;
			set => buttonView.AllowsMixedState = value;
		}

		public NSCellStateValue State
		{
			get => buttonView.State;
			set => buttonView.State = value;
		}

		public NSBezelStyle BezelStyle
		{
			get => buttonView.BezelStyle;
			set => buttonView.BezelStyle = value;
		}
	}

	public class BoxViewWrapper : ViewWrapper
	{
		readonly NSBox buttonView;

		public bool IsFlipped
		{
			get => buttonView.IsFlipped;
		}

		public BoxViewWrapper(NSBox view) : base(view)
		{
			buttonView = view;
		}

		public NSBoxType BoxType
		{
			get => buttonView.BoxType;
			set => buttonView.BoxType = value;
		}

		public NSBorderType BorderType
		{
			get => buttonView.BorderType;
			set => buttonView.BorderType = value;
		}
		public nfloat BoundsRotation
		{
			get => buttonView.BoundsRotation;
			set => buttonView.BoundsRotation = value;
		}
		public NSColor BorderColor
		{
			get => buttonView.BorderColor;
			set => buttonView.BorderColor = value;
		}

		public CGRect BorderRect
		{
			get => buttonView.BorderRect;
		}
	}

	public class ImageViewWrapper : ViewWrapper
	{
		readonly NSImageView buttonView;

		public NSImage Image
		{
			get => buttonView.Image;
			set => buttonView.Image = value;
		}

		public ImageViewWrapper(NSImageView view) : base(view)
		{
			buttonView = view;
		}

		public NSImageScale ImageScaling
		{
			get => buttonView.ImageScaling;
			set => buttonView.ImageScaling = value;
		}

		public NSImageAlignment ImageAlignment
		{
			get => buttonView.ImageAlignment;
			set => buttonView.ImageAlignment = value;
		}
	}

	public class ViewWrapper
	{
		protected readonly NSView view;

		public string Identifier
		{
			get => view.Identifier;
			set => view.Identifier = value;
		}

		public ViewWrapper (NSView view)
		{
			this.view = view;
		}

		public string AccessibilityTitle {
			get => view.AccessibilityTitle;
			set => view.AccessibilityTitle = value;
		}

		public string AccessibilityHelp {
			get => view.AccessibilityHelp;
			set => view.AccessibilityHelp = value;
		}

		public bool AccessibilityElement {
			get => view.AccessibilityElement;
			set => view.AccessibilityElement = value;
		}

		public bool TranslatesAutoresizingMaskIntoConstraints
		{
			get => view.TranslatesAutoresizingMaskIntoConstraints;
			set => view.TranslatesAutoresizingMaskIntoConstraints = value;
		}

		public CGRect AccessibilityFrame {
			get => view.AccessibilityFrame;
			set => view.AccessibilityFrame = value;
		}

		public string AccessibilityChildren
		{
			get => GetChildren();
		}

		string GetChildren ()
		{
			if (view.AccessibilityChildren == null || view.AccessibilityChildren.Length == 0 || view.AccessibilityChildren.Length > 10)
			{
				return $"Count {view.AccessibilityChildren.Length}";
			}

			var builder = new StringBuilder();
			builder.Append("Count " + view.AccessibilityChildren.Length + " : ");

			for (int i = 0; i < view.AccessibilityChildren.Length; i++)
			{
				if (i > 0)
				{
					builder.Append(", ");
				}
				var type = view.AccessibilityChildren[i].GetType().ToString ();
				builder.Append($"{type}");
			}
			return builder.ToString();
		}

		public NSView NextKeyView
		{
			get => view.NextKeyView;
		}

		public NSView NextValidKeyView
		{
			get => view.NextValidKeyView;
		}

		public NSView PreviousKeyView
		{
			get => view.PreviousKeyView;
		}

		public NSView PreviousValidKeyView
		{
			get => view.PreviousValidKeyView;
		}

		public NSFocusRingType FocusRingType
		{
			get => view.FocusRingType;
			set => view.FocusRingType = value;
		}

		public string ToolTip
		{
			get => view.ToolTip;
			set => view.ToolTip = value;
		}

		public int Tag
		{
			get => (int) view.Tag;
		}

		public bool Hidden
		{
			get => view.Hidden;
			set => view.Hidden = value;
		}

		public CGRect VisibleRect {
			get => view.VisibleRect ();
		}

		public CGRect Frame {
			get => view.Frame;
			set {
				view.Frame = value;
			}
		}

		public Type CurrentType {
			get => view.GetType ();
		}
	}
}