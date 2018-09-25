﻿
using System.Collections.Generic;
using AppKit;

namespace MonoDevelop.Mac.Debug
{
	public static class ViewDebuggerContext
	{
		static readonly Dictionary<NSWindow, ViewDebugDelegate> items;
		static ViewDebuggerContext ()
		{
			items = new Dictionary<NSWindow, ViewDebugDelegate> ();
		}

		public static void Attach (NSWindow window) 
		{
			if (!items.ContainsKey (window)) {
				var viewDebugDelegate = new ViewDebugDelegate (window);
				viewDebugDelegate.StartWatcher ();
				items.Add (window,viewDebugDelegate);
			}
		}

		public static void Remove (NSWindow window)
		{
			ViewDebugDelegate selected;
			if (items.TryGetValue (window,out selected)) {
				selected.Dispose ();
				items.Remove (window);
			}
		}
	}
}
