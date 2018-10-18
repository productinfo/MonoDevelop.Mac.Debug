﻿using System;
using AppKit;
using Xwt;
using System.Linq;
using CoreGraphics;

namespace MonoDevelop.Mac.Debug
{
	public class MacXwtWindowWrapper : Xwt.Window, IWindowWrapper
	{
		public IViewWrapper ContentView {
			get {
				if (NativeObject is NSWindow window && window.ContentView != null) {
					return new MacViewWrapper (window.ContentView);
				}
				return null;
			}
			set {
				base.Content = value.Content as Widget;
			}
		}

		public IViewWrapper FirstResponder {
			get {
				if (NativeObject is NSWindow host && host.FirstResponder != null) {
					return new MacViewWrapper (host.FirstResponder as NSView);
				}
				return null;
			}
		}

		protected override void OnBoundsChanged (BoundsChangedEventArgs a)
		{
			MovedRequested?.Invoke (this, EventArgs.Empty);
			base.OnBoundsChanged (a);
		}

		public event EventHandler ResizeRequested;
		public event EventHandler MovedRequested;
		public event EventHandler LostFocus;
		public event EventHandler GotFocus;

		public event EventHandler BecomeMainWindow;

		protected override void OnShown ()
		{
			Window.DidResize += (s, e) => {
				ResizeRequested?.Invoke (this, EventArgs.Empty);
			};

			Window.DidMove += (s, e) => {
				MovedRequested?.Invoke (this, EventArgs.Empty);
			};

			Window.DidResignKey += (s, e) => {
				LostFocus?.Invoke (this, EventArgs.Empty);
			};

			Window.DidBecomeMain += (s, e) => {
				OnBecomeMainWindow (this, EventArgs.Empty);
			};

			Window.DidBecomeKey += (sender, e) => {

			};

			base.OnShown ();
		}

		protected virtual void OnBecomeMainWindow (object sender, EventArgs args)
		{

		}

		public void AddChildWindow (IWindowWrapper borderer)
		{
			Window.AddChildWindow (borderer.NativeObject as NSWindow, NSWindowOrderingMode.Above);
		}

		public void RecalculateKeyViewLoop ()
		{
			Window.RecalculateKeyViewLoop ();
		}

		public bool ContainsChildWindow (IWindowWrapper debugOverlayWindow)
		{
			return Window.ChildWindows.Contains (debugOverlayWindow.NativeObject as NSWindow);
		}

		public void AlignLeft (IWindowWrapper toView, int pixels)
		{
			var toViewWindow = toView.NativeObject as NSWindow;
			var frame = Window.Frame;
			frame.Location = new CGPoint (toViewWindow.Frame.Left -  Window.Frame.Width - pixels, toViewWindow.Frame.Bottom - frame.Height);
			Window.SetFrame (frame, true);
		}

		public void AlignTop (IWindowWrapper toView, int pixels)
		{
			var toViewWindow = toView.NativeObject as NSWindow;
			var frame = Window.Frame;
			frame.Location = new CGPoint (toViewWindow.Frame.Left, toViewWindow.AccessibilityFrame.Y + toViewWindow.Frame.Height + pixels);
			Window.SetFrame (frame, true);
		}

		public void AlignRight (IWindowWrapper toView, int pixels)
		{
			var toViewWindow = toView.NativeObject as NSWindow;
			var frame = Window.Frame;
			frame.Location = new CGPoint (toViewWindow.Frame.Right + pixels, toViewWindow.Frame.Bottom - frame.Height);
			Window.SetFrame (frame, true);
		}

		public void SetTitle (string v)
		{
			Window.Title = v;
		}

		void IWindowWrapper.Close ()
		{
			Window.Close ();
		}

		public void SetContentSize (int toolbarWindowWidth, int toolbarWindowHeight)
		{
			Window.SetContentSize (new CGSize (toolbarWindowWidth, toolbarWindowHeight));
		}

		protected NSWindow Window => NativeObject as NSWindow;

		public object NativeObject => BackendHost.Backend.Window;

		public bool HasParentWindow => Window.ParentWindow != null;
	}
}