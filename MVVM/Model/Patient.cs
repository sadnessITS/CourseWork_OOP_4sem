using System.ComponentModel.DataAnnotations;

namespace HospitalPatientRecords.MVVM.Model
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Fio]
        public string Fio { get; set; }

        [Age]
        public int Age { get; set; }

        public string Sex { get; set; }

        [Address]
        public string Address { get; set; }

        public string email { get; set; }
    }
}
