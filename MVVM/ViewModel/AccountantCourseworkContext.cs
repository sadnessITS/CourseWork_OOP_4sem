using System;
using System.Threading.Tasks;
using HospitalPatientRecords.Core;
using HospitalPatientRecords.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public partial class AccountantCourseworkContext : DbContext
    {
        public AccountantCourseworkContext()
        {
        }

        public AccountantCourseworkContext(DbContextOptions<AccountantCourseworkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diagnosis> Diagnosis { get; set; }
        public virtual DbSet<Medicine> Medicine { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
