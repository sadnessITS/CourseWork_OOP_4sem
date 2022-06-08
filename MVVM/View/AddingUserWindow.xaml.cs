using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class AddingUserWindow : Window
    {
        AccountantCourseworkContext dbContext;
        public AddingUserWindow(AccountantCourseworkContext dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;
            List<MedicalSpecialization> specializationsWithoutAdmin =
                dbContext.MedicalSpecialization.Where((sp) => sp.Name != "ADMIN").ToList();
            specializationsCombobox.ItemsSource = specializationsWithoutAdmin;
        }
        
        bool Validate(Doctor user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, context, results, true))
            {
                MessageWindow mesWin = new MessageWindow();
                
                string errors = "";
                foreach (var error in results)
                {
                    errors += error.ErrorMessage; errors += "\n";
                }

                errors = errors.Substring(0, errors.Length - 2);
                mesWin.Height += 13 * results.Count;
                mesWin.MessageField.Text = errors;
                mesWin.ShowDialog();
                return false;
            }
            else return true;
        }
        
        public string CalcHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash);
        }
        
        private void AddingUser()
        {
            List<Doctor> list = dbContext.Doctor.ToList();

            string hash = CalcHash(PasswordField.Text);

            MedicalSpecialization selectedSpecialization = specializationsCombobox.SelectedItem as MedicalSpecialization;
            
            if (selectedSpecialization == null)
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Select specialization!";
                mesWin.ShowDialog();
                return;
            }
            try
            {
                Doctor doctor = new Doctor();
                doctor.Login = LoginField.Text;
                doctor.Password = hash;
                doctor.Fio = DoctorFio.Text;
                doctor.Role = Role.USER;
                doctor.MedicalSpecialization = selectedSpecialization;
                
                if (Validate(doctor) == false) return;

                dbContext.Doctor.Add(doctor);
                dbContext.SaveChanges();

                MessageWindow mesWin = new MessageWindow();
                mesWin.TitleField.Text = "Congratulations!";
                mesWin.MessageField.Text = "Added!!!";
                mesWin.ShowDialog();
            }
            catch
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Incorrect information!";
                mesWin.ShowDialog();
            }
        }

        private void DrugWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        
        private void Minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddingUser();
        }
    }
}