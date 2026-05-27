using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HabitTracker
{
    public partial class ColorPickerWindow : Window
    {
        // Это свойство читает CreateHabitWindow
        public string SelectedColorHex { get; private set; } = "#4180ff";

        public ColorPickerWindow()
        {
            InitializeComponent();
            LoadColors();
        }

        private void LoadColors()
        {
            // Твои 8 фиксированных цветов
            var colors = new List<string>
            {
                "#4180ff",
                "#244230",
                "#ffb926",
                "#5657e6",
                "#cb4544",
                "#35afeb",
                "#ff7b4a",
                "#2ea8b7"
            };

            if (Content is UniformGrid grid)
            {
                grid.Children.Clear();

                foreach (var hex in colors)
                {
                    var brush = (Brush)new BrushConverter().ConvertFrom(hex);

                    var btn = new Button
                    {
                        Width = 52,
                        Height = 52,
                        Margin = new Thickness(6),
                        Background = brush,
                        BorderBrush = Brushes.Transparent,
                        Tag = hex,
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    // скруглённая кнопка
                    btn.Template = CreateRoundButtonTemplate(brush);
                    btn.Click += ColorButton_Click;
                    grid.Children.Add(btn);
                }
            }
        }

        private ControlTemplate CreateRoundButtonTemplate(Brush background)
        {
            var template = new ControlTemplate(typeof(Button));
            var factory = new FrameworkElementFactory(typeof(Border));
            factory.SetValue(Border.BackgroundProperty, background);
            factory.SetValue(Border.CornerRadiusProperty, new CornerRadius(12));
            template.VisualTree = factory;
            return template;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string hex)
            {
                SelectedColorHex = hex;
                DialogResult = true;
                Close();
            }
        }
    }
}