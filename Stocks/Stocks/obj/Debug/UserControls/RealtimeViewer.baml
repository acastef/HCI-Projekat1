   M S B A M L   `   `   `  ����A  =Stocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Stocks.UserControlsStocks.UserControls  5      (    "Stocks.UserControls.RealtimeViewer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:Stocks.UserControls     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��2* �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   �� $��0��6   $��0��6$   
�������$��20(�6I    �� TextAlignment$ Center=�6W   $��Center=�6n   Last value:6�   �� 5   
   $��0��6   $��1��6$   
�������$��20(�6I   $ Center=�6W   $��Center=�6n   Current value:6�   �� 5   
   $��0��6   $��2��6$   
�������$��20(�6I   $ Center=�6W   $��Center=�6n   Trend:6�   �� 5   
   $��0��6   $��3��6$   
�������$��20(�6I   $ Center=�6W   $��Center=�6n   Trend( % ):6�   �� 5   
   $��1��6   $��0��6$   
�������$��20(�6I   $ Center=�6W   $��Center=�6n   ��6�   �� *	LastValue+ �� StringFormat$ {0:N2}��6�   �� 5   
   $��1��6   $��1��6$   
�������$��20(�6I   $ Center=�6W   $��Center=�6n   ��6�   �� *CurrentValue+$ {0:N2}��6�   �� 5   
   $��1��6   $��2��6$   $��20(�6N   $ Center=�6\   $��Center=�6s   ��61   �� *Color+��6�   �� *Trend+$ {0:N2}��6�   �� 5   
   $��1��6   $��3��6$   $��20(�6N   $ Center=�6\   $��Center=�6s   ��61   �� *Color+��6�   �� *TrendPercentage+$ {0:N2}��6�   �� 5   
   $��4��6   $��1��6"   $��3q�62   .+�5      �� $
��Bold"�6   .��6'   RefreshRate64   �� 5      -   5        $��RefreshRate��5       �� SelectedIndex$ 0��6*   6>   5       5!      5"      