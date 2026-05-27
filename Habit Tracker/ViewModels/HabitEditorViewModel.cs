using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HabitTracker.Helpers;
using HabitTracker.Models;

namespace HabitTracker.ViewModels
{
    public class HabitEditorViewModel : INotifyPropertyChanged
    {
        private HabitType _selectedType;
        private string _description;

        public HabitType SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged();
                    UpdateDescription();
                }
            }
        }

        public string Description
        {
            get => _description;
            private set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectRegularCommand { get; }
        public ICommand SelectBadCommand { get; }
        public ICommand SelectOneTimeCommand { get; }

        public HabitEditorViewModel()
        {
            // команды для кнопок
            SelectRegularCommand = new RelayCommand(() => SelectedType = HabitType.Regular);
            SelectBadCommand = new RelayCommand(() => SelectedType = HabitType.Bad);
            SelectOneTimeCommand = new RelayCommand(() => SelectedType = HabitType.OneTime);

            // значение по умолчанию
            SelectedType = HabitType.Regular;
        }

        private void UpdateDescription()
        {
            switch (SelectedType)
            {
                case HabitType.Regular:
                    Description = "Регулярная привычка будет появляться в списке каждый день.";
                    break;

                case HabitType.Bad:
                    Description = "Вредная привычка всегда считается выполненной, пока ты не нажмёшь и не отметишь срыв.";
                    break;

                case HabitType.OneTime:
                    Description = "Одноразовая привычка появится в списке только один раз.";
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}