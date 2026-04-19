using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

string server_ip = "127.0.0.1";
int server_port = 4444;

Console.Title = "UDP CLIENT - READ ONLY";
Console.Clear();
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("=======================================");
Console.WriteLine("     SISTEMI I KONTROLLIT - READ ONLY  ");
Console.WriteLine("=======================================");
Console.ResetColor();

UdpClient client = new UdpClient();
IPEndPoint server_endpoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);

try
{
    while (true)
    {
        Console.WriteLine("\n--- MENYJA ---");
        Console.WriteLine("[1] LIST   - Shfaq file");
        Console.WriteLine("[2] READ   - Lexo file");
        Console.WriteLine("[3] PING    - Testo lidhjen");
        Console.WriteLine("[exit] EXIT - Mbyll klientin");

        Console.Write("\nZgjidhni një opsion: ");
        string? zgjedhja = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(zgjedhja)) continue;
        if (zgjedhja.ToLower() == "exit") break;

        string kërkesa = "";

        switch (zgjedhja)
        {
            case "1":
                kërkesa = "readonly:list";
                break;

            case "2":
                Console.Write("Emri i file: ");
                string fileRead = Console.ReadLine();
                kërkesa = $"readonly:read:{fileRead}";
                break;

            case "3":
                kërkesa = "readonly:ping";
                break;

            default:
                Console.WriteLine("Opsion i pavlefshëm!");
                continue;
        }

        byte[] data = Encoding.UTF8.GetBytes(kërkesa);
        client.Send(data, data.Length, server_endpoint);
        Console.WriteLine($"[INFO] U dërgua: {kërkesa}");

        IPEndPoint sender_ep = new IPEndPoint(IPAddress.Any, 0);

        client.Client.ReceiveTimeout = 5000;

        byte[] responseData = client.Receive(ref sender_ep);
        string response = Encoding.UTF8.GetString(responseData);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n>>> PËRGJIGJA NGA SERVERI:");
        Console.WriteLine(response);
        Console.ResetColor();
        Console.WriteLine("--------------------------------------------------");
    }
}
catch (SocketException)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n[GABIM] Serveri nuk po përgjigjet! Sigurohu që serveri është ndezur.");
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.WriteLine("\n[GABIM] " + ex.Message);
}
finally
{
    client.Close();
    Console.WriteLine("\nLidhja u mbyll.");
}