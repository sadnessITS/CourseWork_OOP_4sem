using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using HospitalPatientRecords.MVVM.Model;
using HospitalPatientRecords.MVVM.ViewModel;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace HospitalPatientRecords.MVVM.View
{
    public partial class AdministrateView : UserControl
    {
        AccountantCourseworkContext dbContext;
        public AdministrateView()
        {
            InitializeComponent();

            object db;

            VarsDictionary.varsDictionary.TryGetValue(VarsDictionary.Key.DB_CONTEXT, out db);

            dbContext = db as AccountantCourseworkContext;
            
            UpdateDb();
        }

        public void UpdateDb()
        {
            var listDoctors = dbContext.Doctor.ToList();
            
            foreach (var d in listDoctors)
            {
                d.MedicalSpecialization = dbContext.MedicalSpecialization.Where(item => item.Id == d.IdMedicalSpecialization).FirstOrDefault();
            }
            
            UserDatabase.ItemsSource = listDoctors;
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddingUserWindow addingForm = new AddingUserWindow(dbContext);
            addingForm.ShowDialog();
            UpdateDb();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            Doctor itemDel = UserDatabase.SelectedItem as Doctor;
            
            if (itemDel != null)
            {
                Doctor userDel = dbContext.Doctor
                    .Where(o => o.Id == itemDel.Id)
                    .FirstOrDefault();

                if (userDel.Role != Role.ADMIN)
                {
                    dbContext.Doctor.Remove(userDel);
                    dbContext.SaveChanges();
                }
                else
                {
                    MessageWindow mesWin = new MessageWindow();
                    mesWin.MessageField.Text = "You can't delete yourself!";
                    mesWin.ShowDialog();
                    return;
                }

                UpdateDb();
            }
            else
            {
                MessageWindow mesWin = new MessageWindow();
                mesWin.MessageField.Text = "Item not a data!";
                mesWin.ShowDialog();
            }
        }
        
        private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedList = dbContext.Doctor.Where(checkSearchCriterias()).ToList();

            UserDatabase.ItemsSource = selectedList;
        }

        private Expression<Func<Doctor, bool>> checkSearchCriterias()
        {
            return m => m.Login.Contains(Searcher.Text) || m.Fio.Contains(Searcher.Text);
        }
        
        bool Validate(Doctor user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, context, results, true))
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

        private void Specialization_OnClick(object sender, RoutedEventArgs e)
        {
            MedicalSpecializationView medicalSpecializationView = new MedicalSpecializationView(dbContext);
            medicalSpecializationView.ShowDialog();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            dbContext.SaveChanges();
            
            MessageWindow mesWin = new MessageWindow();
            mesWin.TitleField.Text = "Congratulations!";
            mesWin.MessageField.Text = "Name was changed!";
            mesWin.ShowDialog();
        }
    }
}