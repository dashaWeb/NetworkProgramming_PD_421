using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Program
{
    static string address = "127.0.0.1";
    static int port = 8080;

    private static void Main(string[] args)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        UdpClient listener = new UdpClient(ipPoint);
        try
        {
          
            //listenSocket.Bind(ipPoint);

            Console.WriteLine("Server strarted! Waiting for connection ... ");
            while (true)
            {
                int bytes = 0;
                //byte[] data = new byte[1024];
                //bytes = listenSocket.ReceiveFrom(data, ref remoteEndPoint);
                byte[] data = listener.Receive(ref remoteEndPoint);
                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine($"{DateTime.Now.ToShortDateString()} :: {message} from {remoteEndPoint}");


                // send answer
                string msg = "Message was send!";
                data = Encoding.Unicode.GetBytes(msg);
                //listenSocket.SendTo(data, remoteEndPoint);
                listener.Send(data, data.Length, remoteEndPoint);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        /*listenSocket.Shutdown(SocketShutdown.Both);
        listenSocket.Close();*/
        listener.Close();
    }
}