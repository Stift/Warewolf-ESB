﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 11.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Dev2.Studio.UI.Tests.UIMaps.PluginSourceMapClasses
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "11.0.51106.1")]
    public partial class PluginSourceMap
    {
        
        /// <summary>
        /// ClickCancel
        /// </summary>
        public void ClickCancel()
        {
            #region Variable Declarations
            WpfImage uIItemImage = this.UIPluginSourceManagmenWindow.UIItemImage;
            #endregion

            // Click image
            Mouse.Click(uIItemImage, new Point(26, 198));
        }
        
        #region Properties
        public UIPluginSourceManagmenWindow UIPluginSourceManagmenWindow
        {
            get
            {
                if ((this.mUIPluginSourceManagmenWindow == null))
                {
                    this.mUIPluginSourceManagmenWindow = new UIPluginSourceManagmenWindow();
                }
                return this.mUIPluginSourceManagmenWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIPluginSourceManagmenWindow mUIPluginSourceManagmenWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "11.0.51106.1")]
    public class UIPluginSourceManagmenWindow : WpfWindow
    {
        
        public UIPluginSourceManagmenWindow()
        {
            #region Search Criteria
            this.SearchProperties[WpfWindow.PropertyNames.Name] = "Plugin Source Managment";
            this.SearchProperties.Add(new PropertyExpression(WpfWindow.PropertyNames.ClassName, "HwndWrapper", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Plugin Source Managment");
            #endregion
        }
        
        #region Properties
        public WpfImage UIItemImage
        {
            get
            {
                if ((this.mUIItemImage == null))
                {
                    this.mUIItemImage = new WpfImage(this);
                    #region Search Criteria
                    this.mUIItemImage.WindowTitles.Add("Plugin Source Managment");
                    #endregion
                }
                return this.mUIItemImage;
            }
        }
        #endregion
        
        #region Fields
        private WpfImage mUIItemImage;
        #endregion
    }
}
