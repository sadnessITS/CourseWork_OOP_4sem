using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class MedicalSpecialization
    {
        public int IdMedicine { get; set; }
        public string MedicineName { get; set; }
        public string FrequencyVisits { get; set; }

    }
}
