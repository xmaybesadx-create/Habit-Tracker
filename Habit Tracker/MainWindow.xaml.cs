using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HabitTracker.Models;
using HabitTracker.ViewModels;

namespace HabitTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        // Клик по кружку выполнения
        private void CompleteCircle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Habit habit)
            {
                habit.IsCompletedToday = !habit.IsCompletedToday;

                // обновляем сортировку, чтобы карточка уехала вниз
                if (DataContext is MainViewModel vm)
                {
                    vm.HabitsView.Refresh();
                }
                else
                {
                    // fallback: через DefaultView
                    CollectionViewSource.GetDefaultView(habit).Refresh();
                }
            }
        }
    }
}