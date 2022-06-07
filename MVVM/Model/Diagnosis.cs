﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalPatientRecords.MVVM.Model
{
    public class Diagnosis
    {
        [Key]
        public int Id { get; set; }

        public int? IdPatient { get; set; }

        [ForeignKey("IdPatient")]
        public Patient Patient { get; set; }

        public int? IdUser { get; set; }

        [ForeignKey("IdUser")]
        public Doctor Doctor { get; set; }
        
        public string DiagnosticResult  { get; set; }
        
        public DateTime Date { get; set; }
    }
}
