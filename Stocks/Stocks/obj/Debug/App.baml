   M S B A M L   `   `   `  ����A  =Stocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:StocksStocks  5      )clr-namespace:Stocks.UtilStocks.Util  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5          
Stocks.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      &localclr-namespace:Stocks     5      ,singleclr-namespace:Stocks.Util     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5        �� 	Resources  5      �� 5	   
    �� Member$# single:Configuration.Instance��6      Key
Config  6K   �� 5   
   CustomWindow   �� 
TargetType" �6%   .�5      ��  �� Property � �� �� Value$	 500��6)   �� 5       � ��$	 500��6*   5      �� 5   
   CustomButton  " ��6%   .�5      ��  � ��$ 0 0 5 0q�6'   69   �� 5       � ��$
 Bold"�6+   6:   �� 5       � �� 5      �� 5       �� 
TargetType" ��6&   .R�5      ��  �� CornerRadius$ 3,3,3,3��6!   ���#ccc$��1,1,1,1q�6K   #��z��6e   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       �� Triggers 5      P� 5        P� Property �@J�IsMouseOver	 P� Value	 . . �5!      ��  � ��
 ���@�63   �� 5"       � ��
 �   �63   5#      P� 5%       � {�	 .  . �5&      ��  � ��
 �����63   �� 5'       � ��
 �����63   5(      5+      �� 5-   
   CustomTabitem  " ��6&   .�5.      ��  � �� �#2827286=   �� 5/       � ��
 �   �6;   �� 50       � ��$
 Bold"�6+   6:   �� 51       � �� 52      �� 53      " ��6&   .R�54      �� $��Border��6!   $��1,1,1,0q�6/   
�������$ 4,4,0,0��6a   $	��2,0q�6x   ._�55      �� 
 ��Name$
 ContentSite��6/   $��Center=�56   )   $��Center=�57   )   $��Header��58   )   $
��10,2q�59   )   5:       �� Triggers 5;      P� 5<       �@��
IsSelected	 . . �5=   "   ��  �� 
TargetName$ Border��6)    � �� �#3edde06S   �� 5>   "    � ��
 �   �6O   5?      P� 5@       �@��
IsSelected	 .  . �5A   "   �� $ Border��6)    � �� �#2827286S   �� 5B   "    � ��
 �����6T   5C      5E      5G      5H      �� 5K   
   CustomListBox  " ��6&   .�5L      ��  � �� �#2827286=   �� 5M       � ��
 �����6@   5N      �� 5P   
   CustomCheckBox  " ��6'   .�5Q      ��  � ��
 �����6@    5R      P� 5S       �@d�	IsChecked$
	 TrueY�6/   . �5T      ��  � �� �#27b7ba6E   �� 5U       � �� �#27b7ba6E   5V      5X      5[      