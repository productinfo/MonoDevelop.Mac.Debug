﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MonoDevelop.Inspector.Mac
{
    internal class MacInspectorContext : InspectorContext
    {
        public MacInspectorContext()
        {
           
        }

        IInspectDelegate macDelegate;
        public virtual IInspectDelegate GetInspectorDelegate ()
        {
            if (macDelegate == null)
            macDelegate = new MacInspectorDelegate();
            return macDelegate;
        }

        protected override InspectorManager GetInitializationContext()
        {
            var inspectorDelegate = GetInspectorDelegate();
            var over = new MacBorderedWindow (CGRect.Empty, NSColor.Green);
            var next = new MacBorderedWindow (CGRect.Empty, NSColor.Red);
            var previous = new MacBorderedWindow (CGRect.Empty, NSColor.Blue);
            var acc = new MacAccessibilityWindow(new CGRect(10, 10, 600, 700));
            var ins = new InspectorWindow(inspectorDelegate, new CGRect(10, 10, 600, 700)); ;
            var tool = new MacToolbarWindow (inspectorDelegate);

            tool.ShowToolkit(hasToolkit);

            return new InspectorManager(macDelegate, over, next, previous, acc,ins, tool);
        }

        public static MacInspectorContext Current { get; set; } = new MacInspectorContext();
    }

    [Register("DebugWindow")]
	public class MacAccInspectorWindow : MacWindowWrapper, IMainWindowWrapper, INSTouchBarDelegate
    {
        NSTouchBar touchbar;

        MacInspectorContext context;
        ToolbarService service;

        public InspectorViewMode ViewMode { get; set; } = InspectorViewMode.Native;

        public MacAccInspectorWindow(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MacAccInspectorWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base(contentRect, aStyle, bufferingType, deferCreation)
        {
            Initialize();
        }

        public MacAccInspectorWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen screen) : base(contentRect, aStyle, bufferingType, deferCreation, screen)
        {
            Initialize();
        }

        protected MacAccInspectorWindow(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected MacAccInspectorWindow(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        void Initialize()
		{
            NSApplication.SharedApplication.SetAutomaticCustomizeTouchBarMenuItemEnabled(true);
            touchbar = new NSTouchBar();
            service = ToolbarService.Current;
            context = MacInspectorContext.Current;
            context.Initialize(false);
            service.SetDelegate(context.GetInspectorDelegate());

            context.FocusedViewChanged += Context_FocusedViewChanged;
        }

        void Context_FocusedViewChanged(object sender, IViewWrapper e)
        {
            if (e.NativeObject is NSView view)
            {
                RefreshBar(view);
            }
        }

        public override void BecomeMainWindow ()
		{
            MacInspectorContext.Current.Attach (this);
			base.BecomeMainWindow ();
		}

        protected override void Dispose(bool disposing)
        {
            context.FocusedViewChanged -= Context_FocusedViewChanged;
            base.Dispose(disposing);
        }


        [Export ("makeTouchBar")]
        public NSTouchBar MakeTouchBar ()
        {
            return touchbar;
        }

        void RefreshBar (NSView view)
        {
            if (service.GetTouchBarDelegate(view)?.NativeObject is TouchBarBaseDelegate touchBarDelegate)
            {
                touchBarDelegate.SetCurrentView(view);
                touchbar.Delegate = touchBarDelegate;
                touchbar.DefaultItemIdentifiers = touchBarDelegate.Identifiers;
                view.SetTouchBar(touchbar);
            }
        }

        public override bool MakeFirstResponder (NSResponder aResponder)
		{
			if (aResponder is NSView view) {
                MacInspectorContext.Current.ChangeFocusedView (new MacViewWrapper (view));
                RefreshBar(view);
            }
            return base.MakeFirstResponder (aResponder);
		}
	}
}
