using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class Medicine
    {
        public Medicine()
        {
            Diagnosis = new HashSet<Diagnosis>();
            User = new HashSet<User>();
        }

        public string Medicine1 { get; set; }
        public string FrequencyVisits { get; set; }

        public virtual ICollection<Diagnosis> Diagnosis { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
