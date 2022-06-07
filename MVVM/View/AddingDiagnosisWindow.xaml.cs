using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddingDiagnosisWindow : Window
    {
        private AccountantCourseworkContext db;
        
        private Patient patient { get; set; }

        public AddingDiagnosisWindow(AccountantCourseworkContext dbContext, Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            db = dbContext;
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
        
        bool Validate(Diagnosis diagnosis)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(diagnosis);
            if (!Validator.TryValidateObject(diagnosis, context, results, true))
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
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            db = new AccountantCourseworkContext();
            List<Diagnosis> listDiagnosis = db.Diagnosis.ToList();

            object currentDoctorObject;
            if (!VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out currentDoctorObject))
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "We don't have info about doctor :(";
                mesWin.ShowDialog();
                return;
            }

            Doctor currentDoctor = (Doctor)currentDoctorObject;

            Doctor checkUser = db.Doctor
                .Where(o => o.Id == currentDoctor.Id)
                .FirstOrDefault();

            try
            {
                Diagnosis diagnosis = new Diagnosis();
                diagnosis.Id = (listDiagnosis.Count + 1);
                diagnosis.Patient = patient;
                diagnosis.Doctor = currentDoctor;
                diagnosis.DiagnosticResult = DiagnosisTextBox.Text;
                diagnosis.Date = DateTime.Now;
                
                if (Validate(diagnosis) == false) return;

                db.Diagnosis.Add(diagnosis);
                db.SaveChanges();
            
                MessageWindow mesWin = new MessageWindow();
                mesWin.TitleField.Text = "Congratulations!";
                mesWin.MessageField.Text = "Diagnosis was added!";
                mesWin.ShowDialog();
                this.Close();
            }
            catch
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Something was wrong!";
                mesWin.ShowDialog();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
