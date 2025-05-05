using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*        const string serverAddress = "127.0.0.1";
                const int port = 4040;*/
        IPEndPoint serverPoint;
        TcpClient client;
        NetworkStream ns = null;
        StreamWriter sw = null;
        StreamReader sr = null;
        ObservableCollection<MessageInfo> messages;
        public MainWindow()
        {
            InitializeComponent();
            messages = new ObservableCollection<MessageInfo>();
            
            string serverAddress = ConfigurationManager.AppSettings["serverAddress"]!;
            int port = int.Parse(ConfigurationManager.AppSettings["serverPort"]!);
            serverPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            this.DataContext = messages;
        }

        private void SendBtn(object sender, RoutedEventArgs e)
        {
            string message = msgTextBox.Text;
            //msgTextBox.Text = "";
            if (sw == null)
                return;
            sw.WriteLine(message);
            sw.Flush(); // примусове виштовхування з буфера
        }

        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendBtn(sender, e);
            }
        }


        private void ConnectBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(serverPoint);
                ns = client.GetStream();

                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Listener();

        }
        private async void Listener()
        {
            while (true)
            {
                try
                {
                    string? message = await sr.ReadLineAsync();
                    if(message != null)
                        messages.Add(new MessageInfo(message));
                }
                catch (Exception ex)
                {

                  break;
                }

            }
        }

        private void DisconnectBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                sw.WriteLine("close");
                sw.Flush();
                
                ns.Close();
                client.Close();

                sw = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
    public class MessageInfo
    {
        public string Message { get; set; }
        private DateTime time;
        public string Time => time.ToLongTimeString();
        public MessageInfo(string text)
        {
            this.Message = text;
            time = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Message,-20} {Time}";
        }
    }
}
