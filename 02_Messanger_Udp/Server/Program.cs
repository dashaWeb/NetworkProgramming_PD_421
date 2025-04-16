using System.Net;
using System.Net.Sockets;
using System.Text;

public class ChatServer
{
    const int port = 4040;
    const string JOIN_CMD = "$<Join>";
    static List<IPEndPoint> members;
    UdpClient server;
    IPEndPoint client = null;

    public ChatServer()
    {
        server= new UdpClient(port);
        members= new List<IPEndPoint>();
    }
    public void Start()
    {
        while (true)
        {
            byte[] data = server.Receive(ref client);
            string message = Encoding.Unicode.GetString(data);
            switch (message)
            {
                case JOIN_CMD:
                    AddMember(client);
                    break;
                default:
                    SendAll(message);
                    break;
            }
        }
    }
    private void AddMember(IPEndPoint client)
    {
        if (!members.Contains(client))
            members.Add(client);
        Console.WriteLine($"Members was added. Number {members.Count}");
    }
    private void SendAll(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        foreach (var item in members)
        {
            server.SendAsync(data, data.Length, item);
        }
    }
}
internal class Program
{
    private static void Main(string[] args)
    {
        ChatServer server = new ChatServer();
        server.Start();
    }
    
}