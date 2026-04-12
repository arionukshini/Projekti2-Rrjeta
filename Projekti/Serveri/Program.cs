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

        while (true) {
            byte[] data = udp_server.Receive(ref endpoint);
            string message = Encoding.UTF8.GetString(data);

            Console.WriteLine(endpoint + " " + message);
        }
    }
}