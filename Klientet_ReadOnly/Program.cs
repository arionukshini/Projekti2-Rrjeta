using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Klienti Read-Only eshte gati");
string server_ip = "127.0.0.1"; 
int server_port = 4444;
UdpClient client = new UdpClient();
IPEndPoint server_endpoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);
try 
{
    while (true) 
    {
        Console.Write("\nTest: ");
        string? message = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(message)) continue;
        if (message.ToLower() == "exit") break;

        
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length, server_endpoint);

        
        IPEndPoint sender_ep = new IPEndPoint(IPAddress.Any, 0);
        byte[] responseData = client.Receive(ref sender_ep);
        string response = Encoding.UTF8.GetString(responseData);

        Console.WriteLine("Përgjigja nga Serveri: " + response);
    }
}
catch (Exception ex) 
{
    Console.WriteLine("Gabim: " + ex.Message);
}
finally 
{
    client.Close();
    Console.WriteLine("Lidhja u mbyll.");
}
