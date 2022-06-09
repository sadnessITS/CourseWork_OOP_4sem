using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalPatientRecords.MVVM.Model
{
    public class DoctorVisitsFrequency
    {
        [Key]
        public int Id { get; set; }
        public int Frequency { get; set; } // через сколько дней уведомлять

        public int? IdPatient { get; set; }

        [ForeignKey("IdPatient")]
        public Patient Patient { get; set; }

        public int? IdMedicalSpecialization { get; set; }

        [ForeignKey("IdMedicalSpecialization")]
        public MedicalSpecialization MedicalSpecialization { get; set; }
    }
}
