﻿#pragma checksum "C:\Users\gen3r\source\repos\GroupProject_RichardsFootlong\GroupProject_RichardsFootlong\AdminPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DD19FEEA3B86C641EBAD2ED9C2ACEE63666ADBA3226441CD1ED61B8561CD5119"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GroupProject_RichardsFootlong
{
    partial class AdminPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // AdminPage.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // AdminPage.xaml line 13
                {
                    this.imgUser = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3: // AdminPage.xaml line 14
                {
                    this.btnLogout = (global::Windows.UI.Xaml.Controls.HyperlinkButton)(target);
                    ((global::Windows.UI.Xaml.Controls.HyperlinkButton)this.btnLogout).Click += this.btnLogout_Click;
                }
                break;
            case 4: // AdminPage.xaml line 15
                {
                    this.txtBreadId = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // AdminPage.xaml line 16
                {
                    this.txtBreadName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // AdminPage.xaml line 20
                {
                    this.btnAddBread = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnAddBread).Click += this.btnAddBread_Click;
                }
                break;
            case 7: // AdminPage.xaml line 21
                {
                    this.btnUpdateBread = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnUpdateBread).Click += this.btnUpdateBread_Click;
                }
                break;
            case 8: // AdminPage.xaml line 22
                {
                    this.btnDeleteBread = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnDeleteBread).Click += this.btnDeleteBread_Click;
                }
                break;
            case 9: // AdminPage.xaml line 23
                {
                    this.txtMeatId = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10: // AdminPage.xaml line 24
                {
                    this.txtMeatName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 11: // AdminPage.xaml line 28
                {
                    this.btnAddMeat = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnAddMeat).Click += this.btnAddMeat_Click;
                }
                break;
            case 12: // AdminPage.xaml line 29
                {
                    this.btnUpdateMeat = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnUpdateMeat).Click += this.btnUpdateMeat_Click;
                }
                break;
            case 13: // AdminPage.xaml line 30
                {
                    this.btnDeleteMeat = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnDeleteMeat).Click += this.btnDeleteMeat_Click;
                }
                break;
            case 14: // AdminPage.xaml line 31
                {
                    this.txtMeatPrice = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15: // AdminPage.xaml line 33
                {
                    this.txtUsername = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 16: // AdminPage.xaml line 56
                {
                    this.dataMeat = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 18: // AdminPage.xaml line 37
                {
                    this.dataBread = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
