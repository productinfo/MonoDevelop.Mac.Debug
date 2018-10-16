﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;

namespace MonoDevelop.Mac.Debug
{
	public interface IWindowWrapper
	{
		IViewWrapper ContentView { get; set; }
		IViewWrapper FirstResponder { get; }

		event EventHandler LostFocus;
		event EventHandler ResizeRequested;
		event EventHandler MovedRequested;

		NSWindow GetWindow();
	}
}
