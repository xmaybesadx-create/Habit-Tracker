using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using HabitTracker.Models;

namespace HabitTracker
{
    public partial class HabitDaysWindow : Window
    {
        public bool[] DaysOfWeek { get; }
        public int DaysPerMonth { get; private set; }
        public HabitScheduleMode Mode { get; private set; }

        public HabitDaysWindow(bool[] initialDays, int initialDaysPerMonth = 0, HabitScheduleMode initialMode = HabitScheduleMode.WeekDays)
        {
            InitializeComponent();

            DaysOfWeek = initialDays != null && initialDays.Length == 7
                ? initialDays.ToArray()
                : Enumerable.Repeat(true, 7).ToArray();

            DaysPerMonth = initialDaysPerMonth > 0 ? initialDaysPerMonth : 5;

            // если явно передан режим или есть DaysPerMonth, стартуем с DaysPerMonth, иначе по дням недели
            Mode = initialMode != HabitScheduleMode.WeekDays || initialDaysPerMonth > 0
                ? HabitScheduleMode.DaysPerMonth
                : HabitScheduleMode.WeekDays;

            InitDaysButtons();
            InitMonthDaysButtons();

            UpdateDaysSubtitle();
            UpdateMonthDaysSubtitle();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        // ---------- КАРТОЧКА 1: Определённые дни недели ----------

        private void DaysCardBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // переключаем свою панель
            bool willShow = DaysInnerPanel.Visibility != Visibility.Visible;

            DaysInnerPanel.Visibility = willShow ? Visibility.Visible : Visibility.Collapsed;

            // активируем режим дней недели
            Mode = HabitScheduleMode.WeekDays;

            // сворачиваем вторую карточку
            MonthDaysInnerPanel.Visibility = Visibility.Collapsed;

            UpdateDaysSubtitle();
            UpdateMonthDaysSubtitle();
        }

        private void InitDaysButtons()
        {
            MonButton.IsChecked = DaysOfWeek[0];
            TueButton.IsChecked = DaysOfWeek[1];
            WedButton.IsChecked = DaysOfWeek[2];
            ThuButton.IsChecked = DaysOfWeek[3];
            FriButton.IsChecked = DaysOfWeek[4];
            SatButton.IsChecked = DaysOfWeek[5];
            SunButton.IsChecked = DaysOfWeek[6];

            UpdateDaysButtonsStyle();
        }

        private void DayToggle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton btn && int.TryParse(btn.Tag?.ToString(), out int index))
            {
                DaysOfWeek[index] = btn.IsChecked == true;

                Mode = HabitScheduleMode.WeekDays;
                // при взаимодействии с днями недели сворачиваем блок "дни в месяц"
                MonthDaysInnerPanel.Visibility = Visibility.Collapsed;

                UpdateDaysButtonsStyle();
                UpdateDaysSubtitle();
                UpdateMonthDaysSubtitle();
            }
        }

        private void UpdateDaysButtonsStyle()
        {
            void StyleButton(ToggleButton btn, bool isOn)
            {
                var onBrush = (Brush)new BrushConverter().ConvertFrom("#2196F3");
                var offBrush = (Brush)new BrushConverter().ConvertFrom("#2F3846");

                btn.Background = isOn ? onBrush : offBrush;
                btn.Foreground = Brushes.White;
                btn.BorderBrush = Brushes.Transparent;
                btn.Width = 40;
                btn.Height = 40;
                btn.Margin = new Thickness(4);
                btn.Cursor = System.Windows.Input.Cursors.Hand;
                btn.FontWeight = FontWeights.SemiBold;
            }

            StyleButton(MonButton, DaysOfWeek[0]);
            StyleButton(TueButton, DaysOfWeek[1]);
            StyleButton(WedButton, DaysOfWeek[2]);
            StyleButton(ThuButton, DaysOfWeek[3]);
            StyleButton(FriButton, DaysOfWeek[4]);
            StyleButton(SatButton, DaysOfWeek[5]);
            StyleButton(SunButton, DaysOfWeek[6]);
        }

        private void UpdateDaysSubtitle()
        {
            DaysModeSubtitle.Text = BuildDaysShortLabel(DaysOfWeek);
        }

        private string BuildDaysShortLabel(bool[] days)
        {
            if (days == null || days.Length != 7)
                return "Каждый день";

            string[] labels = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
            var selected = new System.Collections.Generic.List<string>();

            for (int i = 0; i < 7; i++)
            {
                if (days[i])
                    selected.Add(labels[i]);
            }

            if (selected.Count == 7)
                return "Каждый день";
            if (selected.Count == 0)
                return "Без повторения";

            return string.Join(", ", selected);
        }

        // ---------- КАРТОЧКА 2: Х д. в месяц ----------

        private void MonthDaysCardBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // переключаем свою панель
            bool willShow = MonthDaysInnerPanel.Visibility != Visibility.Visible;

            MonthDaysInnerPanel.Visibility = willShow ? Visibility.Visible : Visibility.Collapsed;

            // активируем режим "дни в месяц"
            Mode = HabitScheduleMode.DaysPerMonth;

            // сворачиваем первую карточку
            DaysInnerPanel.Visibility = Visibility.Collapsed;

            UpdateMonthDaysSubtitle();
            UpdateDaysSubtitle();
        }

        private void InitMonthDaysButtons()
        {
            foreach (var child in MonthDaysButtonsPanel.Children)
            {
                if (child is Button btn && int.TryParse(btn.Tag?.ToString(), out int value))
                {
                    StyleNumberButton(btn, value == DaysPerMonth);
                }
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int value))
            {
                DaysPerMonth = value;
                Mode = HabitScheduleMode.DaysPerMonth;

                // при выборе числа явно выбираем режим "дни в месяц"
                DaysInnerPanel.Visibility = Visibility.Collapsed;

                foreach (var child in MonthDaysButtonsPanel.Children)
                {
                    if (child is Button b && int.TryParse(b.Tag?.ToString(), out int v))
                    {
                        StyleNumberButton(b, v == DaysPerMonth);
                    }
                }

                UpdateMonthDaysSubtitle();
                UpdateDaysSubtitle();
            }
        }

        private void StyleNumberButton(Button btn, bool isSelected)
        {
            var onBrush = (Brush)new BrushConverter().ConvertFrom("#2196F3");
            var offBrush = (Brush)new BrushConverter().ConvertFrom("#2F3846");

            btn.Width = 48;
            btn.Height = 48;
            btn.Margin = new Thickness(4);
            btn.Background = isSelected ? onBrush : offBrush;
            btn.Foreground = Brushes.White;
            btn.BorderBrush = Brushes.Transparent;
            btn.FontWeight = isSelected ? FontWeights.Bold : FontWeights.Normal;
            btn.Cursor = System.Windows.Input.Cursors.Hand;
        }

        private void UpdateMonthDaysSubtitle()
        {
            MonthDaysSubtitle.Text = $"{DaysPerMonth} дн. в месяц";
        }
    }
}