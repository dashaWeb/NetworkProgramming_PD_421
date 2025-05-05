using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        UdpClient client;
        ObservableCollection<MessageInfo> messages;
        public MainWindow()
        {
            InitializeComponent();
            messages = new ObservableCollection<MessageInfo>();
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["serverAddress"]!;
            int port = int.Parse(ConfigurationManager.AppSettings["serverPort"]!);
            serverPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            this.DataContext = messages;
        }

        private void SendBtn(object sender, RoutedEventArgs e)
        {
            string message = msgTextBox.Text;
            msgTextBox.Text = "";
            SendMessage(message);
            

        }

        private void msgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendBtn(sender, e);
            }
        }
        private async void SendMessage(string message)
        {
            
            byte[] data = Encoding.Unicode.GetBytes(message);
            await client.SendAsync(data, data.Length, serverPoint);
        }

        private void JoinBtn(object sender, RoutedEventArgs e)
        {
            SendMessage("$<Join>");
            Listener();
        }
        private async void Listener()
        {
            while (true)
            {
                var data = await client.ReceiveAsync();
                string message = Encoding.Unicode.GetString(data.Buffer);
                messages.Add(new MessageInfo(message));
            }
        }

        private void LeaveBtn(object sender, RoutedEventArgs e)
        {
            SendMessage("$<Leave>");
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
