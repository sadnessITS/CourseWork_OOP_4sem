using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class Diagnosis
    {
        public int IdVisiting { get; set; }
        public int? IdPatient { get; set; }
        public int IdUser { get; set; }
        public string Medicine { get; set; }
        public string Diagnosis1 { get; set; }
        public DateTime Date { get; set; }
        public string DoctorFio { get; set; }

        public virtual Patient IdPatientNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual Medicine MedicineNavigation { get; set; }
    }
}
