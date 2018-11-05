﻿using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace MonoDevelop.Inspector.Mac
{
    class MacToolbarDataSource : NSCollectionViewDataSource
    {
        public bool IsOnlyImage { get; set; }

        readonly List<CollectionHeaderItem> items;
        public MacToolbarDataSource(List<CollectionHeaderItem> items)
        {
            this.items = items;
        }

        public override NSCollectionViewItem GetItem(NSCollectionView collectionView, NSIndexPath indexPath)
        {
            var item = collectionView.MakeItem(IsOnlyImage ? MacInspectorToolbarImageCollectionViewItem.Name : MacInspectorToolbarCollectionViewItem.Name, indexPath);
            if (item is MacInspectorToolbarCollectionViewItem itmView)
            {
                var selectedItem = items[(int)indexPath.Section].Items[(int)indexPath.Item];

                itmView.View.ToolTip = selectedItem.ToolTip ?? "";
                itmView.TextField.StringValue = selectedItem.Label;
                itmView.DescriptionTextField.StringValue = string.Format ("- {0}", selectedItem.Description) ;
                itmView.TextField.AccessibilityTitle = selectedItem.AccessibilityLabel ?? "";
                itmView.TextField.AccessibilityHelp = selectedItem.AccessibilityHelp ?? "";
                itmView.ImageView.Image = selectedItem.Image?.NativeObject as NSImage;
                //TODO: carefull wih this deprecation (we need a better fix)
                //ImageView needs modify the AccessibilityElement from it's cell, doesn't work from main view
                itmView.ImageView.Cell.AccessibilityElement = false;
            }
            else if (item is MacInspectorToolbarImageCollectionViewItem imgView)
            {
                var selectedItem = items[(int)indexPath.Section].Items[(int)indexPath.Item];
                imgView.View.ToolTip = selectedItem.Label ?? "";
                imgView.Image = selectedItem.Image?.NativeObject as NSImage;
                imgView.ImageView.AccessibilityTitle = selectedItem.Label ?? "";
                imgView.ImageView.AccessibilityElement = true;
            }
            return item;
        }

        public override NSView GetView(NSCollectionView collectionView, NSString kind, NSIndexPath indexPath)
        {
            return collectionView.MakeSupplementaryView(kind, MacInspectorToolbarHeaderCollectionViewItem.Name, indexPath);
        }

        public override nint GetNumberofItems(NSCollectionView collectionView, nint section)
        {

            return items[(int)section].Items.Count;
        }

        public override nint GetNumberOfSections(NSCollectionView collectionView)
        {
            return items.Count;
        }
    }
}