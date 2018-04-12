﻿#pragma checksum "..\..\..\UserControls\HistoryTrending.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "90944C3F86FA930C9C46C4C5E17B64F6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LiveCharts.Wpf;
using Stocks.UserControls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Stocks.UserControls {
    
    
    /// <summary>
    /// HistoryTrending
    /// </summary>
    public partial class HistoryTrending : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\UserControls\HistoryTrending.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\UserControls\HistoryTrending.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\UserControls\HistoryTrending.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.Axis X;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\UserControls\HistoryTrending.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.Axis Y;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Stocks;component/usercontrols/historytrending.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\HistoryTrending.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 23 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MinHistory);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 24 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History5y);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 25 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History2y);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 26 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History1y);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 27 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History3m);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 28 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History1m);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 29 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.History10d);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 30 "..\..\..\UserControls\HistoryTrending.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ResetZoomOnClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.AddButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\UserControls\HistoryTrending.xaml"
            this.AddButton.Click += new System.Windows.RoutedEventHandler(this.Add);
            
            #line default
            #line hidden
            return;
            case 10:
            this.RemoveButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\UserControls\HistoryTrending.xaml"
            this.RemoveButton.Click += new System.Windows.RoutedEventHandler(this.Remove);
            
            #line default
            #line hidden
            return;
            case 11:
            this.X = ((LiveCharts.Wpf.Axis)(target));
            return;
            case 12:
            this.Y = ((LiveCharts.Wpf.Axis)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

