
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Linq;

namespace HMS.DesktopClient.ViewModels
{
    public class MedicalRecordHistoryViewModel
    {
        private readonly MedicalRecordProxy _medicalRecordProxy;

        public ObservableCollection<MedicalRecordDto> MedicalRecords { get; } = new();

        public MedicalRecordHistoryViewModel(int patientId, MedicalRecordProxy medicalRecordProxy)
        {
            _medicalRecordProxy = medicalRecordProxy;
            InitializeAsync(patientId);
        }

        private async Task InitializeAsync(int patientId)
        {
            try
            {
                var records = await _medicalRecordProxy.GetAllAsync();
                foreach (var record in records.Where(r => r.PatientId == patientId))
                {
                    MedicalRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                // Handle error appropriately (e.g., logging, UI notification)
                Console.WriteLine($"Error loading medical records: {ex.Message}");
            }
        }
    }
}