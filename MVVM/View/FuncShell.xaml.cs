using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class FuncShell : Window
    {
        private AccountantCourseworkContext db;
        
        public FuncShell()
        {
            InitializeComponent();
        }

        public void DetermineCurrentUser()
        {
            object currentDoctorObject;
            if (!VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.CURRENT_DOCTOR, out currentDoctorObject))
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "We don't have info about doctor :(";
                mesWin.ShowDialog();
                return;
            }

            Doctor currentDoctor = currentDoctorObject as Doctor;

            db = new AccountantCourseworkContext();
            
            Doctor checkDoctor = db.Doctor
                .Where(doctor => doctor == currentDoctor)
                .FirstOrDefault();

            UserStatus.Text = checkDoctor.Fio;
        }

        private void DrugToolbar(object sender, MouseButtonEventArgs e)
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
        
        private void Resize_MouseUp (object sender, MouseButtonEventArgs e)
        {
            Rect workArea = System.Windows.SystemParameters.WorkArea;
            if (this.Width == SystemParameters.WorkArea.Width)
            {
                this.Width = 1220;
                this.Height = 700;
                this.Left = (workArea.Width - this.Width) / 2 + workArea.Left;
                this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
            }
            else
            {
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;
                this.Left = (workArea.Width - this.Width) / 2 + workArea.Left;
                this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
            }
        }

        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            AuthWindow auth = new AuthWindow();
            auth.Show();
            this.Close();
            
        }
    }
}