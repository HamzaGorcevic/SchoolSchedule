   M S B A M L   `   `   `  ����  SchoolSchedule)    #SchoolSchedule.Views.AttendanceForm   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      
�������$
��Auto��5      $
��Auto��6   .��5      � $��20q�6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5           Name$  
dgStudents��5         AutoGenerateColumns .     
IsReadOnly .    RowHeaderWidth$ 0Q�5         SelectionMode$ Single=�6&      CanUserAddRows .  $	��Top=�5	      $��Center=�6+   
�������
����İ�$��1q�5
   C      AlternatingRowBackground
 ��·� �� AlternationCount$ 2��5   ;   $��0,0,0,10q�5      ��5   U   �� *
Students+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$
	 Name��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6Q   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *	FirstName+6Q     5      $	 	Last Name��6%   $
 * 6U    68   �� *
LastName+6U     5      $	 Grade��6%   $
 * 6N    64   �� *Grade+6N   4  .System.Windows.Controls.DataGridTemplateColumn  5      $	 Absence��6)   $

 Auto 6:      CellTemplate 5      �� 5      .R�5      �� $��Center=�6y   $��Center=�6�    d� 	IsChecked 6'   �� *	IsPresent+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5      5         RowStyle 5      �� 5       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5      ��  �� Property � �� �� Value
 �����63   �� 5       � ��
 �   �63   �� 5       � ��
 ��İ�64   �� 5       � ��$ 1q�68   �� 5       � ��$	 0,2q�6/   �� 5        � ��$ 30��6/   5!         	CellStyle 5#      �� 5$      *  $System.Windows.Controls.DataGridCell"  6   .�5%      ��  � ��$ 1q�68   �� 5&       � ��
 ��İ�64   5'      5)      �� 5+   
   -   5        $��Record Attendance��5+      $��50��6B   $	��200��6N   $��Bottom=�5,      $��Center=�6,   
����·�
�������$��16(�5-   >   
����İ�$��1q�5.   .   $��10q�5/      $��0,10,0,0q�6   50      51      