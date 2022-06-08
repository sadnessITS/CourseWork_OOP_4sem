using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalPatientRecords.MVVM.Model
{
    public class MedicalCardHistory
    {
        [Key]
        public int Id { get; set; }

        [Id]
        public int CardNumber { get; set; }

        public int? IdPatient { get; set; }

        [Required]
        [ForeignKey("IdPatient")]
        public Patient Patient { get; set; }

        [Address]
        public string Address{ get; set; }

        public DateTime Date { get; set; }

        public string ActionWithCard { get; set; }
    }
}