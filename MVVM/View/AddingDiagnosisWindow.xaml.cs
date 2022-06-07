using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;

namespace HospitalPatientRecords.MVVM.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddingDiagnosisWindow : Window
    {
        AccountantCourseworkContext db;
        
        public int IdPatient { get; set; }

        public AddingDiagnosisWindow()
        {
            InitializeComponent();
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
        
        bool Validate(Diagnosis diagnosis)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(diagnosis);
            if (!Validator.TryValidateObject(diagnosis, context, results, true))
            {
                MessageWindow mesWin = new MessageWindow();
                
                string errors = "";
                foreach (var error in results)
                {
                    errors += error.ErrorMessage; errors += "\n";
                }

                errors = errors.Substring(0, errors.Length - 2);
                mesWin.Height += 13 * results.Count;
                mesWin.MessageField.Text = errors;
                mesWin.ShowDialog();
                return false;
            }
            else return true;
        }
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            db = new AccountantCourseworkContext();
            List<Diagnosis> listDiagnosis = db.Diagnosis.ToList();

            string activeUser;
            if (!VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.key.IdActiveUser, out activeUser))
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "We don't have info about doctor :(";
                mesWin.ShowDialog();
                return;
            }
            
            User checkUser = db.User
                .Where(o => o.IdUser == Convert.ToInt32(activeUser))
                .FirstOrDefault();

            try
            {
                Diagnosis diagnosis = new Diagnosis();
                diagnosis.IdVisiting = (listDiagnosis.Count + 1);
                diagnosis.IdPatient = IdPatient;
                diagnosis.IdUser = Convert.ToInt32(activeUser);
                diagnosis.Medicine = checkUser.Medicine;
                diagnosis.Diagnosis1 = DiagnosisTextBox.Text;
                diagnosis.Date = DateTime.Now;
                //diagnosis.
                
                if (Validate(diagnosis) == false) return;

                db.Diagnosis.Add(diagnosis);
                db.SaveChanges();
            
                MessageWindow mesWin = new MessageWindow();
                mesWin.TitleField.Text = "Congratulations!";
                mesWin.MessageField.Text = "Diagnosis was added!";
                mesWin.ShowDialog();
                this.Close();
            }
            catch
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Something was wrong!";
                mesWin.ShowDialog();
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
