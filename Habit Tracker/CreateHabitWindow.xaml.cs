using System;
using System.Windows;
using System.Windows.Media;
using HabitTracker.Models;

namespace HabitTracker
{
    public partial class CreateHabitWindow : Window
    {
        private readonly HabitType _habitType;

        public string SelectedIconKey { get; private set; }
        public string SelectedIconEmoji { get; private set; }
        public string SelectedColorHex { get; private set; }
        public string SelectedTimeOfDay { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public bool[] SelectedDaysOfWeek { get; private set; }
        public int SelectedDaysPerMonth { get; private set; }
        public HabitScheduleMode SelectedScheduleMode { get; private set; }

        public CreateHabitWindow(HabitType habitType)
        {
            InitializeComponent();

            _habitType = habitType;

            // значения по умолчанию
            SelectedIconKey = "🙂";
            SelectedIconEmoji = "🙂";
            SelectedColorHex = "#2E7D32";
            SelectedTimeOfDay = "Anytime";
            SelectedDate = DateTime.Today;

            SelectedDaysOfWeek = new[] { true, true, true, true, true, true, true };
            SelectedDaysPerMonth = 5;
            SelectedScheduleMode = HabitScheduleMode.WeekDays;

            Title = $"Создание привычки ({habitType})";

            // текст под типом
            TypeTextBlock.Text = $"Тип: {habitType}";

            // иконка по умолчанию
            SelectedIconText.Text = SelectedIconEmoji;
            SelectedIconText.Foreground = Brushes.White;

            // цвет по умолчанию
            SelectedColorPreview.Background =
                (Brush)new BrushConverter().ConvertFrom(SelectedColorHex);
            ColorSubtitleText.Text = "Нажми, чтобы выбрать цвет";

            // дата по умолчанию
            DateButton.Content = SelectedDate.ToString("dd.MM.yyyy");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Плейсхолдер для текстбокса "Название привычки . . ."
        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "Название привычки . . .")
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = Brushes.White;
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameTextBox.Text = "Название привычки . . .";
                NameTextBox.Foreground =
                    (Brush)new BrushConverter().ConvertFrom("#C7CBD6");
            }
        }

        private void SelectIconButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new IconPickerWindow
            {
                Owner = this
            };

            var result = picker.ShowDialog();
            if (result == true)
            {
                SelectedIconEmoji = picker.SelectedEmoji;
                SelectedIconKey = picker.SelectedKey;

                SelectedIconText.Text = SelectedIconEmoji;
                SelectedIconText.Foreground = Brushes.White;
                IconSubtitleText.Text = "Нажми, чтобы изменить значок";
            }
        }

        private void SelectColorButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new ColorPickerWindow
            {
                Owner = this
            };

            var result = picker.ShowDialog();
            if (result == true)
            {
                SelectedColorHex = picker.SelectedColorHex;

                var brush =
                    (Brush)new BrushConverter().ConvertFrom(SelectedColorHex);
                SelectedColorPreview.Background = brush;
                ColorSubtitleText.Text = SelectedColorHex;
            }
        }

        private void DateButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем HabitDaysWindow с текущими настройками повтора
            var win = new HabitDaysWindow(
                SelectedDaysOfWeek,
                SelectedDaysPerMonth,
                SelectedScheduleMode)
            {
                Owner = this
            };

            var result = win.ShowDialog();
            if (result == true)
            {
                // Забираем значения, выбранные в HabitDaysWindow
                SelectedDaysOfWeek = win.DaysOfWeek;
                SelectedDaysPerMonth = win.DaysPerMonth;
                SelectedScheduleMode = win.Mode;

                // Обновляем текст в поле даты / повтора,
                // чтобы пользователь видел, что выбрал.
                // Пример простого отображения:
                if (SelectedScheduleMode == HabitScheduleMode.WeekDays)
                {
                    DateButton.Content = BuildDaysShortLabel(SelectedDaysOfWeek);
                }
                else
                {
                    DateButton.Content = $"{SelectedDaysPerMonth} дн. в месяц";
                }
            }
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
        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button btn)
                return;

            var mode = btn.Tag as string ?? "Anytime";
            SelectedTimeOfDay = mode;

            // Здесь можно подсветить выбранный бордер, если захочешь.
            // Пока просто меняем подпись блока "ВЫПОЛНИТЬ..." или показываем MessageBox при отладке.
            // MessageBox.Show($"Выбрано: {SelectedTimeOfDay}");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                name == "Название привычки . . .")
            {
                MessageBox.Show("Пожалуйста, введите название привычки.",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            var habit = new Habit
            {
                Name = name.Trim(),
                Type = _habitType,
                ColorHex = SelectedColorHex,
                IconKey = SelectedIconEmoji,
                StartDate = SelectedDate,
                ScheduleMode = SelectedScheduleMode,
                DaysOfWeek = SelectedDaysOfWeek,
                DaysPerMonth = SelectedScheduleMode == HabitScheduleMode.DaysPerMonth
                               ? SelectedDaysPerMonth
                               : 0
            };

            this.Tag = habit;
            this.DialogResult = true;
            this.Close();
        }
    }
}