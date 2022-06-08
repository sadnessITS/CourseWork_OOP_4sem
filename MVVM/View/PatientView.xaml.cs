using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

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

            List<MedicalCardHistory> selectedMedicalCardHistories = new List<MedicalCardHistory>();
            MedicalCardHistory medicalCardHistory = new MedicalCardHistory();
            
            foreach (var p in dbContext.Patient.ToList())
            {
                medicalCardHistory = dbContext.MedicalCardHistory
                    .OrderByDescending(history => history.Date)
                    .FirstOrDefault((history) => history.Patient.Id == p.Id);
                selectedMedicalCardHistories.Add(medicalCardHistory);
            }

            patientDataGrid.ItemsSource = selectedMedicalCardHistories;
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

            MedicalCardHistory itemDel = patientDataGrid.SelectedItem as MedicalCardHistory;
            
            if (itemDel != null)
            {
                Patient patientDel = dbContext.Patient
                    .Where(o => o.Id == itemDel.Patient.Id)
                    .FirstOrDefault();
                
                List<Diagnosis> diagnosisDel = dbContext.Diagnosis
                    .Where(o => o.Patient.Id == itemDel.Id).ToList();
                
                List<MedicalCardHistory> medicalCardDel = dbContext.MedicalCardHistory
                    .Where(o => o.Patient.Id == itemDel.Id).ToList();

                foreach (var d in diagnosisDel)
                {
                    dbContext.Diagnosis.Remove(d);
                }
                
                foreach (var m in medicalCardDel)
                {
                    dbContext.MedicalCardHistory.Remove(m);
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

            MedicalCardHistory medicalCardHistory = patientDataGrid.SelectedItem as MedicalCardHistory;
            
            if (medicalCardHistory != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = medicalCardHistory.Patient.Id.ToString();
                diagnosisWindow.FioField.Text = medicalCardHistory.Patient.Fio;
                diagnosisWindow.AgeField.Text = medicalCardHistory.Patient.Age.ToString();
                diagnosisWindow.SexField.Text = medicalCardHistory.Patient.Sex;
                diagnosisWindow.ResidencyField.Text = medicalCardHistory.Patient.Residency;
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

            MedicalCardHistory medicalCardHistory = patientDataGrid.SelectedItem as MedicalCardHistory;
            
            if (medicalCardHistory != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = medicalCardHistory.Patient.Id.ToString();
                diagnosisWindow.FioField.Text = medicalCardHistory.Patient.Fio;
                diagnosisWindow.AgeField.Text = medicalCardHistory.Patient.Age.ToString();
                diagnosisWindow.SexField.Text = medicalCardHistory.Patient.Sex;
                diagnosisWindow.ResidencyField.Text = medicalCardHistory.Patient.Residency;
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

            var selectedList = dbContext.MedicalCardHistory.Where(checkSearchCriterias()).ToList();

            patientDataGrid.ItemsSource = selectedList;
        }

        private Expression<Func<MedicalCardHistory, bool>> checkSearchCriterias()
        {
            return m => m.Patient.Fio.Contains(Searcher.Text) || m.Patient.Residency.Contains(Searcher.Text);
        }
    }
}