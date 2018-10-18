﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MonoDevelop.Mac.Debug
{
	[Register ("DebugWindow")]
	public class MacAccInspectorWindow : MacWindowWrapper
	{
		public MacAccInspectorWindow () : base ()
		{

		}

		public override void BecomeMainWindow ()
		{
			InspectorContext.Attach (this);
			base.BecomeMainWindow ();
		}

		// Called when created from unmanaged code
		public MacAccInspectorWindow (IntPtr handle) : base (handle)
		{

		}

		public MacAccInspectorWindow (NSCoder coder) : base (coder)
		{
		}

		public MacAccInspectorWindow (CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base (contentRect, aStyle, bufferingType, deferCreation)
		{
		}

		public MacAccInspectorWindow (CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen screen) : base (contentRect, aStyle, bufferingType, deferCreation, screen)
		{
		}

		protected MacAccInspectorWindow (NSObjectFlag t) : base (t)
		{
		}

		public override bool MakeFirstResponder (NSResponder aResponder)
		{
			if (aResponder is NSView view) {
				InspectorContext.ChangeFocusedView (new MacViewWrapper (view));
			}
			return base.MakeFirstResponder (aResponder);
		}
	}
}