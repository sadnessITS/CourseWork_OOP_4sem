using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalPatientRecords.MVVM.Model
{
    public class PatientVisitsFrequency
    {
        [Key]
        public int Id { get; set; }
        public string Frequency { get; set; }

        public int? IdDoctor { get; set; }

        [ForeignKey("IdDoctor")]
        public Doctor Doctor { get; set; }
    }
}
