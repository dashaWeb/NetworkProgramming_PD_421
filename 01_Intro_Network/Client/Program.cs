using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static string address = "127.0.0.1";
    static int port = 8080;

    private static void Main(string[] args)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            IPEndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);
            //EndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            UdpClient udpClient = new UdpClient();
            string message = "";
            while (message != "end")
            {
                Console.Write("Enter a message :: ");
                message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data,data.Length,ipPoint);
                //socket.SendTo(data, ipPoint);
                /*socket.Connect(ipPoint);
                socket.Send(data);*/

                //int bytes = 0;
                string response = "";
                data = new byte[1024]; // 1Kb
                /*do
                {
                    bytes = socket.ReceiveFrom(data, ref remoteIpPoint);
                    response += Encoding.Unicode.GetString(data);
                } while (socket.Available > 0);*/
                data = udpClient.Receive(ref remoteIpPoint);
                response = Encoding.Unicode.GetString(data);
                Console.WriteLine($"Server response :: {response}");
            }

            /*socket.Shutdown(SocketShutdown.Both);
            socket.Close();*/
            udpClient.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}