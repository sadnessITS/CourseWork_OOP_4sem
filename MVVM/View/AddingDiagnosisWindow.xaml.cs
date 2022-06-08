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
    public partial class AddingDiagnosisWindow : Window
    {
        private AccountantCourseworkContext dbContext;
        
        private Patient patient { get; set; }

        public AddingDiagnosisWindow(AccountantCourseworkContext dbContext, Patient patient)
        {
            InitializeComponent();
            this.dbContext = dbContext;
            this.patient = patient;
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
        
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            List<Diagnosis> listDiagnosis = dbContext.Diagnosis.ToList();

            object currentDoctorObject;
            if (!VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out currentDoctorObject))
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "We don't have info about doctor :(";
                mesWin.ShowDialog();
                return;
            }
            Doctor currentDoctor = (Doctor)currentDoctorObject;

            Doctor checkDoctor = dbContext.Doctor
                .Where(o => o.Id == currentDoctor.Id)
                .FirstOrDefault();

            try
            {
                Diagnosis diagnosis = new Diagnosis();
                diagnosis.Patient = patient;
                diagnosis.Doctor = checkDoctor;
                diagnosis.DiagnosticResult = DiagnosisTextBox.Text;
                diagnosis.Date = DateTime.Now;

                dbContext.Diagnosis.Add(diagnosis);
            
                dbContext.SaveChanges();
            
                MessageWindow mesWin2 = new MessageWindow();
                mesWin2.TitleField.Text = "Congratulations!";
                mesWin2.MessageField.Text = "Diagnosis was added!";
                mesWin2.ShowDialog();
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
