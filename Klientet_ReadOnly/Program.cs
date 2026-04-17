using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Klienti Read-Only eshte gati");
string server_ip = "127.0.0.1"; 
int server_port = 4444;
UdpClient client = new UdpClient();
IPEndPoint server_endpoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);
