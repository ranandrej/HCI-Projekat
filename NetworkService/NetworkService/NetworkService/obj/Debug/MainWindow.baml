   M S B A M L   `   `   `  ����I  ENetworkService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NetworkServiceNetworkService  5   	   ;"clr-namespace:NetworkService.ViewsNetworkService.Views  5   	   C&clr-namespace:NetworkService.ViewModelNetworkService.ViewModel  5   	       NetworkService.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:NetworkService     5   	   4views"clr-namespace:NetworkService.Views     5   	   <	viewModel&clr-namespace:NetworkService.ViewModel     5   	   -   5          �Name$  
mainWindow��5   	    � Title$  ��5
   
   $	��880��6   $	��520��6     � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������ 
������  � AllowsTransparency .  � DataContext 5      2   ,NetworkService.ViewModel.MainWindowViewModel  5   
    � 	Resources 5       �� DataType-   'NetworkService.ViewModel.TableViewModel$   NetworkService.Views.TableView3   -NetworkService.ViewModel.DragAndDropViewModel*   $NetworkService.Views.DragAndDropView-   'NetworkService.ViewModel.GraphViewModel$   NetworkService.Views.GraphView��%�   (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5       .��5#      � 	 � RowDefinitions	 5$   
   �� 5%      $
��Auto �6   �� 5&      �� 5'      $
��Auto �6   .+�5)   
   � $��0��6   ���#E6F7FF.+�5*      �� $��68��6   $��0q�6    $��8q�64   $��Close X��6@   $
��Left=�6R   $��27,16,0,5.2q�6m   
���  ��
�������
 � Cursor$

 Hand��6�   ��6�   �� *CloseCommand+ 5+       �� 
TargetType �� Property �� Value��%_   '��       �� 5,      " ��6   .�5-      ��  �@��CornerRadius$ 10��69   5.      5/      50      51      � 52   
   $��2��6   
�������.+�54      ]� 
�������$��Center=�6/   $��
0,0,0,-0.6q�6L   $	��520��6`   .+�55      o� $	��Top=�6   $��20q�6,   
������ $��
104,0,63,0q�6R   .@�56      m� $��50��6   
�������$��30(�6?   $
��🏠��6M    m� CommandParameter$ table��6x   $��75��6�    m� Command 6Y   �� *
NavCommand+6�   m� 57      $��50��6   $��30(�6+   $
��🏁��69   
�������$ dragAndDrop��6v   $��76��6�    6W   �� *
NavCommand+6�   m� 58      $��50��6   $��30(�6+   $
��📊��69   
�������$ graph��6v   $��73��6�    6W   �� *
NavCommand+6�   m� 59      $��43��6   $��30(�6+   $��↩️��69   
�������$��86��6X    6c   �� *UndoCommand+5:      5;      5<      � 5>   
   $��1��6   .+�5A      �� ��6   �� *CurrentViewModel+6D   5C      5G      5H      