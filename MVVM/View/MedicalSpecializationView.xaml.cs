using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class MedicalSpecializationView : Window
{
    private AccountantCourseworkContext dbContext;
    
    public MedicalSpecializationView(AccountantCourseworkContext dbContext)
    {
        InitializeComponent();

        this.dbContext = dbContext;
        
        UpdateMedicalSpecialization();
    }

    private void DrugWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void UpdateMedicalSpecialization()
    {
        dbContext.MedicalSpecialization.Load();
        medicalSpecializationDataGrid.ItemsSource = dbContext.MedicalSpecialization.Local.ToBindingList();
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
        new AddingMedicalSpecializationWindow(dbContext).ShowDialog();
        UpdateMedicalSpecialization();
    }

    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedMedicalSpecialization = medicalSpecializationDataGrid.SelectedItem as MedicalSpecialization;

        Doctor doctorExist = dbContext.Doctor
            .FirstOrDefault(item => item.IdMedicalSpecialization == selectedMedicalSpecialization.Id);

        if (doctorExist == null)
        {
            dbContext.MedicalSpecialization.Remove(selectedMedicalSpecialization);
            dbContext.SaveChanges();
        }
        else
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "There is a doctor with\nselected specialization!";
            mesWin.ShowDialog();
            return;
        }
    }
}