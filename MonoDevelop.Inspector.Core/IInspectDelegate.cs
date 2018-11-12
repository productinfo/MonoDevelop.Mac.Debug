﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MonoDevelop.Inspector
{
    public interface IInspectDelegate
	{
        void SetFont(IViewWrapper view, IFontWrapper font);
		FontData GetFont(IViewWrapper view);
		void ConvertToNodes(IViewWrapper view, INodeView nodeView, InspectorViewMode viewMode);
        object GetWrapper (INativeObject viewSelected, InspectorViewMode viewMode);
		void Recursively (IViewWrapper contentView, List<DetectedError> DetectedErrors, InspectorViewMode viewMode);
		void RemoveAllErrorWindows(IWindowWrapper windowWrapper);
		Task<IImageWrapper> OpenDialogSelectImage(IWindowWrapper selectedWindow);
        IToolbarWrapperDelegateWrapper GetTouchBarDelegate(object element);
        void SetButton(IButtonWrapper button, IImageWrapper image);
		void SetButton(IImageViewWrapper imageview, IImageWrapper image);
		Task InvokeImageChanged(IViewWrapper view, IWindowWrapper selectedWindow);
		IBorderedWindow CreateErrorWindow (IViewWrapper view);
        IFontWrapper GetFromName(string selected, int fontSize);
        IMenuWrapper GetSubMenu();
        void ClearSubmenuItems(List<IMenuItemWrapper> menuItems, IMenuWrapper submenu);
        IMenuItemWrapper CreateMenuItem(string title, EventHandler menuItemOpenHandler);
        IMenuItemWrapper GetSeparatorMenuItem();
        IMenuItemWrapper GetShowInspectorWindowMenuItem(EventHandler menuItemOpenHandler);
        IMenuItemWrapper GetShowAccessibilityWindowMenuItem(EventHandler menuItemOpenHandler);
        IImageWrapper GetImageResource(string v);
        IButtonWrapper GetImageButton(IImageWrapper invokeImage);
        void SetAppearance(bool isDark,params IWindowWrapper[] inspectorWindow);
        void CreateItem(IViewWrapper view, ToolbarView e);
        void SetCultureInfo(CultureInfo cultureInfo);
    }
}