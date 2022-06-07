using System.ComponentModel.DataAnnotations;

namespace HospitalPatientRecords.MVVM.Model
{
    public class Patient
    {
        [Id]
        [Key]
        public int Id { get; set; }

        [Fio]
        public string Fio { get; set; }

        [Age]
        public int Age { get; set; }

        public string Sex { get; set; }

        [Residency]
        public string Residency { get; set; }
    }
}
