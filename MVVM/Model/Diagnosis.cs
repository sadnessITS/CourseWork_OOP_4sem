using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.Model
{
    public partial class Diagnosis
    {
        [Key]
        public int IdVisiting { get; set; }
        
        public int? IdPatient { get; set; }
        [ForeignKey("IdPatient")]
        public Patient Patient { get; set; }
        
        public int? IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User User { get; set; }
        
        public string Medicine { get; set; }
        
        public string DiagnosticResult  { get; set; }
        
        public DateTime Date { get; set; }

        public virtual Patient IdPatientNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual Medicine MedicineNavigation { get; set; }
    }
}
