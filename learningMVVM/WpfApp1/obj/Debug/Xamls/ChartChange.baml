   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfApp1.XamlsWpfApp1.Xamls  5   	       WpfApp1.Xamls.ChartChange   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WpfApp1.Xamls     5   	     � Title$  ChartChange��5   	   $	��600��6   $	��900��6*    � WindowStyle$
 None=�67   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��740* �6   �� 5      $	��25* �6   �� 5      $
��120* �6   �� 5      $��8* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 
myDataGrid��5         AutoGenerateColumns .  $��Center=�6C   $	��300��6`   $��Center=�5         CanUserAddRows .     CanUserDeleteRows . ��6G   �� *MustBuildings+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 编号��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 61   �� *Index+
 �� Mode$
 OneTime=�  5      $ 必配项目��6%   	 63   �� *Name+$
 OneTime=�  5      $ 人均建筑面积��6%   	 65   �� *
Area_per+$
 OneWay=�  5      $ 分区��6%   	 61   �� *District_name+  5      $ 层数��6%   	 61   �� *Layer+  5      $ 密度��6%   	 61   �� *	Density+  5      $ 总建筑面积��6%   	 64   �� *Area+  5      $ 面积调整��6%   	 63   �� *
AreaBias+  5      $ 用地��6%   	 61   �� *	Site_area+5      �� 5   
   $��更新��6   $	��200��5       $��50��6   $��Center=�6)   $��Bottom=�6F   $��347,0,193,19.667q�6a   ��5      �� *CheckBuildingsCommand+��6I   �� *MustBuildings+5    a   5!      5"      