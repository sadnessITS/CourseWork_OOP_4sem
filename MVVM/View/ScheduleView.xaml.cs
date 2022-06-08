using System;
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
            
        //UpdateDb();
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
        //UpdateMedicalSpecialization();
    }

    private void Delete_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}