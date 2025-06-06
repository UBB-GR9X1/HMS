using HMS.DesktopClient.Utils;
using HMS.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HMS.DesktopClient.ViewModels
{
    public class LoggerViewModel : INotifyPropertyChanged
    {
        private readonly LoggerService _loggerService;

        // Observable collection to store and display logs
        public ObservableCollection<LogDto> logs { get; } = new ObservableCollection<LogDto>();

        // Available action types for filtering
        private ObservableCollection<string> _actionTypes = new ObservableCollection<string>();
        public ObservableCollection<string> actionTypes
        {
            get => _actionTypes;
            private set
            {
                _actionTypes = value;
                OnPropertyChanged();
            }
        }

        // UI binding properties
        private string _userIdInput = string.Empty;
        public string user_id_input
        {
            get => _userIdInput;
            set
            {
                if (_userIdInput != value)
                {
                    _userIdInput = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedActionType;
        public string selected_action_type
        {
            get => _selectedActionType;
            set
            {
                if (_selectedActionType != value)
                {
                    _selectedActionType = value;
                    System.Diagnostics.Debug.WriteLine($"Selected action type set to: {value}");
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _selectedTimestamp;
        public DateTime? selected_timestamp
        {
            get => _selectedTimestamp;
            set
            {
                if (_selectedTimestamp != value)
                {
                    _selectedTimestamp = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commands for UI interactions
        public ICommand loadAllLogsCommand { get; }
        public ICommand filterLogsByUserIdCommand { get; }
        public ICommand filterLogsByActionTypeCommand { get; }
        public ICommand filterLogsByTimestampCommand { get; }
        public ICommand applyAllFiltersCommand { get; }

        public LoggerViewModel(LoggerService loggerService)
        {
            _loggerService = loggerService;

            actionTypes = new ObservableCollection<string>();

            loadAllLogsCommand = new RelayCommand(async _ =>
            {
                var allLogs = await _loggerService.GetAllLogsAsync();
                UpdateLogsCollection(allLogs);
                await UpdateActionTypesAsync();
            });

            filterLogsByUserIdCommand = new RelayCommand(async _ =>
            {
                if (int.TryParse(user_id_input, out int userId))
                {
                    var filteredLogs = await _loggerService.GetLogsByUserIdAsync(userId);
                    UpdateLogsCollection(filteredLogs);
                }
            }, _ => true);

            filterLogsByActionTypeCommand = new RelayCommand(async _ =>
            {
                if (!string.IsNullOrEmpty(selected_action_type))
                {
                    System.Diagnostics.Debug.WriteLine($"Filtering by action type: {selected_action_type}");
                    var filteredLogs = await _loggerService.GetLogsByActionAsync(selected_action_type);
                    UpdateLogsCollection(filteredLogs);
                }
            }, _ => true);

            filterLogsByTimestampCommand = new RelayCommand(async _ =>
            {
                if (selected_timestamp.HasValue)
                {
                    var filteredLogs = await _loggerService.GetLogsByTimestampAsync(selected_timestamp.Value);
                    UpdateLogsCollection(filteredLogs);
                }
            }, _ => true);

            applyAllFiltersCommand = new RelayCommand(async _ =>
            {
                int? userId = null;
                if (int.TryParse(user_id_input, out int parsedUserId))
                {
                    userId = parsedUserId;
                }

                var filteredLogs = await _loggerService.GetLogsByMultipleFiltersAsync(
                    userId,
                    selected_action_type,
                    selected_timestamp);

                UpdateLogsCollection(filteredLogs);
            });
        }

        public async Task InitializeActionTypesAsync()
        {
            await UpdateActionTypesAsync();
        }

        private void UpdateLogsCollection(IEnumerable<LogDto> newLogs)
        {
            logs.Clear();
            foreach (var log in newLogs)
            {
                logs.Add(log);
            }
        }

        private async Task UpdateActionTypesAsync()
        {
            var types = await _loggerService.GetActionTypesAsync();

            actionTypes.Clear();
            foreach (var type in types)
            {
                actionTypes.Add(type);
            }

        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Simple relay command implementation
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}