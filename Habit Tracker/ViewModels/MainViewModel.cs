using Habit_Tracker;
using HabitTracker.Helpers;
using HabitTracker.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace HabitTracker.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Habit> Habits { get; }
        public ICollectionView HabitsView { get; }

        public ICommand CreateCommand { get; }

        public MainViewModel()
        {
            Habits = new ObservableCollection<Habit>();

            // Вьюха с сортировкой: сначала невыполненные, потом выполненные
            HabitsView = CollectionViewSource.GetDefaultView(Habits);
            HabitsView.SortDescriptions.Add(
                new SortDescription(nameof(Habit.IsCompletedToday), ListSortDirection.Ascending));

            CreateCommand = new RelayCommand(CreateHabit);

            // Примеры для теста (можно убрать)
            // Habits.Add(new Habit { Name = "Вода", ColorHex = "#4180ff", IconKey = "💧" });
            // Habits.Add(new Habit { Name = "Чтение", ColorHex = "#ffb926", IconKey = "📚" });
        }

        private void CreateHabit()
        {
            // 1. Сначала окно выбора типа (HabitEditorWindow)
            var editor = new HabitEditorWindow
            {
                Owner = App.Current.MainWindow
            };

            var editorResult = editor.ShowDialog();
            if (editorResult != true)
                return; // пользователь закрыл/отменил

            // Забираем выбранный тип из Tag
            if (editor.Tag is not HabitType selectedType)
                return; // на всякий случай, если что-то пошло не так

            // 2. Теперь открываем CreateHabitWindow с выбранным типом
            var win = new CreateHabitWindow(selectedType)
            {
                Owner = App.Current.MainWindow
            };

            if (win.ShowDialog() == true && win.Tag is Habit habit)
            {
                Habits.Add(habit);
                HabitsView.Refresh();
            }
        }
    }
    
}