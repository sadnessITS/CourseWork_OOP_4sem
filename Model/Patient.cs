using System.ComponentModel.DataAnnotations;

namespace HospitalPatientRecords.MVVM.Model
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public string Fio { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }

        public string email { get; set; }
    }
}
