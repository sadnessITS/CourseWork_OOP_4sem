using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class Patient
    {
        public Patient()
        {
            Diagnosis = new HashSet<Diagnosis>();
        }

        [Id]
        public int IdPatient { get; set; }
        [Fio]
        public string Fio { get; set; }
        [Age]
        public int Age { get; set; }
        public string Sex { get; set; }
        [Residency]
        public string Residency { get; set; }
        [CopyPapers]
        public string CopyPapers { get; set; }

        public virtual ICollection<Diagnosis> Diagnosis { get; set; }
    }
}
