using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SendMessage_Smtp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string server = "smtp.gmail.com";
        int port = 587;
        const string username = "dashakonopelko@gmail.com";
        const string password = "azzt gjac hkrn xwir";
        const string otheruser = @"velax12199@idoidraw.com";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Client_SendMessage(object sender, RoutedEventArgs e)
        {
            MailMessage message = new MailMessage(username,otheruser,"Theme","Lorem ipsum test");

            //sent html body
            using (StreamReader sr = new StreamReader("index.html"))
            {
                message.Body += sr.ReadToEnd();
            }
            message.IsBodyHtml = true;

            message.Priority = MailPriority.High;
            message.Attachments.Add(new Attachment(@"img.avif"));
            message.Attachments.Add(new Attachment(@"text.txt"));

            // create a send object
            SmtpClient smtpClient = new SmtpClient(server,port);
            smtpClient.EnableSsl= true;

            smtpClient.Credentials = new NetworkCredential(username, password);

            smtpClient.SendCompleted += Client_sendCompleted;

            smtpClient.SendAsync(message,message);
        }

        private void Client_sendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var state = (MailMessage)e.UserState!;
            MessageBox.Show($"Message was send! Subject : {state?.Subject}");
        }
    }
}
