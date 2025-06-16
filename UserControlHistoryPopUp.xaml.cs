﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CHAT_BOT
{
    public partial class UserControlHistoryPopUp : UserControl
    {
        public event EventHandler<HistoryItem> HistoryItemClicked;
        public event EventHandler CloseAnimationCompleted;
        public event EventHandler OpenAnimationCompleted;
        
        public ObservableCollection<HistoryItem> historyItemsSource { get; set; } = new ObservableCollection<HistoryItem>();

        public UserControlHistoryPopUp()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void HistoryItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is HistoryItem historyItem)
            {
                HistoryItemClicked?.Invoke(this, historyItem);
            }
        }

        public void BeginCloseAnimation()
        {
            Storyboard closeAnimation = Resources["PopupCloseAnimation"] as Storyboard;
            closeAnimation?.Begin(this);
        }

        private void CloseAnimation_Completed(object sender, EventArgs e)
        {
            CloseAnimationCompleted?.Invoke(this, EventArgs.Empty);
        }
        
        private void OpenAnimation_Completed(object sender, EventArgs e)
        {
            OpenAnimationCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    public class HistoryItem
    {
        public string Heading { get; set; }
    }
}
