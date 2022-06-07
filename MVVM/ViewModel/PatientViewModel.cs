using System.Threading.Tasks;
using HospitalPatientRecords.Core;

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public class PatientViewModel : ObservableObject
    {
        public int CurrentDoctor { get; set; }
        public string DoctorFio { get; set; }
    }
}