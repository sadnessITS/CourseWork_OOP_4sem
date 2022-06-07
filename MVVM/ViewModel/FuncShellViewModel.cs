using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.Core;
using HospitalPatientRecords.MVVM.View;

namespace HospitalPatientRecords.MVVM.ViewModel
{
    public class FuncShellViewModel : ObservableObject
    {
        public RelayCommand DatabaseCommand { get; set; }
        public RelayCommand AdministrateCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }

        public DatabaseViewModel DatabaseVM { get; set; }
        public AdministrateViewModel AdministrateVM { get; set; }
        public AboutViewModel AboutVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value; 
                OnPropertyChanged();
            }
        }
        
        public int CurrentDoctor { get; set; }
        public string DoctorFio { get; set; }
        
        public FuncShellViewModel()
        {
            DatabaseVM = new DatabaseViewModel();
            AdministrateVM = new AdministrateViewModel();
            AboutVM = new AboutViewModel();

            CurrentView = DatabaseVM;

            DatabaseCommand = new RelayCommand(o =>
            {
                CurrentView = DatabaseVM;
            });
            
            AboutCommand = new RelayCommand(o =>
            {
                CurrentView = AboutVM;
            });

            AdministrateCommand = new RelayCommand(o =>
            {
                CurrentView = AdministrateVM;
            });
        }
    }
}