using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using DataGrid = System.Windows.Forms.DataGrid;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class DatabaseView : UserControl
    {
        AccountantCourseworkContext db;
        public DatabaseView()
        {
            InitializeComponent();
            UpdateDb();
        }

        public void UpdateDb()
        {
            db = new AccountantCourseworkContext();
            db.Patient.Load();
            PatientDatabase.ItemsSource = db.Patient.Local.ToBindingList();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddingPatientWindow addingForm = new AddingPatientWindow();
            addingForm.ShowDialog();
            UpdateDb();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            db = new AccountantCourseworkContext();

            Patient itemDel = PatientDatabase.SelectedItem as Patient;
            
            if (itemDel != null)
            {
                Patient patientDel = db.Patient
                    .Where(o => o.IdPatient == itemDel.IdPatient)
                    .FirstOrDefault();
                
                List<Diagnosis> diagnosisDel = db.Diagnosis
                    .Where(o => o.IdPatient == itemDel.IdPatient).ToList();

                foreach (var d in diagnosisDel)
                {
                    db.Diagnosis.Remove(d);
                }

                db.Patient.Remove(patientDel);
                db.SaveChanges();
                
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
            db = new AccountantCourseworkContext();

            Patient itemDiagnosis = PatientDatabase.SelectedItem as Patient;

            if (itemDiagnosis != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = Convert.ToString(itemDiagnosis.IdPatient);
                diagnosisWindow.FioField.Text = itemDiagnosis.Fio;
                diagnosisWindow.AgeField.Text = Convert.ToString(itemDiagnosis.Age);
                diagnosisWindow.SexField.Text = itemDiagnosis.Sex;
                diagnosisWindow.ResidencyField.Text = itemDiagnosis.Residency;
                diagnosisWindow.CopyPapersField.Text = itemDiagnosis.CopyPapers;
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
            db = new AccountantCourseworkContext();

            Patient itemDiagnosis = PatientDatabase.SelectedItem as Patient;

            if (itemDiagnosis != null)
            {
                DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
                diagnosisWindow.IdPatientField.Text = Convert.ToString(itemDiagnosis.IdPatient);
                diagnosisWindow.FioField.Text = itemDiagnosis.Fio;
                diagnosisWindow.AgeField.Text = Convert.ToString(itemDiagnosis.Age);
                diagnosisWindow.SexField.Text = itemDiagnosis.Sex;
                diagnosisWindow.ResidencyField.Text = itemDiagnosis.Residency;
                diagnosisWindow.CopyPapersField.Text = itemDiagnosis.CopyPapers;
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
            db = new AccountantCourseworkContext();

            var selectedList = db.Patient.Where(checkSearchCriterias()).ToList();

            PatientDatabase.ItemsSource = selectedList;
        }

        private Expression<Func<Patient, bool>> checkSearchCriterias()
        {
            return p => p.Fio.Contains(Searcher.Text) || p.Residency.Contains(Searcher.Text);
        }
    }
}