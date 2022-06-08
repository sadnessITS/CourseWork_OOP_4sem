using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class AddingEntryWindow : Window
{
    private AccountantCourseworkContext dbContext;
    
    public AddingEntryWindow(AccountantCourseworkContext dbContext)
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
}