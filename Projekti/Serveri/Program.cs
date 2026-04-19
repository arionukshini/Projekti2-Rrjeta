using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Serveri
{
    static void Main()
    {
        int port = 4444;

        UdpClient server = new UdpClient(port);
        Console.WriteLine("Serveri u startua ne port " + port);

        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);

        string server_files = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\ServerFiles");

        if (!Directory.Exists(server_files))
            Directory.CreateDirectory(server_files);

        Console.WriteLine("Folder: " + server_files);

        while (true)
        {
            byte[] data = server.Receive(ref endpoint);
            string msg = Encoding.UTF8.GetString(data);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[{DateTime.Now}] {endpoint}");
            Console.ResetColor();
            Console.WriteLine("Kerkesa: " + msg);

            string response = "";

            try
            {
                string[] parts = msg.Split(':', 5);

                string role = parts[0];
                string command = parts.Length > 1 ? parts[1] : "";

                if (role == "admin")
                {
                    switch (command)
                    {
                        case "list":
                            var files = Directory.GetFiles(server_files);
                            response = files.Length == 0
                                ? "Nuk ka file."
                                : string.Join("\n", files.Select(Path.GetFileName));
                            break;

                        case "read":
                            if (parts.Length < 3)
                            {
                                response = "Jep emrin e file!";
                                break;
                            }

                            string readPath = Path.Combine(server_files, parts[2]);

                            if (!File.Exists(readPath))
                            {
                                response = "File nuk ekziston!";
                                break;
                            }

                            response = File.ReadAllText(readPath);
                            break;

                        case "write":
                            if (parts.Length < 5)
                            {
                                response = "Format gabim!";
                                break;
                            }

                            string mode = parts[2];
                            string filename = parts[3];
                            string content = parts[4];

                            string fullPath = Path.Combine(server_files, filename);

                            if (mode == "new")
                            {
                                File.WriteAllText(fullPath, content);
                                response = "File u krijua!";
                            }
                            else if (mode == "append")
                            {
                                if (!File.Exists(fullPath))
                                {
                                    response = "File nuk ekziston!";
                                    break;
                                }

                                File.AppendAllText(fullPath, content + Environment.NewLine);
                                response = "U shtua ne file!";
                            }
                            else
                            {
                                response = "Mode i panjohur!";
                            }

                            break;

                        case "message":
                            string message = parts[2];

                            response = "Mesazhi u pranua nga serveri!";
                            Console.WriteLine("Mesazhi: " + message);

                            break;

                        case "ping":
                            response = "PONG";
                            break;

                        default:
                            response = "Komande e panjohur!";
                            break;
                    }
                }
                else if (role == "readonly")
                {
                    switch (command)
                    {
                        case "list":
                            var files = Directory.GetFiles(server_files);
                            response = files.Length == 0
                                ? "Nuk ka file."
                                : string.Join("\n", files.Select(Path.GetFileName));
                            break;

                        case "read":
                            if (parts.Length < 3)
                            {
                                response = "Jep emrin e file!";
                                break;
                            }

                            string readPath = Path.Combine(server_files, parts[2]);

                            if (!File.Exists(readPath))
                            {
                                response = "File nuk ekziston!";
                                break;
                            }

                            response = File.ReadAllText(readPath);
                            break;

                        case "ping":
                            response = "PONG";
                            break;

                        default:
                            response = "NUK KE AKSES!";
                            break;
                    }
                }
                else
                {
                    response = "Role i panjohur!";
                }
            }
            catch (Exception ex)
            {
                response = "ERROR: " + ex.Message;
            }

            byte[] reply = Encoding.UTF8.GetBytes(response);
            server.Send(reply, reply.Length, endpoint);
        }
    }
}