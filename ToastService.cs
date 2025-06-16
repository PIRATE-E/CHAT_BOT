using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CHAT_BOT
{
    public static class ToastService
    {
        public static void ShowToast(string content, FrameworkElement parent)
        {
            // Check if we're on the UI thread
            if (!parent.Dispatcher.CheckAccess())
            {
                // If not on UI thread, dispatch to UI thread and return
                parent.Dispatcher.InvokeAsync(() => ShowToast(content, parent));
                return;
            }

            // The rest of the original ShowToast implementation (which will now run on the UI thread)
            Border toast = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(230, 30, 30, 30)),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(16, 12, 16, 12),
                MaxWidth = 300,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 20),
                RenderTransform = new TranslateTransform(0, 100),
                Opacity = 0
            };

            TextBlock textBlock = new TextBlock
            {
                Text = content,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };

            toast.Child = textBlock;

            // Add to visual tree
            Grid parentGrid = parent as Grid;
            if (parentGrid == null)
            {
                Panel panel = parent as Panel;
                if (panel != null)
                {
                    panel.Children.Add(toast);
                }
                else
                {
                    // Create a new grid for the toast if parent isn't a panel
                    Grid overlayGrid = new Grid();
                    Panel.SetZIndex(overlayGrid, 1000);
                    
                    ContentControl contentControl = parent as ContentControl;
                    if (contentControl != null)
                    {
                        var originalContent = contentControl.Content;
                        contentControl.Content = overlayGrid;
                        
                        if (originalContent is UIElement element)
                        {
                            overlayGrid.Children.Add(element);
                        }
                        
                        overlayGrid.Children.Add(toast);
                    }
                }
            }
            else
            {
                parentGrid.Children.Add(toast);
                Panel.SetZIndex(toast, 1000);
            }

            // Animate in
            DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            DoubleAnimation translateAnimation = new DoubleAnimation(50, 0, TimeSpan.FromMilliseconds(300));
            translateAnimation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut };

            Storyboard showStoryboard = new Storyboard();
            showStoryboard.Children.Add(opacityAnimation);
            showStoryboard.Children.Add(translateAnimation);

            Storyboard.SetTarget(opacityAnimation, toast);
            Storyboard.SetTarget(translateAnimation, toast);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(translateAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));

            // Start the animation
            showStoryboard.Begin();

            // Schedule hide animation
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Tick += (sender, args) =>
            {
                timer.Stop();

                DoubleAnimation hideOpacityAnimation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
                DoubleAnimation hideTranslateAnimation = new DoubleAnimation(0, 50, TimeSpan.FromMilliseconds(300));
                hideTranslateAnimation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn };

                Storyboard hideStoryboard = new Storyboard();
                hideStoryboard.Children.Add(hideOpacityAnimation);
                hideStoryboard.Children.Add(hideTranslateAnimation);

                Storyboard.SetTarget(hideOpacityAnimation, toast);
                Storyboard.SetTarget(hideTranslateAnimation, toast);
                Storyboard.SetTargetProperty(hideOpacityAnimation, new PropertyPath("Opacity"));
                Storyboard.SetTargetProperty(hideTranslateAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));

                hideStoryboard.Completed += (s, e) =>
                {
                    // Remove from visual tree after animation
                    if (parentGrid != null)
                    {
                        parentGrid.Children.Remove(toast);
                    }
                    else
                    {
                        Panel panel = parent as Panel;
                        if (panel != null)
                        {
                            panel.Children.Remove(toast);
                        }
                    }
                };

                hideStoryboard.Begin();
            };
            timer.Start();
        }
    }
}
