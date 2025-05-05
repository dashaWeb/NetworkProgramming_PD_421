using System.Net;
using System.Net.Sockets;
using System.Text;

public class ChatServer
{
    const int port = 4040;
    TcpListener server;
    IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

    public ChatServer()
    {
        server = new TcpListener(address);

    }
    public void Start()
    {
        server.Start();
        Console.WriteLine("Waiting for connection");
        //server.AcceptSocket(); // отримати сокет того хто приєднався
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!!!");
        NetworkStream ns = client.GetStream();
        StreamReader sr = new StreamReader(ns);
        StreamWriter sw = new StreamWriter(ns);
        while (true)
        {
            string msg = sr.ReadLine()!;

            if (msg == "close")
            {
                ns.Close();
                server?.Stop();
                break;
            }

            Console.WriteLine($"Got message {msg} from {client.Client.LocalEndPoint}  ({DateTime.Now.ToLongTimeString()})");
            sw.WriteLine(msg);
            sw.Flush();
        }
    }


}
internal class Program
{
    private static void Main(string[] args)
    {
        ChatServer server_ = new ChatServer();
        while (true)
        {
            server_.Start();

        }

    }

}