using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using System.Windows.Controls;
using System;

namespace HospitalPatientRecords.MVVM.View;

public partial class DiagnosisWindow : Window
{
    AccountantCourseworkContext dbContext;

    private MedicalCardHistory medicalCardHistory;

    public DiagnosisWindow(AccountantCourseworkContext dbContext, MedicalCardHistory medicalCardHistory)
    {
        InitializeComponent();

        this.dbContext = dbContext;
        this.medicalCardHistory = medicalCardHistory;
        
        CardNumberField.Text = medicalCardHistory.CardNumber.ToString();
        FioField.Text = medicalCardHistory.Patient.Fio;
        AgeField.Text = medicalCardHistory.Patient.Age.ToString();
        SexField.Text = medicalCardHistory.Patient.Sex;
        AddressField.Text = medicalCardHistory.Patient.Address;
        CopyPapersField.Text = medicalCardHistory.Address;

        checkIfFrequencyRecordExistsAndSetButtonName();
    }

    private void checkIfFrequencyRecordExistsAndSetButtonName()
    {
        object doctorObject;

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out doctorObject);

        Doctor currentDoctor = doctorObject as Doctor;

        DoctorVisitsFrequency frequency = dbContext.DoctorVisitsFrequency
            .FirstOrDefault(fr => fr.Patient.Id == medicalCardHistory.Patient.Id &&
                                  fr.MedicalSpecialization.Id == currentDoctor.MedicalSpecialization.Id);

        if (frequency == null)
            notificationSwitcher.Content = "Notify";
        else
            notificationSwitcher.Content = "Don't Notify";
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
    
        if (checkPatient.Fio != FioField.Text || checkPatient.Address != AddressField.Text)
        {
            try
            {
                checkPatient.Fio = FioField.Text;
                checkPatient.Address = AddressField.Text;
    
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
    
    private void NotificationSwitcher_Click(object sender, RoutedEventArgs e)
    {
        object doctorObject;

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out doctorObject);

        Doctor currentDoctor = doctorObject as Doctor;

        DoctorVisitsFrequency frequency = dbContext.DoctorVisitsFrequency
            .FirstOrDefault(fr => fr.Patient.Id == medicalCardHistory.Patient.Id &&
                                  fr.MedicalSpecialization.Id == currentDoctor.MedicalSpecialization.Id);

        if (frequency == null)
        {

            InputEmailWindow inputEmailWindow = new InputEmailWindow(dbContext, medicalCardHistory);
            inputEmailWindow.ShowDialog();
            
            object emailObject;
            if (!VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.EMAIL_SETUP, out emailObject))
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.MessageField.Text = "You have to input email!";
                messageWindow.ShowDialog();
                return;
            };
            string email = emailObject as string;

            (sender as Button).Content = "Don't Notify";

            DoctorVisitsFrequency doctorVisitsFrequency = new DoctorVisitsFrequency();
            doctorVisitsFrequency.MedicalSpecialization = currentDoctor.MedicalSpecialization;
            doctorVisitsFrequency.Patient = medicalCardHistory.Patient;
            doctorVisitsFrequency.Patient.email = email;
            doctorVisitsFrequency.Frequency = 360; // через сколько дней уведомлять

            dbContext.DoctorVisitsFrequency.Add(doctorVisitsFrequency);
            dbContext.SaveChanges();
        }
        else
        {
            (sender as Button).Content = "Notify";
            dbContext.DoctorVisitsFrequency.Remove(frequency);
            dbContext.SaveChanges();
        }
    }
    
    private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var selectedList = dbContext.Diagnosis.Where(checkSearchCriterias()).ToList();

        diagnosisDataGrid.ItemsSource = selectedList;
    }

    private Expression<Func<Diagnosis, bool>> checkSearchCriterias()
    {
        return m => m.Doctor.MedicalSpecialization.Name.Contains(Searcher.Text);
    }
}