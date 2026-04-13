using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Serveri
{
    static void Main() { 
        string server_ip = "127.0.0.1";
        int server_port = 4444;

        UdpClient udp_server = new UdpClient(server_port);
        Console.WriteLine("Serveri filloi ne " + server_ip + ":" + server_port);

        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);

        string server_files = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\ServerFiles");

        if (!Directory.Exists(server_files))
            Directory.CreateDirectory(server_files);

        while (true) {
            byte[] data = udp_server.Receive(ref endpoint);
            string message = Encoding.UTF8.GetString(data);

            Console.WriteLine(endpoint + " " + message);

            string response;

            if (message.StartsWith("admin"))
            {
                response = "FULL ACCESS (read/write/execute) ";
            }
            else if (message == "list")
            {
                var files = Directory.GetFiles(server_files);
                response = string.Join(", ", files.Select(Path.GetFileName));
            }
            else
            {
                response = "READ ONLY ACCESS";
            }

            byte[] reply = Encoding.UTF8.GetBytes(response);
            udp_server.Send(reply, reply.Length, endpoint);
        }
    }
}