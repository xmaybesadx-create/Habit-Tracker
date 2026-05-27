using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HabitTracker
{
    public partial class IconPickerWindow : Window
    {
        // Эти свойства читает CreateHabitWindow после закрытия окна
        public string SelectedEmoji { get; private set; }
        public string SelectedKey { get; private set; }

        public IconPickerWindow()
        {
            InitializeComponent();
            LoadIcons();
        }

        private void LoadIcons()
        {
            // Список эмодзи для выбора
            var icons = new List<(string emoji, string key)>
            {
                ("🙂", "smile"),
                ("💧", "water"),
                ("📚", "book"),
                ("🏃", "run"),
                ("🧘", "yoga"),
                ("☕", "coffee"),
                ("🍏", "apple"),
                ("🛏️", "sleep"),
                ("💪", "sport"),
                ("🎯", "goal"),
                ("🎨", "art"),
                ("🎵", "music")
            };

            // Находим UniformGrid в XAML и добавляем кнопки
            if (Content is ScrollViewer sv && sv.Content is UniformGrid grid)
            {
                foreach (var (emoji, key) in icons)
                {
                    var btn = new Button
                    {
                        Content = emoji,
                        FontSize = 26,
                        Width = 60,
                        Height = 60,
                        Margin = new Thickness(6),
                        Background = (Brush)new BrushConverter().ConvertFrom("#2F3846"),
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Transparent,
                        Tag = key,
                        Cursor = System.Windows.Input.Cursors.Hand
                    };
                    btn.Click += IconButton_Click;
                    grid.Children.Add(btn);
                }
            }
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                SelectedEmoji = btn.Content?.ToString();
                SelectedKey = btn.Tag?.ToString();
                DialogResult = true; // сигнал для ShowDialog() == true
                Close();
            }
        }
    }
}
