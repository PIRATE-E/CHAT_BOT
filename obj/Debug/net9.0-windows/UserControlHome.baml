   M S B A M L   `   `   `  ����C  ?CHAT_BOT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CHAT_BOTCHAT_BOT  5          CHAT_BOT.UserControlHome   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (localclr-namespace:CHAT_BOT     5      _ [MaterialDesignThemes.Wpf, Version=5.2.2.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5        $��Stretch=�5
      $��Stretch=�6*   .��5      ��   �� CornerRadius$  16��6       MaterialDesignDarkBackground#���   5       �� Padding$ 10q�5      $��1q�5        MaterialDesignDivider#���  5      ._�5   
   � $��10q�6     MaterialDesignPaper#���  6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      � $��0��6   $��0,0,0,15q�6     � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5#      �� -   5         ��Name$ HeadingText��5#      $��Chat Bot��5$       $  MaterialDesignHeadline5TextBlock#��[ 5%        PrimaryHueMidBrush#���  5&      $	��600��5'      $��CharacterEllipsis=�5(      $��Center=�5)      $
��Left=�5*      $��0��5+      �� 5-      -   5         ��Name$ NewChatButton��5-      $��1��5/         MaterialDesignOutlinedButton#��[ 50      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 8��51      $��40��52      $��0,5,15,0q�53      $
��12,0q�54      ���#E0F7FA#���  56      #���  57      .��58      ��  �� Orientation$ 
Horizontal=�6!   .+�59      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Plus=�62   $��20��5:   2   $��20��5;   2   $��Center=�5<   2   $��0,0,8,0q�5=   2   �� 5>      $��New Chat��6$   $��Center=�5?   $   $��Medium"�5@   $   5A      5B      �� 5D      -   5        $ HistoryButton��5D      $��2��5E      #��[ 5F      $ 8��5G      $��40��5H      $��0,5,5,0q�5I      $
��12,0q�5J      
�������#���  5M      #���  5N      +  %MaterialDesignThemes.Wpf.RippleAssist	   
IsCentered$
	 True��5O      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist
   IsIndicatorVisible$
 False��5P         IndicatorForeground# �  5Q      $
��Auto��5R      $��Right=�5S       � ToolTip$ View chat history��5T      .��5U      �� $ 
Horizontal=�6!   .+�5V        $ History=�62   $��20��5W   2   $��20��5X   2   $��Center=�5Y   2   $��0,0,8,0q�5Z   2   �� 5[      $��History��6$   $��Center=�5\   $   $��Medium"�5]   $     5^      -   5          Name$ 	ArrowIcon��5^   2   $ ChevronDown=�5_   2   $��16��5`   2   $��16��5a   2   $��Center=�5b   2   $��4,0,0,0q�5c   2   5d      5e      5f      � 5h      -   5         �Name$ HistoryPopup��5h   !   $��0��6    � 	StaysOpen$
 True��5j      $��Bottom=�5l       � AllowsTransparency$
 True��5m      ��5i      �� *IsHistoryOpen+ �� Mode$ OneWay=� � PlacementTarget 5k      ��  �� ElementName$ HistoryButton��.��5n      &    CHAT_BOT.UserControlHistoryPopUp  -   5          Name$ HistoryPopUpControl��5n   0   &    CHAT_BOT.UserControlHistoryPopUp   CloseAnimationCompleted$1 +HistoryPopUpControl_CloseAnimationCompleted��5o   /   5p      #  MaterialDesignThemes.Wpf.Card  5s      $��1��6"   $��0,0,0,15q�5t   "      UniformCornerRadius$ 8Q�5u   "   .  (MaterialDesignThemes.Wpf.ElevationAssist   	Elevation$	 Dp1=�5v   "   #���  5w   "   .��5x      �� -   5         ��Name$ 	scrollBar��5x      $
��Auto=�5y      $��15q�5z      .��5{      �� -	   5         ��Name$ 	ChatPanel��5{   !   $��5q�64   5|      5}      � 5�      $��2��6    5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   .+�5�        $��0��6&   $��0,0,10,0q�5�   &   $ 24Q�5�   &   $	 Dp1=�5�   &   $��2q�5�   &   .��5�      �� -
   5         ��Name$ UserInputTextRField��5�        MaterialDesignTextBox#��[ 5�      )	  #MaterialDesignThemes.Wpf.HintAssist 	  Hint$ Type your message here...��5�      .
  (MaterialDesignThemes.Wpf.TextFieldAssist 
  DecorationVisibility$ Hidden=�5�       
  TextBoxViewMargin$ 0,0,0,0q�5�      $��0q�5�      $
��15,8q�5�       �� TextWrapping$
 Wrap=�5�      $��Center=�5�      $��35��5�       �� AcceptsReturn$
 True��5�        �� 
AcceptsTab$
  True��5�      $	��150��5�      "! �� VerticalScrollBarVisibility$
! Auto=�5�      5�      �� 5�      -   5        $ 
SendButton��5�      $��1��5�        MaterialDesignFlatButton#��[ 5�      $ 8��5�      $��40��5�      $	��5,5q�5�      
������� ( $MaterialDesign.Brush.Chip.Background#���  5�      
���   �$
	 True��5�      $��0,0,5,0q�5�      .��5�      �� $ 
Horizontal=�6!   .+�5�        $
 Send=�62   $��20��5�   2   $��20��5�   2   $��Center=�5�   2   $��0,0,5,0q�5�   2   �� 5�      $
��Send��6$   $��Center=�5�   $   $��Medium"�5�   $   $��0,0,8,0q�5�   $   5�      5�      5�      5�      5�      5�      