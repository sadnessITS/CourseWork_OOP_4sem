using System;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class InputEmailWindow : Window
{
    private AccountantCourseworkContext dbContext;
    private MedicalCardHistory _medicalCardHistory;
    public InputEmailWindow(AccountantCourseworkContext dbContext, MedicalCardHistory medicalCardHistory)
    {
        InitializeComponent();

        this.dbContext = dbContext;
        _medicalCardHistory = medicalCardHistory;
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
        if (!IsValidEmail(emailField.Text))
        {
            MessageWindow messageWindow = new MessageWindow();
            messageWindow.MessageField.Text = "Invalid Email!";
            messageWindow.ShowDialog();
            return;
        }
        
        _medicalCardHistory.Patient.email = emailField.Text;
        
        VarsDictionary.varsDictionary.Remove(VarsDictionary.Key.EMAIL_SETUP);
        VarsDictionary.varsDictionary.Add(VarsDictionary.Key.EMAIL_SETUP, _medicalCardHistory.Patient.email);
        
        MessageWindow messageWindow2 = new MessageWindow();
        messageWindow2.TitleField.Text = "Congratulations!";
        messageWindow2.MessageField.Text = "Done!";
        messageWindow2.ShowDialog();
        this.Close();
    }
    
    bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) {
            return false; // suggested by @TK-421
        }
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }
}