using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class AddingPatientWindow : Window
    {
        AccountantCourseworkContext dbContext;
        public AddingPatientWindow()
        {
            InitializeComponent();
        }
        
        bool Validate(Patient patient)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(patient);
            if (!Validator.TryValidateObject(patient, context, results, true))
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
        private void AddingPatient()
        {
            dbContext = new AccountantCourseworkContext();

            string sex;

            if (this.Man.IsChecked == true) sex = Convert.ToString(this.Man.Content);
            else if (this.Woman.IsChecked == true) sex = Convert.ToString(this.Woman.Content);
            else
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Sex not selected!";
                mesWin.ShowDialog();
                return;
            }
            
            try
            {

                Patient patient = new Patient();
                patient.Id = Convert.ToInt32(IdPatient.Text);
                patient.Fio = Fio.Text;
                patient.Age = Convert.ToInt32(Age.Text);
                patient.Sex = sex;
                patient.Residency = Residency.Text;

                MedicalCardHistory medicalCardHistory = new MedicalCardHistory()
                {
                    Address = medicalCardAddressTextBox.Text,
                    Date = DateTime.Now,
                    ActionWithCard = "Registered",
                    Patient = patient
                };
                
                if (Validate(patient) == false) return;

                dbContext.Patient.Add(patient);
                dbContext.SaveChanges();
            
                MessageWindow mesWin = new MessageWindow();
                mesWin.TitleField.Text = "Congratulations!";
                mesWin.MessageField.Text = "Patient was added!";
                mesWin.ShowDialog();
            }
            catch
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Incorrect ID!";
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
            AddingPatient();
        }
    }
}