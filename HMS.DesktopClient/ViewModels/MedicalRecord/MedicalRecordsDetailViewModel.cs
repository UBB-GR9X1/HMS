using HMS.Shared.DTOs;

namespace HMS.DesktopClient.ViewModels
{
    public class MedicalRecordsDetailViewModel
    {
        public MedicalRecordDto MedicalRecord { get; }

        public MedicalRecordsDetailViewModel(MedicalRecordDto medicalRecord)
        {
            MedicalRecord = medicalRecord;
        }
    }
}