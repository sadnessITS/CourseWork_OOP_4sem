using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
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
        AccountantCourseworkContext db;
        public AdministrateView()
        {
            InitializeComponent();
            UpdateDb();
        }

        public void UpdateDb()
        {
            db = new AccountantCourseworkContext();
            db.Doctor.Load();
            UserDatabase.ItemsSource = db.Doctor.Local.ToBindingList();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddingUserWindow addingForm = new AddingUserWindow();
            addingForm.ShowDialog();
            UpdateDb();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            db = new AccountantCourseworkContext();

            Doctor itemDel = UserDatabase.SelectedItem as Doctor;
            
            if (itemDel != null)
            {
                Doctor userDel = db.Doctor
                    .Where(o => o.Id == itemDel.Id)
                    .FirstOrDefault();

                if (userDel.Role != Role.ADMIN)
                {
                    db.Doctor.Remove(userDel);
                    db.SaveChanges();
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
        
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Searcher_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            db = new AccountantCourseworkContext();

            var selectedList = db.Doctor.Where(p => p.Login.Contains(Searcher.Text)).ToList();

            UserDatabase.ItemsSource = selectedList;
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
    }
}