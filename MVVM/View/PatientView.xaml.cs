using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class PatientView : UserControl
    {
        private AccountantCourseworkContext dbContext;
        public PatientView()
        {
            InitializeComponent();
            UpdateDb();
        }

        public void UpdateDb()
        {
            dbContext = new AccountantCourseworkContext();
            dbContext.Patient.Load();
            patientDataGrid.ItemsSource = dbContext.Patient.Local.ToBindingList();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddingPatientWindow addingForm = new AddingPatientWindow();
            addingForm.ShowDialog();
            UpdateDb();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            dbContext = new AccountantCourseworkContext();

            Patient itemDel = patientDataGrid.SelectedItem as Patient;
            
            if (itemDel != null)
            {
                Patient patientDel = dbContext.Patient
                    .Where(o => o.Id == itemDel.Id)
                    .FirstOrDefault();
                
                List<Diagnosis> diagnosisDel = dbContext.Diagnosis
                    .Where(o => o.IdPatient == itemDel.Id).ToList();

                foreach (var d in diagnosisDel)
                {
                    dbContext.Diagnosis.Remove(d);
                }

                dbContext.Patient.Remove(patientDel);
                dbContext.SaveChanges();
                
                UpdateDb();
            }
            else
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Patient not selected!";
                mesWin.ShowDialog();
            }

            //db.Patient.Remove();
        }

        private void Diagnosis_OnClick(object sender, RoutedEventArgs e)
        {
            dbContext = new AccountantCourseworkContext();

            Patient patient = patientDataGrid.SelectedItem as Patient;

            MedicalCardHistory medicalCardHistory = dbContext.MedicalCardHistory
                .OrderByDescending(history => history.Date)
                .FirstOrDefault((history) => history.Patient == patient);

            if (patient != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = Convert.ToString(patient.Id);
                diagnosisWindow.FioField.Text = patient.Fio;
                diagnosisWindow.AgeField.Text = Convert.ToString(patient.Age);
                diagnosisWindow.SexField.Text = patient.Sex;
                diagnosisWindow.ResidencyField.Text = patient.Residency;
                diagnosisWindow.CopyPapersField.Text = medicalCardHistory.Address;
                diagnosisWindow.UpdateDiagnosis();
                diagnosisWindow.ShowDialog();
                UpdateDb();
            }
            else
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Patient not selected!";
                mesWin.ShowDialog();
            }
        }

        private void PatientDatabase_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dbContext = new AccountantCourseworkContext();

            Patient patient = patientDataGrid.SelectedItem as Patient;

            MedicalCardHistory medicalCardHistory = dbContext.MedicalCardHistory
                .OrderByDescending(history => history.Date)
                .FirstOrDefault((history) => history.Patient == patient);

            if (patient != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = Convert.ToString(patient.Id);
                diagnosisWindow.FioField.Text = patient.Fio;
                diagnosisWindow.AgeField.Text = Convert.ToString(patient.Age);
                diagnosisWindow.SexField.Text = patient.Sex;
                diagnosisWindow.ResidencyField.Text = patient.Residency;
                diagnosisWindow.CopyPapersField.Text = medicalCardHistory.Address;
                diagnosisWindow.UpdateDiagnosis();
                diagnosisWindow.ShowDialog();
                UpdateDb();
            }
            else
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Patient not selected!";
                mesWin.ShowDialog();
            }
        }

        private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            dbContext = new AccountantCourseworkContext();

            var selectedList = dbContext.Patient.Where(checkSearchCriterias()).ToList();

            patientDataGrid.ItemsSource = selectedList;
        }

        private Expression<Func<Patient, bool>> checkSearchCriterias()
        {
            return p => p.Fio.Contains(Searcher.Text) || p.Residency.Contains(Searcher.Text);
        }
    }
}