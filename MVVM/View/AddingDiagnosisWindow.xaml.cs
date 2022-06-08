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
        private AccountantCourseworkContext dbContext;
        
        private Patient patient { get; set; }

        public AddingDiagnosisWindow(Patient patient)
        {
            InitializeComponent();
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
            dbContext = new AccountantCourseworkContext();
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

            Patient checkPatient = dbContext.Patient
                .Where(p => p.Id == patient.Id)
                .FirstOrDefault();
            
            Diagnosis diagnosis = new Diagnosis();
            diagnosis.Patient = checkPatient;
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

            try
            {
                
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
