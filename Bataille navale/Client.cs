using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1;
using System.Threading;

namespace Bataille_navale
{
    class Client
    {
        public static string responseData;
        public static async Task SendMessage(String server, String message)
        {
            try
            {
                // Création des variables contenant le port et l'adresse IP sur lequel envoyer.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                // Transcription du message à envoyer en bytes.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Ouverture de la connection avec le server.
                NetworkStream stream = client.GetStream();
                // Envoie du message en bytes.
                int recv = 0;
                foreach (byte b in data)
                {
                    if (b != 0)
                    {
                        recv++;
                    }
                }
                //stream.Write(data, 0, recv);
                stream.Flush();
                //Affichage du message envoyé.
                Console.WriteLine("Sent: {0}", message);
                // Receive the TcpServer.response.
                Thread.Sleep(1000);
                stream.Flush();

                // Close everything.
                stream.Close();
                client.Close();
                Thread.Sleep(100);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
        public static async Task Send(String server, String message)
        {
            try
            {
                // Création des variables contenant le port et l'adresse IP sur lequel envoyer.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                // Transcription du message à envoyer en bytes.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Ouverture de la connection avec le server.
                NetworkStream stream = client.GetStream();

                // Envoie du message en bytes.
                stream.Write(data, 0, data.Length);
                stream.Flush();
                //Affichage du message envoyé.
                Console.WriteLine("Sent: {0}", message);
                // Receive the TcpServer.response.
                Thread.Sleep(100);
                stream.Flush();

                // Receive the result of the attack
                data = new Byte[256];
                // String to store the response ASCII representation.
                responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                stream.Flush();
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                if(responseData.Contains("Game Over"))
                {
                    Console.WriteLine("Vous avez gagné. Félicitations.");
                    Program.GameOver();
                }

                // Receive the ennemy the attack
                data = new Byte[256];
                // String to store the response ASCII representation.
                responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
                bytes = stream.Read(data, 0, data.Length);
                stream.Flush();
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Vous avez été attaqué en : {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
                Thread.Sleep(100);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static async Task AnswerToAttack(string server, string message, bool gameOver)
        {
            try
            {
                // Création des variables contenant le port et l'adresse IP sur lequel envoyer.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                // Transcription du message à envoyer en bytes.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Ouverture de la connection avec le server.
                NetworkStream stream = client.GetStream();
                // Envoie du message en bytes.
                int recv = 0;
                foreach (byte b in data)
                {
                    if (b != 0)
                    {
                        recv++;
                    }
                }
                stream.Write(data, 0, recv);
                //Affichage du message envoyé.
                if (!gameOver)
                    Console.WriteLine("Sent: {0}", message);
                else if (gameOver)
                {
                    Console.WriteLine("Game Over!");
                }

                // Receive the TcpServer.response.

                // Close everything.
                stream.Close();
                client.Close();
                Thread.Sleep(100);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}

