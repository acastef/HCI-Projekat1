   M S B A M L   `   `   `  ����A  =Stocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Stocks.UserControlsStocks.UserControls  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   &     Stocks.UserControls.CompareGraph   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Stocks.UserControls     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  CompareGraph��5	   	   $	��450��6   $	��800��6+   .��5
      � .+�5   
   #  LiveCharts.Wpf.CartesianChart  $��1��6   &   LiveCharts.Wpf.Charts.Base.Chart   Series 6*   �� *SeriesCollection+ � 	Resources 5       �� 
TargetType �� Property �� Value#  LiveCharts.Wpf.DefaultTooltip  LiveCharts.Wpf.Axis��%�  '        '  N     '  �     �� 5      "  6   .�5      ��  � �� �#2827286E   5      �� 5      "  6   .�5      ��  � �� �#2321236E   �� 5       � ��$
 Bold"�63   6B   �� 5       � ��
 �����6H   5      �� 5      "  6   .�5      ��  �@ 
FontWeight$
 Bold"�63   6B   �� 5       �@ FontSize$ 15Q�61   6>   �� 5       �@ 
Foreground
 �����6H   5      5         AxisX 5           LabelFormatter 6   �� *
XFormatter+   AxisY 5"         6   �� *
YFormatter+	   ChartLegend	 5$      "  LiveCharts.Wpf.DefaultLegend  5%      
   
BulletSize$
 20Q�6$   
���  ��6G   5'       5)   
   ��%    5+      5-      5.      