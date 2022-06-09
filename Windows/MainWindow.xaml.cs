using HospitalPatientRecords.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows;

using System.Collections.Generic;
using HospitalPatientRecords.MVVM.Model;

namespace HospitalPatientRecords.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AccountantCourseworkContext dbContext = new AccountantCourseworkContext();

        private BackgroundWorker worker = new BackgroundWorker();

        private const string smtpServerAddress = "smtp.mail.ru";
        private const int serverSslPort = 587;

        private const string userName = "ppireiko@mail.ru";
        private const string password = "k11ieGMOarshXfOTLJ7M";

        private static TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

        private volatile bool isRunning = true;

        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += Emailing;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Worked!");
        }

        private void Emailing(object sender, DoWorkEventArgs e)
        {
            SmtpClient mailClient = new SmtpClient();
            mailClient.EnableSsl = true;
            mailClient.Host = smtpServerAddress;
            mailClient.Port = serverSslPort;
            mailClient.Credentials = new System.Net.NetworkCredential(userName, password);

            while (isRunning)
            {
                List<Patient> patientsToCheck = dbContext.DoctorVisitsFrequency.Select(fr => fr.Patient).ToList();

                foreach (Patient patient in patientsToCheck)
                {
                    Diagnosis diagnosis = dbContext.Diagnosis
                        .OrderByDescending(d => d.Date)
                        .FirstOrDefault(d => d.Patient.Id == patient.Id);

                    DoctorVisitsFrequency doctorVisitsFrequency = dbContext.DoctorVisitsFrequency
                        .FirstOrDefault(d => d.Patient.Id == patient.Id);

                    MedicalSpecialization medicalSpecialization = dbContext.MedicalSpecialization
                        .FirstOrDefault(ms => ms.Id == doctorVisitsFrequency.IdMedicalSpecialization);

                    int frequency = doctorVisitsFrequency.Frequency;

                    bool toSend = DateTime.Now > diagnosis.Date.AddDays(frequency);

                    if (toSend)
                    {
                        MailMessage message = new MailMessage(userName, patient.email);
                        message.Subject = "HospitalNotifier";
                        message.Body = "You need visit a doctor: " + medicalSpecialization.Name;
                        message.IsBodyHtml = false;

                        mailClient.Send(message);
                    }
                }

                Thread.Sleep(oneDay);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            isRunning = false;
            worker.CancelAsync();
        }
    }
}
