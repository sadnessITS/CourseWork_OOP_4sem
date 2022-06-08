using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class AddingMedicalSpecializationWindow : Window
{
    private AccountantCourseworkContext dbContext;
    
    public AddingMedicalSpecializationWindow(AccountantCourseworkContext dbContext)
    {
        InitializeComponent();

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

    private void Add_OnClick(object sender, RoutedEventArgs e)
    {
        dbContext.MedicalSpecialization.Add(new MedicalSpecialization(nameField.Text));
        dbContext.SaveChanges();
        
        MessageWindow mesWin = new MessageWindow();
        mesWin.TitleField.Text = "Congratulations!";
        mesWin.MessageField.Text = "Added!";
        mesWin.ShowDialog();
    }
}