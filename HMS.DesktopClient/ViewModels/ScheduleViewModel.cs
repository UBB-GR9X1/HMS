using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace HMS.DesktopClient.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleProxy _scheduleProxy;

        // Collection to display schedules
        public ObservableCollection<ScheduleDto> Schedules { get; } = new ObservableCollection<ScheduleDto>();

        // Properties for the currently selected/edited schedule
        private ScheduleDto _selectedSchedule;
        public ScheduleDto SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                if (_selectedSchedule != value)
                {
                    _selectedSchedule = value;
                    // Copy data to editing properties when a schedule is selected
                    if (_selectedSchedule != null)
                    {
                        EditingDoctorId = _selectedSchedule.DoctorId;
                        EditingShiftId = _selectedSchedule.ShiftId;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EditingDoctorId));
                    OnPropertyChanged(nameof(EditingShiftId));
                    OnPropertyChanged(nameof(IsScheduleSelected));
                }
            }
        }

        // Properties for adding/editing schedules (can be bound to input fields in the UI)
        private int _editingDoctorId;
        public int EditingDoctorId
        {
            get => _editingDoctorId;
            set
            {
                if (_editingDoctorId != value)
                {
                    _editingDoctorId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _editingShiftId;
        public int EditingShiftId
        {
            get => _editingShiftId;
            set
            {
                if (_editingShiftId != value)
                {
                    _editingShiftId = value;
                    OnPropertyChanged();
                }
            }
        }

        // Helper property to indicate if a schedule is selected
        public bool IsScheduleSelected => SelectedSchedule != null;

        // Commands for UI buttons
        public ICommand AddScheduleCommand { get; }
        public ICommand UpdateScheduleCommand { get; }
        public ICommand DeleteScheduleCommand { get; }


        public ScheduleViewModel(ScheduleProxy scheduleProxy)
        {
            _scheduleProxy = scheduleProxy;

            // Initialize commands, linking them to the async methods
            AddScheduleCommand = new AsyncRelayCommand(AddScheduleAsync);
            UpdateScheduleCommand = new AsyncRelayCommand(UpdateScheduleAsync, () => IsScheduleSelected);
            DeleteScheduleCommand = new AsyncRelayCommand(DeleteScheduleAsync, () => IsScheduleSelected);
        }

        public async Task LoadSchedulesAsync()
        {
            Debug.WriteLine("Attempting to load schedules...");
            Schedules.Clear();
            try
            {
                var allSchedules = await _scheduleProxy.GetAllAsync();

                Debug.WriteLine($"Fetched {allSchedules?.Count() ?? 0} schedules.");

                if (App.CurrentUser?.Role == UserRole.Admin)
                {
                    Debug.WriteLine("User is Admin. Adding all schedules.");
                    if (allSchedules != null)
                    {
                         foreach (var schedule in allSchedules)
                        {
                            Schedules.Add(schedule);
                        }
                    }
                   
                }
                else if (App.CurrentUser?.Role == UserRole.Doctor)
                {
                    Debug.WriteLine("User is Doctor. Filtering schedules.");
                    int currentDoctorId = App.CurrentUser.Id;
                    if (allSchedules != null)
                    {
                        foreach (var schedule in allSchedules)
                        {
                            if (schedule.DoctorId == currentDoctorId)
                            {
                                Schedules.Add(schedule);
                            }
                        }
                    }
                }
                 Debug.WriteLine($"Schedules collection now has {Schedules.Count} items.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading schedules: {ex.Message}");
            }
        }

        // CRUD Operations

        public async Task AddScheduleAsync()
        {
             try
            {
                var newSchedule = new ScheduleDto
                {
                    DoctorId = EditingDoctorId,
                    ShiftId = EditingShiftId
                };

                var addedSchedule = await _scheduleProxy.AddAsync(newSchedule);
                Schedules.Add(addedSchedule);
                Debug.WriteLine($"Added schedule for DoctorId: {addedSchedule.DoctorId}, ShiftId: {addedSchedule.ShiftId}");
                // Clear the editing fields after adding
                ClearEditingFields();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding schedule: {ex.Message}");
                // TODO: Notify user of error
            }
        }

        public async Task UpdateScheduleAsync()
        {
             if (SelectedSchedule == null) return; // Cannot update if no schedule is selected

             try
            {
                var updatedSchedule = new ScheduleDto
                {
                    DoctorId = EditingDoctorId,
                    ShiftId = EditingShiftId
                };

                // The proxy UpdateAsync might require the original IDs, depending on implementation
                // Assuming it updates based on the IDs in the DTO passed
                bool success = await _scheduleProxy.UpdateAsync(updatedSchedule);

                if (success)
                {
                    // Find and update the schedule in the ObservableCollection
                    var scheduleToUpdate = Schedules.FirstOrDefault(s => s.DoctorId == updatedSchedule.DoctorId && s.ShiftId == updatedSchedule.ShiftId);
                    if (scheduleToUpdate != null)
                    {
                        // Update properties if necessary, or simply rely on fetching all again
                        // For simplicity, let's reload all schedules for now
                         await LoadSchedulesAsync(); // Reload all schedules to reflect changes
                    }
                    Debug.WriteLine($"Updated schedule for DoctorId: {updatedSchedule.DoctorId}, ShiftId: {updatedSchedule.ShiftId}");
                    ClearEditingFields();
                     SelectedSchedule = null; // Deselect after update
                }
                else
                {
                     Debug.WriteLine($"Failed to update schedule for DoctorId: {updatedSchedule.DoctorId}, ShiftId: {updatedSchedule.ShiftId}");
                     // TODO: Notify user of failure
                }
            }
             catch (Exception ex)
            {
                Debug.WriteLine($"Error updating schedule: {ex.Message}");
                // TODO: Notify user of error
            }
        }

        public async Task DeleteScheduleAsync()
        {
            if (SelectedSchedule == null) return; // Cannot delete if no schedule is selected

            try
            {
                bool success = await _scheduleProxy.DeleteAsync(SelectedSchedule.DoctorId, SelectedSchedule.ShiftId);

                if (success)
                {
                    // Remove the schedule from the ObservableCollection
                    Schedules.Remove(SelectedSchedule);
                    Debug.WriteLine($"Deleted schedule for DoctorId: {SelectedSchedule.DoctorId}, ShiftId: {SelectedSchedule.ShiftId}");
                    ClearEditingFields();
                    SelectedSchedule = null; // Deselect after deletion
                }
                else
                {
                    Debug.WriteLine($"Failed to delete schedule for DoctorId: {SelectedSchedule.DoctorId}, ShiftId: {SelectedSchedule.ShiftId}");
                    // TODO: Notify user of failure
                }
            }
            catch (Exception ex)
            {
                 Debug.WriteLine($"Error deleting schedule: {ex.Message}");
                 // TODO: Notify user of error
            }
        }

        // Helper method to clear input fields
        private void ClearEditingFields()
        {
            EditingDoctorId = 0; // Or a default value
            EditingShiftId = 0; // Or a default value
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 