using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
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

            object db;

            VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.DB_CONTEXT, out db);

            dbContext = db as AccountantCourseworkContext;
            
            UpdateDb();
        }

        public void UpdateDb()
        {
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
            AddingPatientWindow addingForm = new AddingPatientWindow(dbContext);
            addingForm.ShowDialog();
            UpdateDb();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
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
                
                List<Appointment> appointmentDel = dbContext.Appointment
                    .Where(o => o.Patient.Id == itemDel.Id).ToList();

                foreach (var d in diagnosisDel)
                {
                    dbContext.Diagnosis.Remove(d);
                }
                
                foreach (var m in medicalCardDel)
                {
                    dbContext.MedicalCardHistory.Remove(m);
                }
                
                foreach (var a in appointmentDel)
                {
                    dbContext.Appointment.Remove(a);
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

        private void OpenDiagnosisView()
        {
            MedicalCardHistory medicalCardHistory = patientDataGrid.SelectedItem as MedicalCardHistory;
            
            if (medicalCardHistory != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow(dbContext, medicalCardHistory);
                
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
        
        private void Diagnosis_OnClick(object sender, RoutedEventArgs e) => OpenDiagnosisView();
        
        private void PatientDatabase_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) => OpenDiagnosisView();

        private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedList = dbContext.MedicalCardHistory.Where(checkSearchCriterias()).ToList();

            patientDataGrid.ItemsSource = selectedList;
        }

        private Expression<Func<MedicalCardHistory, bool>> checkSearchCriterias()
        {
            return m => m.Patient.Fio.Contains(Searcher.Text) || m.Patient.Address.Contains(Searcher.Text) || m.CardNumber.ToString().Contains(Searcher.Text);
        }
    }
}