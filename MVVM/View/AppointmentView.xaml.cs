using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View;

public partial class ScheduleView : UserControl
{
    AccountantCourseworkContext dbContext;
    public ScheduleView()
    {
        InitializeComponent();

        object db;

        VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.DB_CONTEXT, out db);

        dbContext = db as AccountantCourseworkContext;

        UpdateDb();
    }

    private void UpdateDb()
    {
        List<Appointment> selectedAppointment = dbContext.Appointment.ToList();

        foreach (var s in selectedAppointment)
        {
            s.Patient = dbContext.Patient.Where(item => item.Id == s.IdPatient).FirstOrDefault();
        }

        scheduleDataGrid.ItemsSource = selectedAppointment;
    }

    private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var selectedList = dbContext.Doctor.Where(checkSearchCriterias()).ToList();

        scheduleDataGrid.ItemsSource = selectedList;
    }

    private Expression<Func<Doctor, bool>> checkSearchCriterias()
    {
        return m => m.Login.Contains(Searcher.Text) || m.Fio.Contains(Searcher.Text);
    }

    private void Add_OnClick(object sender, RoutedEventArgs e)
    {
        new AddingEntryWindow(dbContext).ShowDialog();
        UpdateDb();
    }

    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
        Appointment selectedAppointment = scheduleDataGrid.SelectedItem as Appointment;

        if (selectedAppointment == null)
        {
            MessageWindow mesWin = new MessageWindow();
            mesWin.MessageField.Text = "Select appointment!";
            mesWin.ShowDialog();
            return;
        }

        dbContext.Appointment.Remove(selectedAppointment);
        dbContext.SaveChanges();

        UpdateDb();
    }
}