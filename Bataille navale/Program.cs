﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Bataille_navale;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Client.SendMessage("127.0.0.1", "I'm the first message");
            Client.SendMessage("192.168.1.33", "Connected");
            GameLoop();
        }
        static int lineCharacterToInt(char charToUnicode)
        {
            return (int)charToUnicode - (int)'A';
        }

        public static async Task GameLoop()
        {
            int longitudeShot = 0;
            int latitudeShot = 0;

            string[,] battleMap = new string[10, 10];
            Dictionary<string, int> boats = new Dictionary<string, int>();
            boats.Add("carrier", 5);
            boats.Add("battleship", 4);
            boats.Add("cruiser", 3);
            boats.Add("submarine", 3);
            boats.Add("destroyer", 2);

            int playerHealth = boats.Sum(x => x.Value);

            longitudeShot = 0;
            latitudeShot = 0;

            battleMap[1, 1] = "carrier";
            battleMap[1, 2] = "carrier";
            battleMap[1, 3] = "carrier";
            battleMap[1, 4] = "carrier";
            battleMap[1, 5] = "carrier";
            battleMap[2, 7] = "battleship";
            battleMap[3, 7] = "battleship";
            battleMap[4, 7] = "battleship";
            battleMap[5, 7] = "battleship";
            battleMap[3, 2] = "cruiser";
            battleMap[4, 2] = "cruiser";
            battleMap[5, 2] = "cruiser";
            battleMap[5, 4] = "submarine";
            battleMap[6, 4] = "submarine";
            battleMap[7, 4] = "submarine";
            battleMap[8, 8] = "destroyer";
            battleMap[8, 9] = "destroyer";


            // Input check and crunch
            string playerInput = "";
            Regex longitudeCheck = new Regex(@"^[A-J]+$");
            Regex latitudeCheck = new Regex(@"^[0-9]+$");

            string linePosition = "";
            string columnPosition = ";";
            string answerToAttack = "";

            while (true)
            {
                Console.WriteLine("Indiquez les coordonées du tir.");
                playerInput = Console.ReadLine();

                try
                {
                    linePosition = playerInput.Substring(0, 1/*EXCLU*/);
                    columnPosition = playerInput.Substring(1);

                    // Verifier la validité de la ligne
                    if (longitudeCheck.IsMatch(linePosition))
                    {
                        longitudeShot = lineCharacterToInt(playerInput[0]);
                    }
                    else
                    {
                        throw new Exception("La lettre de la ligne n'est pas valide");
                    }
                    // Verifier la validité de la colonne
                    if (latitudeCheck.IsMatch(columnPosition)
                        && int.Parse(columnPosition) > 0 && int.Parse(columnPosition) <= 10)
                    {
                        latitudeShot = int.Parse(columnPosition) - 1;
                    }
                    else
                    {
                        throw new Exception("Le chiffre de la colonne n'est pas valide");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    continue;
                }

                await Client.Send("192.168.1.33", playerInput);
                playerInput = Client.responseData;

                linePosition = playerInput.Substring(0, 1/*EXCLU*/);
                columnPosition = playerInput.Substring(1);

                longitudeShot = lineCharacterToInt(playerInput[0]);
                latitudeShot = int.Parse(columnPosition) - 1;

                // Attaque de l'adversaire
                if (battleMap[longitudeShot, latitudeShot] == null)
                {
                    answerToAttack = "On a entendu plouf au loin. Et des rires distants.";
                }
                else if (battleMap[longitudeShot, latitudeShot] == "Touché")
                {
                    answerToAttack = $"Vous avez déjà tiré sur cette case. Achetez-vous des lunettes.";
                }
                else if (battleMap[longitudeShot, latitudeShot] != null)
                {
                    //Console.WriteLine(battleMap[longitudeShot, latitudeShot]);

                    //Console.WriteLine($"Le bateau de type {battleMap[longitudeShot, latitudeShot].ToString()} a été touché en {playerInput}");
                    boats[battleMap[longitudeShot, latitudeShot]] -= 1;
                    if (boats[battleMap[longitudeShot, latitudeShot]] == 0)
                    {
                        battleMap[longitudeShot, latitudeShot] = "Touché";
                        answerToAttack = "Touché. Coulé.";
                    }
                    else
                    {
                        battleMap[longitudeShot, latitudeShot] = "Touché";
                        answerToAttack = "Touché.";
                    }
                    playerHealth--;
                }

                // On vérifie à chaque attaque si le joueur a perdu. Si c'est le cas, on sort de la boucle while(true) et on appelle la function Game over
                if (playerHealth == 0)
                {
                    await Client.AnswerToAttack("192.168.1.33", "Game Over", true);
                    break;
                }
                // Sinon, on envoie le résulat de l'attaque au serveur
                else
                {
                    await Client.AnswerToAttack("192.168.1.33", answerToAttack, false);
                }
            }
            Console.WriteLine("Vous avez perdu!");
            GameOver();
        }
        public static void GameOver()
        {
            string playerInput;
            while (true)
            {
                Console.WriteLine("Fin de la partie. Voulez-vous rejouer? Ecrivez oui pour rejouer. Ecrivez non pour quitter.");
                playerInput = Console.ReadLine();
                try
                {
                    if (playerInput == "oui")
                    {
                        Console.Clear();
                        GameLoop();
                    }
                    if (playerInput == "non")
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Commande invalide");
                }
            }
        }
    }
}