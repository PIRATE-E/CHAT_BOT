   M S B A M L   `   `   `  ����C  ?CHAT_BOT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CHAT_BOTCHAT_BOT  5      9!clr-namespace:CHAT_BOT.ConvertersCHAT_BOT.Converters  5      &     CHAT_BOT.UserControlHistoryPopUp   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (localclr-namespace:CHAT_BOT     5      8
converters!clr-namespace:CHAT_BOT.Converters     5      _ [MaterialDesignThemes.Wpf, Version=5.2.2.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        $
��Root��5      $	��340��5      $	��450��6   .  (MaterialDesignThemes.Wpf.ElevationAssist    	Elevation$	  Dp3=�5       � 	Resources 5      2  ,System.Windows.Media.Animation.QuadraticEase   EaseOutQuad7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode.  (System.Windows.Media.Animation.CubicEase  EaseOutCubic  
EaseInQuad  ItemHoverAnimation �� TargetProperty W� From	 W� To g� Duration  ItemMouseEnter W� EasingFunction  ItemMouseLeave  ItemBackgroundEnter	 �� To	 �� EasingFunction  ItemBackgroundLeave  PopupOpenAnimation
 �� 
TargetName 	 PopupCloseAnimation 
 EmptyListTemplate'  !MaterialDesignThemes.Wpf.PackIcon   Kind  PrimaryHueMidBrush $  MaterialDesignHeadline6TextBlock   MaterialDesignBody1TextBlock  MaterialDesignBodyLight �� TextAlignment>   8CHAT_BOT.Converters.EmptyCollectionToVisibilityConverter ( $EmptyCollectionToVisibilityConverter��%�	  &	        &	 !     &	 B     &	 b     &	 �     7   7   &	 .    7   7   &	 �    7   &	     7   &	 �    7  7   &		 �    7  7  &	
 J    7  7  &	 $	      5   
   $ EaseOut=�6,     5   
   $ EaseOut=�6)     5   
   $ EaseIn=�6+   �� 5   
   .��5      W� $ Opacity�6    06B    16K   $ 0:0:0.2D�6R   5      �� 5   
   .��5      W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6    1.05       1.036)   $ 0:0:0.15D�63   8   6G   W� 5      $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6    1.05       1.036)   $ 0:0:0.15D�63   8  6G   5      �� 5!   
   .��5"      W� $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6    1.035#       1.06*   $ 0:0:0.15D�63   8   6G   W� 5$      $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)�6    1.035%       1.06*   $ 0:0:0.15D�63   8  6G   5&      �� 5)   
   .��5*      �� $( "Background.(SolidColorBrush.Color)�6    #E3F2FD5+      $ 0:0:0.15D�6)   8	   6=   5,      �� 5.   
   .��5/      �� $( "Background.(SolidColorBrush.Color)�6   	 White50      $ 0:0:0.15D�6'   8	   6;   51      �� 54   
   -   5        .��55      W� $

 Root��6   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�56       -5057       06)   $ 0:0:0.3D�60   8   58      W� 59      $

 Root��6   $ Opacity�5:       05;       16'   $ 0:0:0.3D�6.   8  5<      5=      �� 5@   
   -   5        .��5A      W� $

 Root��6   $U O(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)�5B       05C       -506'   $ 0:0:0.3D�60   8   5D      W� 5E      $

 Root��6   $ Opacity�5F       15G       06'   $ 0:0:0.3D�6.   8  5H      5I      �� 5L   
   .R�5M      � $	��180��6   .+�5N      �� $��Center=�6   $��Center=�68   .+�5O        $ History=�6.   $��64��5P   .   $��64��5Q   .   #���  5R   .   $��0,0,0,16q�5S   .   �� 5T      $��No history found��6    8��  #���  5V       $��Center=�5W       �� 5X      $-��'Start a new conversation to see it here��6    8�� #���  5Z       $
��Wrap=�5[       $ Center=�5\       $��0,8,0,0q�5]       5^      5_      5`        5c   
   6:   5d      z�5f      V� 5g   
   .~�5h      �� T� 5i      5j       � Triggers 5m      :� 5n   
    :� RoutedEvent$ UserControl.Loaded��6   .[�5o      �� #��[ 6   5p      .��5s      #  MaterialDesignThemes.Wpf.Card     UniformCornerRadius$ 8Q�6   
�������$	  Dp2=�5u      .��5w   
   �  � RowDefinitions 5x      �� 5y      $
��Auto �6    �� 5z      $��* �6    .+�5~      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ 
PrimaryMid=�6'   $��16,10q�69      CornerRadius$ 8,8,0,0��6I   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5�        $ History=�6.   $��Center=�6=   $��0,0,10,0q�6X   �� 5�      $��Conversation History��6     $  MaterialDesignSubtitle1TextBlock#��[ 5�      $��Medium"�5�      5�      5�      �� 5�      $��1��6   $
��Auto=�6(   $��0q�6K   .��5�      � .+�5�      �� -   5         ��Name$ HistoryListView��5�      $��0q�5�      
������ $
��Auto=�5�      $
��True��5�       %� IsVirtualizing$
 True��5�      $��0q�5�      +	  %MaterialDesignThemes.Wpf.RippleAssist 	  
IsDisabled$
 True��5�        MaterialDesignListView#��[ 5�      ��5�      �� *historyItemsSource+��5�      �� 5�       �� 
TargetType" ��6$     MaterialDesignListBoxItem �� BasedOn# [ 6F   .�5�   "   ��  �� Property � �� �� Value$ 8q�6<   �� 5�   "    � ��$ 4q�6;   �� 5�   "    � ��
 ���� 6?   �� 5�   "    � ��$ 0q�6D   �� 5�   "    �@�Cursor$
 Hand��6;   -   5        5�      ��5�      �� 5�      .R�5�   "     $��0q�66   $ 4Q�5�   5   $	  Dp3=�5�   5   
�������$
��12,8q�5�   5   z�5�   &   V� 5�   *   .~�5�   .   �� T� 5�   .   5�   +    5�   &   :� 5�   *   $ UIElement.MouseEnter��67   .[�5�   .   �� #��[ 6>   �� 5�   .   #��[ 6>   5�   +   :� 5�   *   $ UIElement.MouseLeave��67   .[�5�   .   �� #��[ 6>   �� 5�   .   #��[ 6>   5�   +   .��5�   &   �� .+�5�   *   �� #��[ 5�   4     MaterialDesignBody#���  5�   4   $
��Wrap=�5�   4   $	��280��5�   4   ��5�   4   �� *	Heading+5�   4   5�   '   5�   #   5�      5�      �� 5�      #��[
 5�   %   ��5�   %   �� *historyItemsSource.Count+y�5�   %   �� *historyItemsSource.Count+ �� 	Converter# [ 5�      5�      5�      5�      5�      