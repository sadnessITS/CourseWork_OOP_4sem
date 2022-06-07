using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalPatientRecords.MVVM.Model
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        
        [Text]
        public string Login { get; set; }
        
        [Text]
        public string Password { get; set; }

        [Text]
        public string Fio { get; set; }

        public int? IdMedicalSpecialization { get; set; }

        [ForeignKey("IdMedicalSpecialization")]
        public MedicalSpecialization MedicalSpecialization { get; set; }

        public Role Role { get; set; }
    }
}
