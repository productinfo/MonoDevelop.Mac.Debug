﻿// This file has been autogenerated from a class added in the UI designer.

using System;

namespace MonoDevelop.Inspector.Mac
{
	public class XwtToolbarWindow : MacXwtWindowWrapper, IToolbarWindow
	{
		public bool ImageChangedEnabled { get => throw new NotImplementedException (); set => throw new NotImplementedException (); }

		public event EventHandler<bool> KeyViewLoop;
		public event EventHandler<bool> NextKeyViewLoop;
		public event EventHandler<bool> PreviousKeyViewLoop;
		public event EventHandler<bool> ThemeChanged;
		public event EventHandler ItemDeleted;
		public event EventHandler ItemImageChanged;
		public event EventHandler<FontData> FontChanged;
		public event EventHandler<InspectorViewMode> InspectorViewModeChanged;

		public XwtToolbarWindow ()
		{

		}

	}
}
