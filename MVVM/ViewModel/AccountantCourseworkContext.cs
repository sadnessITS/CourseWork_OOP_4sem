using HospitalPatientRecords.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public partial class AccountantCourseworkContext : DbContext
    {
        public AccountantCourseworkContext() { }

        public AccountantCourseworkContext(DbContextOptions<AccountantCourseworkContext> options) : base(options) { }

        public DbSet<Diagnosis> Diagnosis { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<MedicalCardHistory> MedicalCardHistory { get; set; }

        public DbSet<MedicalSpecialization> Medicine { get; set; }

        public DbSet<Patient> Patient { get; set; }

        public DbSet<DoctorVisitsFrequency> DoctorVisitsFrequency { get; set; }
    }
}
