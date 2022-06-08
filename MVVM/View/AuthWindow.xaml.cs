using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class AuthWindow : Window
    {
        private AccountantCourseworkContext dbContext;
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Logo_OnMouseDown(object sender, MouseButtonEventArgs e)
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

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordField.Password.Length > 0) Placeholder.Visibility = Visibility.Hidden;
            else Placeholder.Visibility = Visibility.Visible;
        }
        
        public string CalcHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(hash);
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            dbContext = new AccountantCourseworkContext();

            dbContext.Database.Initialize(true);

            if (dbContext.Doctor.Count() == 0)
                addDefaultAdmin(dbContext);

            Doctor user = dbContext.Doctor.FirstOrDefault(u => u.Login == Username.Text);

            if (Username.Text == "" || PasswordField.Password == "")
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Field can not be empty!";
                mesWin.ShowDialog();
                return;
            }
            
            if (user == null)
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Incorrect login!";
                mesWin.ShowDialog();
                return;
            }
            
            string hash = CalcHash(PasswordField.Password);

            if (user.Password != hash)
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Incorrect password!";
                mesWin.ShowDialog();
                return;
            }

            if (user.Role == Role.USER)
            {
                FuncShell funcShell = new FuncShell(dbContext);
                funcShell.UserDb.Visibility = Visibility.Collapsed;
                funcShell.Show();
                this.Close();
            }

            if (user.Role == Role.ADMIN)
            {
                FuncShell funcShell = new FuncShell(dbContext);
                FuncShellViewModel funcShellViewModel = new FuncShellViewModel();

                VarsDictionary.varsDictionary.Add(VarsDictionary.Key.CURRENT_DOCTOR, user);
                
                funcShell.DetermineCurrentUser();
                funcShell.Show();
                this.Close();
            }
        }

        private void addDefaultAdmin(AccountantCourseworkContext dbContext)
        {
            MedicalSpecialization adminSpecialization =
                dbContext.MedicalSpecialization.Add(new MedicalSpecialization("ADMIN"));

            dbContext.Doctor.Add(new Doctor()
            {
                Login = "admin",
                Password = CalcHash("admin"),
                Fio = "admin",
                Role = Role.ADMIN,
                MedicalSpecialization = adminSpecialization
            });

            dbContext.SaveChanges();
        }
    }
}