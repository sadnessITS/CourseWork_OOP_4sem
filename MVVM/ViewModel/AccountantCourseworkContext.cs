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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-5R6G6IU;Database=AccountantCoursework;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => e.IdVisiting);

                entity.Property(e => e.IdVisiting)
                    .HasColumnName("ID_Visiting")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Diagnosis1)
                    .IsRequired()
                    .HasColumnName("Diagnosis")
                    .HasMaxLength(100);

                entity.Property(e => e.IdPatient).HasColumnName("ID_Patient");

                entity.Property(e => e.Medicine)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdPatientNavigation)
                    .WithMany(p => p.Diagnosis)
                    .HasForeignKey(d => d.IdPatient)
                    .HasConstraintName("FK_Diagnosis_Patient");

                entity.HasOne(d => d.MedicineNavigation)
                    .WithMany(p => p.Diagnosis)
                    .HasForeignKey(d => d.Medicine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Diagnosis_Medicine");
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.Medicine1);

                entity.Property(e => e.Medicine1)
                    .HasColumnName("Medicine")
                    .HasMaxLength(20);
                
                entity.Property(e => e.FrequencyVisits)
                    .HasColumnName("FrequencyVisits")
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient);

                entity.Property(e => e.IdPatient)
                    .HasColumnName("ID_Patient")
                    .ValueGeneratedNever();

                entity.Property(e => e.CopyPapers)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(50);

                entity.Property(e => e.Residency)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
