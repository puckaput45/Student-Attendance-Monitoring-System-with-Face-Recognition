   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   WhomIsThisFace&    WhomIsThisFace.SelectClassWindow   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$*  $เลือกรายวิชา��$	��300��$	��300��(   "MahApps.Metro.Controls.MetroWindow   ShowMaxRestoreButton .   � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�   ShowMinButton .   � 	Resources  �� MergedDictionaries �� Source��  �� $ /Resources/Icons.xaml?�%       RightWindowCommands +   %MahApps.Metro.Controls.WindowCommands  .@��� -   	 ��Name$	 btnNewSubject��.���� 
 �� Orientation$
 
Horizontal=�.+��� $��20��$��20������  �� RelativeSource ��  �� AncestorType$ ButtonL� �� Path$ 
Foreground� J� OpacityMask !�  j� Stretch$
 Fill=�   appbar_page_add !� Visual# [  �� $��4 0 0 0q�$��Center=�$6��0สร้างรายวิชาใหม่���� -   $	 btnADDSubject��.���� $
 
Horizontal=�.+��� $��20��$��20������  �� $ ButtonL�$ 
Foreground� !� $
 Fill=�# [  �� $��4 0 0 0q�$��Center=�$*��$เพิ่มรายวิชา���� -   $	 btnCancelSubject��.���� $
 
Horizontal=�.+��� $��20��$��20������  �� $ ButtonL�$ 
Foreground� !� $
 Fill=�  appbar_cancel# [ �� $��4 0 0 0q�$��Center=�$��ยกเลิก��.��� .+��� $
��Auto=�$��0,0,0,0q�.���� -    ��Name$ StackPanelMain��