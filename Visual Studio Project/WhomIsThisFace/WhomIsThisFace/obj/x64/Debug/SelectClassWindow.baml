   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    WhomIsThisFace&    WhomIsThisFace.SelectClassWindow   5      HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$*  $เลือกรายวิชา��5   	   $	��300��6   $	��300��6+   (   "MahApps.Metro.Controls.MetroWindow   ShowMaxRestoreButton .   � 
ResizeMode$ NoResize=�5   &    � WindowStartupLocation$ CenterScreen=�5   	      ShowMinButton .   � 	Resources 5       �� MergedDictionaries �� Source�� 5	   
    5
      �� 5      $ /Resources/Icons.xaml?�6%   %    5         RightWindowCommands 5      +   %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5        	 ��Name$	 btnNewSubject��5      .��5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� $��20��6    $��20��5      ��5      ��  �� RelativeSource ��  �� AncestorType$ ButtonL� �� Path$ 
Foreground� J� OpacityMask 5      !� 5       j� Stretch$
 Fill=�6*      appbar_page_add !� Visual# [  69   5      �� 5      $��4 0 0 0q�6    $��Center=�5       $6��0สร้างรายวิชาใหม่��5!      5"      5#      �� 5$      -   5        $	 btnADDSubject��5$      .��5%      �� $
 
Horizontal=�6   .+�5&      �� $��20��6    $��20��5'      ��5(      ��  �� $ ButtonL�$ 
Foreground� 5)      !� 5*      $
 Fill=�6*   # [  69   5,      �� 5-      $��4 0 0 0q�6    $��Center=�5.      $*��$เพิ่มรายวิชา��5/      50      51      �� 52      -   5        $	 btnCancelSubject��52      .��53      �� $
 
Horizontal=�6   .+�54      �� $��20��6    $��20��55      ��56      ��  �� $ ButtonL�$ 
Foreground� 57      !� 58      $
 Fill=�6*     appbar_cancel# [ 69   5:      �� 5;      $��4 0 0 0q�6    $��Center=�5<      $��ยกเลิก��5=      5>      5?      5@      .��5B      � .+�5C   
   �� $
��Auto=�6   $��0,0,0,0q�6:   .��5D      �� -   5         ��Name$ StackPanelMain��5D      5E      5F      5H      5I      