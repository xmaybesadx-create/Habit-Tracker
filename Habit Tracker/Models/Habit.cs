using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace HabitTracker.Models
{
    public enum HabitScheduleMode
    {
        WeekDays,
        DaysPerMonth
    }

    public class Habit : INotifyPropertyChanged
    {
        private string? _name;
        private string? _colorHex;
        private string? _iconKey;
        private DateTime _startDate = DateTime.Today;
        private HabitType _type;
        private HabitScheduleMode _scheduleMode = HabitScheduleMode.WeekDays;
        private bool[] _daysOfWeek = new bool[7];
        private int _daysPerMonth;

        private bool _isCompletedToday;   // выполнена ли сегодня

        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string? ColorHex
        {
            get => _colorHex;
            set
            {
                if (_colorHex == value) return;
                _colorHex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ColorBrush));
            }
        }

        public string? IconKey
        {
            get => _iconKey;
            set
            {
                if (_iconKey == value) return;
                _iconKey = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public HabitType Type
        {
            get => _type;
            set
            {
                if (_type.Equals(value)) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public HabitScheduleMode ScheduleMode
        {
            get => _scheduleMode;
            set
            {
                if (_scheduleMode == value) return;
                _scheduleMode = value;
                OnPropertyChanged();
            }
        }

        // 0 = Пн, 1 = Вт, ..., 6 = Вс
        public bool[] DaysOfWeek
        {
            get => _daysOfWeek;
            set
            {
                _daysOfWeek = value ?? new bool[7];
                OnPropertyChanged();
            }
        }

        public int DaysPerMonth
        {
            get => _daysPerMonth;
            set
            {
                if (_daysPerMonth == value) return;
                _daysPerMonth = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompletedToday
        {
            get => _isCompletedToday;
            set
            {
                if (_isCompletedToday == value) return;
                _isCompletedToday = value;
                OnPropertyChanged();
            }
        }

        public Brush ColorBrush
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ColorHex))
                {
                    try
                    {
                        return (Brush)new BrushConverter().ConvertFrom(ColorHex);
                    }
                    catch { }
                }
                return new SolidColorBrush(Colors.Gray);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}