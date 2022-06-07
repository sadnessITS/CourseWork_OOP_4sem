using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class User
    {
        public User()
        {
            Diagnosis = new HashSet<Diagnosis>();
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        [Text]
        public string Login { get; set; }
        [Text]
        public string Password { get; set; }
        [Text]
        public string DoctorFio { get; set; }
        public string Medicine { get; set; }
        public int? Permission { get; set; }
        
        public virtual ICollection<Diagnosis> Diagnosis { get; set; }
        public virtual Medicine MedicineNavigation { get; set; }
    }
}
