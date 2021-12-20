using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        public static int longitudeShot = 0;
        public static int latitudeShot = 0;
        static void Main(string[] args)
        {
            string[,] battleMap = new string[10, 10];
            Dictionary<string, int> boats = new Dictionary<string, int>();
            boats.Add("carrier", 5);
            boats.Add("battleship", 4);
            boats.Add("cruiser", 3);
            boats.Add("submarine", 3);
            boats.Add("destroyer", 2);

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

            // Bonne chance guillaume
            bool positionBoat = false;
            while (!positionBoat)
            {
                for (int i = 5; i > 0; i--)
                {

                    bool longLatTest = false;
                    while (!longLatTest)
                    {
                        try
                        {
                            int longitudeBoat = 0;
                            int latitudeBoat = 0;

                            Console.WriteLine("Indiquez les coordonées de votre porte-avion.");
                            playerInput = Console.ReadLine();


                            string linePosition = playerInput.Substring(0, 1/*EXCLU*/);
                            string columnPosition = playerInput.Substring(1);


                            // Verifier la validité de la ligne
                            if (longitudeCheck.IsMatch(linePosition))
                            {
                                longitudeBoat = lineCharacterToInt(playerInput[0]);
                            }
                            else
                            {
                                throw new Exception("La lettre de la ligne n'est pas valide");
                            }
                            // Verifier la validité de la colonne
                            if (latitudeCheck.IsMatch(columnPosition)
                                && int.Parse(columnPosition) > 0 && int.Parse(columnPosition) <= 10)
                            {
                                latitudeBoat = int.Parse(columnPosition) - 1;
                            }
                            else
                            {
                                throw new Exception("Le chiffre de la colonne n'est pas valide");
                            }

                            if (battleMap[longitudeBoat, latitudeBoat] == null)
                            {
                                            
                                if (battleMap[longitudeBoat + 1, latitudeBoat] != null || battleMap[longitudeBoat + 1, latitudeBoat + 1] != null || battleMap[longitudeBoat + 1, latitudeBoat - 1] != null || battleMap[longitudeBoat - 1, latitudeBoat] != null || battleMap[longitudeBoat - 1, latitudeBoat - 1] != null || battleMap[longitudeBoat, latitudeBoat + 1] != null || battleMap[longitudeBoat, latitudeBoat - 1] != null)
                                {
                                    if (battleMap[longitudeBoat + 1, latitudeBoat] == "carrier" || battleMap[longitudeBoat - 1, latitudeBoat] != "carrier" || battleMap[longitudeBoat, latitudeBoat + 1] != "carrier" || battleMap[longitudeBoat, latitudeBoat - 1] != "carrier")
                                    {
                                        battleMap[longitudeBoat, latitudeBoat] = "carrier";
                                        longLatTest = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("votre coordonné est trop près d'un autre bateau, Veuillez en entrer une autre");
                                    }
                                }
                                else
                                {
                                    battleMap[longitudeBoat, latitudeBoat] = "carrier";
                                    longLatTest = true;
                                }
                            }
                            else if (battleMap[longitudeBoat, latitudeBoat] != null)
                            {
                                Console.WriteLine("il existe déja un bateau à cette coordonné, veuillez en entrer une autre");
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                            continue;
                        }
                        positionBoat = !positionBoat;

                    }
                }
            }



            while (true)
            {
                Console.WriteLine("Indiquez les coordonées du tir.");
                playerInput = Console.ReadLine();


                try
                {
                    string linePosition = playerInput.Substring(0, 1/*EXCLU*/);
                    string columnPosition = playerInput.Substring(1);
                    // **************************************************************************************************
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
                //*************************************************************************************************
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    continue;
                }
                Console.WriteLine($"{longitudeShot},{latitudeShot}");

                // Attaque de l'adversaire
                if (battleMap[longitudeShot, latitudeShot] == null)
                {
                    Console.WriteLine("On a entendu plouf au loin. Et des rires distants.");
                }
                else if (battleMap[longitudeShot, latitudeShot] == "Touché")
                {
                    Console.WriteLine($"Vous avez déjà tiré sur cette case. Achetez-vous des lunettes.");
                }
                else if (battleMap[longitudeShot, latitudeShot] != null)
                {
                    Console.WriteLine(battleMap[longitudeShot, latitudeShot]);

                    Console.WriteLine($"Le bateau de type {battleMap[longitudeShot, latitudeShot].ToString()} a été touché en {playerInput}");
                    boats[battleMap[longitudeShot, latitudeShot]] -= 1;
                    if (boats[battleMap[longitudeShot, latitudeShot]] == 0)
                    {
                        battleMap[longitudeShot, latitudeShot] = "Touché";
                        Console.WriteLine("Touché. Coulé.");
                    }
                    else
                    {
                        battleMap[longitudeShot, latitudeShot] = "Touché";
                    }
                }
            }
            Console.WriteLine("Fin de la boucle. Pourquoi êtes-vous ici?");
            Console.ReadLine();


            /* static int validityTest(string linePosition, string columnPosition, Regex longitudeCheck, Regex latitudeCheck)
            {
                int longitudeBoat = 0;
                int latitudeBoat = 0;

            if (longitudeCheck.IsMatch(linePosition))
            {
                return longitudeBoat = lineCharacterToInt(playerInput[0]);
            }
            else
            {
                return new Exception("La lettre de la ligne n'est pas valide");
            }
            // Verifier la validité de la colonne
            if (latitudeCheck.IsMatch(columnPosition)
                && int.Parse(columnPosition) > 0 && int.Parse(columnPosition) <= 10)
            {
                return latitudeBoat = int.Parse(columnPosition) - 1;
            }
            else
            {
                return new Exception("Le chiffre de la colonne n'est pas valide");
            }

            } */

            int lineCharacterToInt(char charToUnicode)
            {
                return (int)charToUnicode - (int)'A';
            }



        }
    }
}