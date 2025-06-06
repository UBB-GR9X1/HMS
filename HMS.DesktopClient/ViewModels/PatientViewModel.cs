using HMS.Shared.Entities;
using HMS.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class PatientViewModel
    {
        private readonly IPatientService _patient_service;
        public event PropertyChangedEventHandler? property_changed;
        private bool is_loading = false;

        public PatientViewModel(IPatientService patient_service)
        {
            this._patient_service = patient_service;
        }

        public async Task<bool> AddPatient(Patient patient)
        {
            try
            {
                this.is_loading = true;
                var added = await _patient_service.AddPatient(patient);
                return added;
            }
            finally { this.is_loading = false; }
        }
    }
}
