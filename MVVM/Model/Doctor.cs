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
        [Required]
        public MedicalSpecialization MedicalSpecialization { get; set; }

        public Role Role { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Doctor doctor = obj as Doctor;
            
            if (doctor == null)
                return false;

            if (doctor.Id == Id &&
                doctor.Login == Login &&
                doctor.Password == Password &&
                doctor.Fio == Fio &&
                doctor.Role == Role)
                return true;

            return false;
        }
    }
}
