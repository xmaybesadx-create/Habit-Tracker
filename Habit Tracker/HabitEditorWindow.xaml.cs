using System.Windows;
using HabitTracker.ViewModels;
using HabitTracker.Models;

namespace HabitTracker
{
    public partial class HabitEditorWindow : Window
    {
        public HabitEditorViewModel ViewModel { get; }

        public HabitEditorWindow()
        {
            InitializeComponent();
            ViewModel = new HabitEditorViewModel();
            DataContext = ViewModel;
        }

        // Это вызывается по кнопке "Далее" / "Создать"
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // Берём выбранный тип из ViewModel
            HabitType selected = ViewModel.SelectedType;

            // Кладём его в Tag и закрываем окно с DialogResult = true
            this.Tag = selected;
            this.DialogResult = true;
            this.Close();
        }
    }
}   