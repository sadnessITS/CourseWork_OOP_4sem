using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class DiagnosisWindow : Window
{
    AccountantCourseworkContext dbContext;

    private MedicalCardHistory medicalCardHistory;
    
    private List<Diagnosis> _diagnosisList;
    public DiagnosisWindow(AccountantCourseworkContext dbContext, MedicalCardHistory medicalCardHistory)
    {
        InitializeComponent();

        this.dbContext = dbContext;
        this.medicalCardHistory = medicalCardHistory;
        
        CardNumberField.Text = medicalCardHistory.CardNumber.ToString();
        FioField.Text = medicalCardHistory.Patient.Fio;
        AgeField.Text = medicalCardHistory.Patient.Age.ToString();
        SexField.Text = medicalCardHistory.Patient.Sex;
        ResidencyField.Text = medicalCardHistory.Patient.Residency;
        CopyPapersField.Text = medicalCardHistory.Address;
    }
    
    public void UpdateDiagnosis()
    {
        List<Diagnosis> selectedDiagnoses = dbContext.Diagnosis.ToList();
            
        foreach (var d in selectedDiagnoses)
        {
            d.Doctor = dbContext.Doctor.Where(item => item.Id == d.IdDoctor).FirstOrDefault();

            foreach (var dm in selectedDiagnoses)
            {
                dm.Doctor.MedicalSpecialization = dbContext.MedicalSpecialization.Where(item => item.Id == dm.Doctor.IdMedicalSpecialization).FirstOrDefault();
            }
        }
        
        diagnosisDataGrid.ItemsSource = selectedDiagnoses;
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
        Patient checkPatient = dbContext.Patient
            .Where(o => o.Id == medicalCardHistory.Patient.Id)
            .FirstOrDefault();
    
        if (checkPatient.Fio != FioField.Text || checkPatient.Residency != ResidencyField.Text)
        {
            try
            {
                checkPatient.Fio = FioField.Text;
                checkPatient.Residency = ResidencyField.Text;
    
                dbContext.SaveChanges();
    
                MessageWindow mesWin = new MessageWindow();
                mesWin.TitleField.Text = "Congratulations!";
                mesWin.MessageField.Text = "Info was changed!";
                mesWin.ShowDialog();
            }
            catch
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "It seems to be impossible :(";
                mesWin.ShowDialog();
            }
        }
        else
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "Info about Patient wasn't changed.\nTable saved!";
            mesWin.ShowDialog();
            dbContext.SaveChanges();
        }
    }

    private void AddDiagnosis_Click(object sender, RoutedEventArgs e)
    {
        int patientId = medicalCardHistory.Patient.Id;
        
        AddingDiagnosisWindow addingDiagnosis = new AddingDiagnosisWindow(dbContext, medicalCardHistory.Patient);
        addingDiagnosis.ShowDialog();
        UpdateDiagnosis();
    }

    private void DeleteDiagnosis_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Diagnosis itemSelect = diagnosisDataGrid.SelectedItem as Diagnosis;
            var remove = dbContext.Diagnosis.Where(d => d.Id == itemSelect.Id).FirstOrDefault();

            dbContext.Diagnosis.Remove(remove);
            dbContext.SaveChanges();
        
            UpdateDiagnosis();
        }
        catch
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "Something was wrong!";
            mesWin.ShowDialog();
        }
    }
    
    private void CardHistory_OnClick(object sender, RoutedEventArgs e)
    {
        CardHistoryView cardHistoryView = new CardHistoryView(dbContext, medicalCardHistory.Patient);
        cardHistoryView.ShowDialog();
    }
    
    // private void ExpandList(string medicine)
    // {
    //     int idPatient = Convert.ToInt32(IdPatientField.Text);
    //     var tempList = db.Diagnosis.Where(d => d.IdPatient == idPatient && d.Medicine == medicine).ToList();
    //     if (tempList.Count >= 1) _diagnosisList.AddRange(tempList);
    // }
    //
    // private void ReduceList(string medicine)
    // {
    //     int idPatient = Convert.ToInt32(IdPatientField.Text);
    //     var tempList = db.Diagnosis.Where(d => d.IdPatient == idPatient && d.Medicine == medicine).ToList();
    //     if (tempList.Count != _diagnosisList.Count) _diagnosisList.RemoveRange(_diagnosisList.Count - 1, tempList.Count);
    // }
    //
    // private void DentistCheckBox_OnChecked(object sender, RoutedEventArgs e) => ExpandList("Dentist");
    // private void DentistCheckBox_OnUnchecked(object sender, RoutedEventArgs e) => ReduceList("Dentist");
    // private void ENT_OnChecked(object sender, RoutedEventArgs e) => ExpandList("ENT");
    // private void ENT_OnUnchecked(object sender, RoutedEventArgs e) => ReduceList("ENT");
    // private void Psychiatrist_OnChecked(object sender, RoutedEventArgs e) => ExpandList("Psychiatrist");
    // private void Psychiatrist_OnUnchecked(object sender, RoutedEventArgs e) => ReduceList("Psychiatrist");
    // private void Surgeon_OnChecked(object sender, RoutedEventArgs e) => ExpandList("Surgeon");
    // private void Surgeon_OnUnchecked(object sender, RoutedEventArgs e) => ReduceList("Surgeon");
    // private void Therapist_OnChecked(object sender, RoutedEventArgs e) => ExpandList("Therapist");
    // private void Therapist_OnUnchecked(object sender, RoutedEventArgs e) => ReduceList("Therapist");
    
}