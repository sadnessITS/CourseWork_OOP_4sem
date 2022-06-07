using HospitalPatientRecords.MVVM.Model;
using System.Data.Entity;

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public partial class AccountantCourseworkContext : DbContext
    {
        public AccountantCourseworkContext() : base("name = AccountantCourseworkEntities") { }
        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(DESKTOP-5R6G6IU);Database=AccountantCoursework;Trusted_Connection=True;");
        //}


        public DbSet<Diagnosis> Diagnosis { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<MedicalCardHistory> MedicalCardHistory { get; set; }

        public DbSet<MedicalSpecialization> MedicalSpecialization { get; set; }

        public DbSet<Patient> Patient { get; set; }

        public DbSet<DoctorVisitsFrequency> DoctorVisitsFrequency { get; set; }
    }
}
