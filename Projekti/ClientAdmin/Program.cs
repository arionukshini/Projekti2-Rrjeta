using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

 
string server_ip = "127.0.0.1"; 
int server_port = 4444; 

Console.Title = "UDP CLIENT - ADMIN (FULL ACCESS)";
Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("==================================================");
Console.WriteLine("    SISTEMI I KONTROLLIT - ADMIN (PERSONI 2)    ");
Console.WriteLine("==================================================");
Console.ResetColor();

UdpClient client = new UdpClient();
IPEndPoint server_endpoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);

try 
{
    while (true) 
    {
        Console.WriteLine("\n--- MENYJA E KOMANDAVE ---");
        Console.WriteLine("[1] READ    - Listo skedarët në server");
        Console.WriteLine("[2] WRITE   - Shkruaj një mesazh/skedar të ri");
        Console.WriteLine("[3] EXECUTE - Ekzekuto një proces në server");
        Console.WriteLine("[4] PING    - Testo lidhjen");
        Console.WriteLine("[exit]      - Mbyll klientin");
        
        Console.Write("\nZgjidhni një opsion: ");
        string? zgjedhja = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(zgjedhja)) continue;
        if (zgjedhja.ToLower() == "exit") break;

        string kërkesa = "";

       
        
        switch (zgjedhja)
        {
            case "1":
                kërkesa = "admin:read";
                break;
            case "2":
                Console.Write("Shkruaj tekstin që dëshiron të ruash në server: ");
                string? teksti = Console.ReadLine();
                kërkesa = "admin:write:" + teksti;
                break;
            case "3":
                kërkesa = "admin:execute";
                break;
            case "4":
                kërkesa = "admin:ping";
                break;
            default:
               
                kërkesa = "admin:" + zgjedhja;
                break;
        }

        
        byte[] data_e_nisur = Encoding.UTF8.GetBytes(kërkesa);
        client.Send(data_e_nisur, data_e_nisur.Length, server_endpoint);
        Console.WriteLine($"[INFO] U dërgua: {kërkesa}");

      
        IPEndPoint sender_ep = new IPEndPoint(IPAddress.Any, 0);
        
        
        client.Client.ReceiveTimeout = 5000; 

        byte[] data_e_pranuar = client.Receive(ref sender_ep);
        string përgjigja = Encoding.UTF8.GetString(data_e_pranuar);

        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n>>> PËRGJIGJA NGA SERVERI: " + përgjigja);
        Console.ResetColor();
        Console.WriteLine("--------------------------------------------------");
    }
}
catch (SocketException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n[GABIM] Serveri nuk po përgjigjet! Sigurohu që Personi 1 e ka ndezur serverin.");
    Console.ResetColor();
}
catch (Exception ex) 
{
    Console.WriteLine("\n[GABIM] Ndodhi një problem: " + ex.Message);
}
finally 
{
    client.Close();
    Console.WriteLine("\nLidhja u mbyll. Shtypni çfarëdo tasti për të dalë...");
    Console.ReadKey();
}