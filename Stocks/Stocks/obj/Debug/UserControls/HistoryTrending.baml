   M S B A M L   `   `   `  ����A  =Stocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Stocks.UserControlsStocks.UserControls  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      )    #Stocks.UserControls.HistoryTrending   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Stocks.UserControls     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5        � 	Resources  5
      -   'Stocks.UserControls.ZoomingModeCoverter   ZoomingModeCoverter��%   &	          5   
   6B   5      .��5      � $	��270��6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   �� $��1��6    �� Orientation$ 
Horizontal=�6"   $��
0, 0, 5, 0q�6;   .+�5      �� -   5          CustomButton#��[ 5   (   .��6N   MIN6S   �� 5      -   5        #��[ 5   '   .��6M   5y6Q   �� 5      -   5        #��[ 5   '   .��6M   2y6Q   �� 5      -   5        #��[ 5   '   .��6M   1y6Q   �� 5      -   5        #��[ 5   '   .��6M   3m6Q   �� 5      -   5        #��[ 5   '   .��6M   1m6Q   �� 5      -   5        #��[ 5   (   .��6N   10d6S   �� 5      -   5        #��[ 5   .   .��6T   
Reset Zoom6`   �� 5      -	   5        $��	AddButton��5      $��Right=�6&   $��50, 0, 0, 0q�6B   #��[ 6d   .��6�   	Compare6�   �� 5      -
   5        $��RemoveButton��5      $��Right=�6)   $��
5, 0, 0, 0q�6E   {�. #��[ 6y   .��6�   Remove from compare chart6�   5"      #  LiveCharts.Wpf.CartesianChart  5#   
   $��2��6   &   LiveCharts.Wpf.Charts.Base.Chart   Series 6*   �� *SeriesCollection+   Zoom 6N   �� *ZoomingMode+   AxisX 5%        LiveCharts.Wpf.Axis  -   5        $��X��5%         LiveCharts.Wpf.DefaultAxes   CleanSeparator   	Separator# Z 5&         LabelFormatter 5%   $   �� *
XFormatter+5&      	   AxisY	 5)        -   5        $��Y��5)       6$   �� *
YFormatter+  5+        LiveCharts.Wpf.LineSeries
 �� 
TargetType  LiveCharts.Wpf.Series �� Property �� Value#  LiveCharts.Wpf.DefaultTooltip��%9  '        '  �     '  ^    �� 5,      "
  6   .�5-      ��  �@ Stroke
 ���@�6C   �� 5.       �@ StrokeThickness$ 7Q�68   6D   �� 5/       �@ PointForeground
 ���@�6L   50      �� 51      "
  6   .�52      ��  � �� �#2321236E   �� 53       � ��$
 Bold"�63   6B   �� 54       � ��
 �����6H   55      �� 56      "
  6   .�57      ��  �@ 
FontWeight$
 Bold"�63   6B   �� 58       �@ FontSize$ 10Q�61   6>   �� 59       �@ 
Foreground
 �����6H   5:      5;      5<      5=      5>      