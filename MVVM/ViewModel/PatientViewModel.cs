using System.Threading.Tasks;
using HospitalPatientRecords.Core;
using Microsoft.EntityFrameworkCore;

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public class PatientViewModel : ObservableObject
    {
        public int CurrentDoctor { get; set; }
        public string DoctorFio { get; set; }
    }
}