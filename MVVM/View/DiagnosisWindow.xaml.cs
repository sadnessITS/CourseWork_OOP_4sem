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
    public DiagnosisWindow()
    {
        InitializeComponent();
    }
    
    public void UpdateDiagnosis()
    {
        db = new AccountantCourseworkContext();
        int idPatient = Convert.ToInt32(IdPatientField.Text);

        //var editableList = db.Diagnosis.Where(d => d.IdPatient == idPatient).ToList();
        //DiagnosisDatabase.ItemsSource = editableList;
        
        db.Diagnosis.Where(d => d.IdPatient == idPatient).Load();
        DiagnosisDatabase.ItemsSource = db.Diagnosis.Local.ToBindingList();

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

        if (checkPatient.Fio != IdPatientField.Text || checkPatient.Residency != ResidencyField.Text ||
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
                mesWin.MessageField.Text = "Sorry, but it seems to be impossible :(";
                mesWin.ShowDialog();
            }
        }
        else db.SaveChanges();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}