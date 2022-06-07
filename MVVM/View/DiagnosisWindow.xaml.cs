using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HospitalPatientRecords.MVVM.View;

public partial class DiagnosisWindow : Window
{
    AccountantCourseworkContext db;
    private List<Diagnosis> _diagnosisList;
    public DiagnosisWindow()
    {
        InitializeComponent();
    }
    
    public void UpdateDiagnosis()
    {
        db = new AccountantCourseworkContext();
        int idPatient = Convert.ToInt32(IdPatientField.Text);

        _diagnosisList = db.Diagnosis.Where(d => d.IdPatient == idPatient).ToList();
        DiagnosisDatabase.ItemsSource = _diagnosisList;

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
        Patient checkPatient = db.Patient
            .Where(o => o.IdPatient == Convert.ToInt32(IdPatientField.Text))
            .FirstOrDefault();

        if (checkPatient.Fio != FioField.Text || checkPatient.Residency != ResidencyField.Text ||
            checkPatient.CopyPapers != CopyPapersField.Text)
        {
            try
            {
                checkPatient.Fio = FioField.Text;
                checkPatient.Residency = ResidencyField.Text;
                checkPatient.CopyPapers = CopyPapersField.Text;

                db.SaveChanges();

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
            db.SaveChanges();
        }
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void AddDiagnosis_Click(object sender, RoutedEventArgs e)
    {
        AddingDiagnosisWindow addingDiagnosis = new AddingDiagnosisWindow();
        addingDiagnosis.IdPatient = Convert.ToInt32(IdPatientField.Text); 
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