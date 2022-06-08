using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class AddingEntryWindow : Window
{
    private AccountantCourseworkContext dbContext;

    Appointment newAppointment;
    
    public AddingEntryWindow(AccountantCourseworkContext dbContext)
    {
        InitializeComponent();

        newAppointment = new Appointment();

        object doctorObject;

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out doctorObject);

        newAppointment.Doctor = doctorObject as Doctor;

        this.dbContext = dbContext;
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

    private void searchPatient_Click(object sender, RoutedEventArgs e)
    {
        new SearchPatientWindow(dbContext).ShowDialog();

        object selectedPatientObject;

        bool isPatientSelected = VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.SELECTED_IN_SEARCH_PATIENT,
            out selectedPatientObject);

        if (isPatientSelected)
        {
            newAppointment.Patient = selectedPatientObject as Patient;
            searchPatient.Content = newAppointment.Patient.Fio;
        }
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!meetingDate.SelectedDate.HasValue)
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "Choose the time!";
            mesWin.ShowDialog();
            return;
        }

        newAppointment.AppointmentTime = meetingDate.SelectedDate.Value;

        if (newAppointment.AppointmentTime < DateTime.Now)
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "Incorrect date!";
            mesWin.ShowDialog();
            return;
        }

        object patientObject;

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.SELECTED_IN_SEARCH_PATIENT, out patientObject);

        newAppointment.Patient = patientObject as Patient;

        Appointment existingAppointment = dbContext.Appointment
            .FirstOrDefault(ap => ap.AppointmentTime.Equals(newAppointment.AppointmentTime));

        if (existingAppointment != null)
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "This time is already choosed by another person!";
            mesWin.ShowDialog();
            return;
        }

        newAppointment = dbContext.Appointment.Add(newAppointment);

        object doctorObject;

        var medicalCard = dbContext.MedicalCardHistory.FirstOrDefault(item => item.IdPatient == newAppointment.IdPatient);

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out doctorObject);

        Doctor currentDoctor = doctorObject as Doctor;

        MedicalCardHistory medicalCardHistory = new MedicalCardHistory()
        {
            CardNumber = medicalCard.CardNumber,
            Address = "[Replaced:] " + currentDoctor.Fio,
            Date = DateTime.Now,
            ActionWithCard = "Replaced to doctor",
            Patient = newAppointment.Patient
        };
        
        dbContext.MedicalCardHistory.Add(medicalCardHistory);
        dbContext.SaveChanges();
        Close();
    }
}