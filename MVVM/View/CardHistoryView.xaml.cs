using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class CardHistoryView : Window
{
    private AccountantCourseworkContext dbContext;

    private Patient currentPatient;
    public CardHistoryView(AccountantCourseworkContext dbContext, Patient patient)
    {
        InitializeComponent();

        currentPatient = patient;
        this.dbContext = dbContext;
        
        UpdateCardHistory();
    }
        
    private void DrugWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void UpdateCardHistory()
    {
        List<MedicalCardHistory> selectedCardHistories = dbContext.MedicalCardHistory.Where(item => item.IdPatient == currentPatient.Id).ToList();
            
        cardHistoryDataGrid.ItemsSource = selectedCardHistories;
    }
        
    private void Minimize_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void Close_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.Close();
    }

    private void Cancel_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}