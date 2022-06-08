using System.ComponentModel.DataAnnotations;

namespace HospitalPatientRecords.MVVM.Model
{
    public class MedicalSpecialization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public MedicalSpecialization(string name)
        {
            Name = name;
        }
        
        public MedicalSpecialization() {}
        
        public override string ToString()
        {
            return Name;
        }
    }
}
