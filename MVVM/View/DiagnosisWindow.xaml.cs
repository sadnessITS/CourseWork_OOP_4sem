using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class DiagnosisWindow : Window
{
    AccountantCourseworkContext dbContext;
    private List<Diagnosis> _diagnosisList;
    public DiagnosisWindow()
    {
        InitializeComponent();
    }
    
    public void UpdateDiagnosis()
    {
        dbContext = new AccountantCourseworkContext();
        int idPatient = Convert.ToInt32(IdPatientField.Text);

        //var combinedList = dbContext.Diagnosis.Where(d => d.Patient.Id == idPatient).Join(dbContext.Doctor,
        //    u => u.IdUser,
        //    c => c.Id,
        //    (u, c) => new
        //    {
        //        IdVisiting = u.IdDiagnosisVisiting,
        //        IdPatient = u.IdPatient,
        //        IdUser = c.Id,
        //        DoctorFio = c.Fio,
        //        Medicine = u.MedicalSpecialization,
        //        Diagnosis1 = u.Diagnosis1,
        //        Date = u.Date
        //    }).ToList();

        var listDiagnosisDataGrid = dbContext.Diagnosis.ToList();

        DiagnosisDatabase.ItemsSource = listDiagnosisDataGrid;

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
            .Where(o => o.Id == Convert.ToInt32(IdPatientField.Text))
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

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AddDiagnosis_Click(object sender, RoutedEventArgs e)
    {
        int patientId = Convert.ToInt32(IdPatientField.Text);
        Patient patient = dbContext.Patient.First(patient => patient.Id == patientId);
        AddingDiagnosisWindow addingDiagnosis = new AddingDiagnosisWindow(dbContext, patient);
        addingDiagnosis.ShowDialog();
        UpdateDiagnosis();
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