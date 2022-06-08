using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class SearchPatientWindow : Window
    {
        private AccountantCourseworkContext dbContext;

        public SearchPatientWindow(AccountantCourseworkContext dbContext)
        {
            InitializeComponent();
            this.dbContext = dbContext;
            patientDataGrid.ItemsSource = dbContext.Patient.ToList();
        }

        private void Searcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedList = dbContext.Patient.Where(checkSearchCriterias()).ToList();

            patientDataGrid.ItemsSource = selectedList;
        }
        private Expression<Func<Patient, bool>> checkSearchCriterias()
        {
            return p => p.Fio.Contains(Searcher.Text);
        }

        private void patientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Patient selectedPatient = patientDataGrid.SelectedItem as Patient;

            VarsDictionary.varsDictionary.Add(VarsDictionary.Key.SELECTED_IN_SEARCH_PATIENT, selectedPatient);

            Close();
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
}
