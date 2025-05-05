using System.Net;
using System.Net.Sockets;

internal class Program
{
    private static void Main(string[] args)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        TcpListener listener = new TcpListener(ipPoint);
        Semaphore semaphore = new Semaphore(2,2);
        try
        {
            listener.Start(10);
            Console.WriteLine("Server started! Waiting for connection");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => ServerClient(client, semaphore));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            listener.Stop();
        }
    }

    private static void ServerClient(TcpClient client, Semaphore semaphore)
    {
        if(!semaphore.WaitOne(200)) {
            using (StreamWriter sw = new StreamWriter(client.GetStream()))
            {
                sw.WriteLine("Server Error!");
            }
            client.Close();
            return;
        }
        try
        {
            NetworkStream stream = client.GetStream();
            StreamReader sr = new StreamReader(stream);

            string message = sr.ReadLine();

            Console.WriteLine($"{client.Client.RemoteEndPoint} - {message} at {DateTime.Now.ToLongTimeString()}");

            StreamWriter sw = new StreamWriter(stream);

            if (!long.TryParse(message, out long number))
            {
                sw.WriteLine("Incorrect format");
            }
            else
            {
                sw.WriteLine(GetFactorial(number).ToString());
            }

            sw.Close();
            sr.Close();
            stream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Close();
            semaphore.Release();
        }
    }

    private static ulong GetFactorial(long number)
    {
        ulong reasult = 1;
        for (int i = 2; i <= number; i++)
        {
            reasult *= (ulong)i;
            Thread.Sleep(500);
        }
        return reasult;
    }
}