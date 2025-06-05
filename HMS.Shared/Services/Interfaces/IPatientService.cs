using HMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services.Interfaces
{
    public interface IPatientService
    {
        Task<bool> AddPatient(Patient patient);
    }
}
