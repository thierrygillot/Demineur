using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demineur
{
    class Program
    {
        static int nbCols;
        static int nbRows;
        static int[,] minesField;
        static int nbBombs;
        static int crsX = 0;
        static int crsY = 0;


        //second terrain de jeu
        static bool[,] visitedField;//si case visité alors....

        //Objet Random - l'aléatoire n'existe pas en informatique
        //il s'agit de millième de secondes choisies
        static Random rand = new Random();
        static void Main(string[] args)
        {
            //on travaille avec des char utf-8
            Console.OutputEncoding = Encoding.UTF8;

            //action 1 Paramètres du jeu
            Console.WriteLine("Entrez un nombre de lignes");
            //demande au user de rentrer un nombre
            //si conversion non faite, alors redemande
            while (!int.TryParse(Console.ReadLine(), out nbRows))
            {
                Console.WriteLine("Entrez un nombre de lignes");
            }
            //action 2
            Console.WriteLine("Entrez un nombre de colonnes");
            //demande au user de rentrer un nombre
            //si conversion non faite, alors redemande
            while (!int.TryParse(Console.ReadLine(), out nbCols))
            {
                Console.WriteLine("Entrez un nombre de lignes");
            }

            //action 3
            nbBombs = ask("Entrez un nombre de bombes");
            minesField = new int[nbRows, nbRows];
            visitedField = new bool[nbRows,nbCols];

            //Vérification pour qu'il n'y ai pas trop de bombe par rapport à la zone de jeu
            while (nbBombs >= nbCols * nbRows / 2)
            {
                nbBombs = ask("Entrez un nombre de bombes");
            }
            
            //ici on place les bombes au hasard
            //Si la valeur égale 0 alors on place la bombe
            //Sinon recommencer
            for (int i = 0; i < nbBombs; i++)
            {
                int cols;
                int rows;
                do
                {
                    cols = rand.Next(0, nbCols);
                    rows = rand.Next(0, nbRows);
                } while (minesField[rows, cols] != 0);
                minesField[rows, cols] = 9;
            }

            getNumbers();
           
            //utilisation du curseur
            while (true)
            {
                //on efface la console à chaque tour de boucle et on réaffiche le visitedField
                Console.Clear();
                displayVisitedField();
                //on place le cursor à la nouvelle position
                Console.SetCursorPosition(crsX, crsY);
                //touche sur laquelle on appuye
                ConsoleKey key = Console.ReadKey().Key;
                if (key== ConsoleKey.UpArrow)
                {
                    if (crsY - 1 >= 0)
                    {
                        crsY--;

                    }
                }

                if (key == ConsoleKey.DownArrow)
                {
                    if (crsY + 1 >= 0)
                    {
                        crsY++;

                    }
                }
                if (key == ConsoleKey.LeftArrow)
                {
                    if (crsX - 1 >= 0)
                    {
                        crsX--;

                    }
                }
                if (key == ConsoleKey.RightArrow)
                {
                    if (crsX + 1 >= 0)
                    {
                        crsX++;

                    }
                }
                if (key == ConsoleKey.Spacebar)
                {
                    checkCase(crsY, crsX);
                }
             
            }
            
            //Affichage >>> le mettre dans une méthode
           
            
        }


        // Les méthodes utilisées

        /// <summary>
        /// Methode qui permet de faire la même chose que les deux premières actions
        /// </summary>
        /// <param name="v"></param>
        /// <returns>La chaine de caractères sous forme d'entiers</returns>
        private static int ask(string v)
        {
            int result;
                Console.WriteLine(v); 
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine(v);
            }

            return result;
        }

        /// <summary>
        /// Affiche la zone de jeu
        /// </summary>
        private static void displayField()
        {
            for (int row = 0; row < minesField.GetLength(0); row++)
            {

                for (int col = 0; col < minesField.GetLength(1); col++)
                {

                    //si bombe, change en rouge
                    if (minesField[row, col] == 9)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(minesField[row, col]);
                    //remettre du blanc après le rouge
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();

            }
        }

        /// <summary>
        /// Incrémente le nombre de 1 si il y a une bombe aux alentours
        /// </summary>
        private static void getNumbers()
        {
            for (int row = 0; row < minesField.GetLength(0); row++)
            {
                for (int col = 0; col < minesField.GetLength(1); col++)
                {

                    if (minesField[row, col] ==9)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            if (row + i >= 0 && row + i < nbRows)

                            {
                                for (int j = -1; j <= 1; j++)
                                    if (col + j >= 0 && col + j < nbCols)
                                    {
                                        if (minesField[row + i, col + j] != 9)
                                        {
                                            minesField[row + i, col + j]++;
                                        }
                                    }
                            }
                        }
                    }
                    
                    
                }
            }
        }


        /// <summary>
        /// Affiche la grille avec des carreaux
        /// </summary>
        private static void displayVisitedField()
        {
            for (int row = 0 ; row < visitedField.GetLength(0); row++)
            {
                for (int col = 0; col < visitedField.GetLength(1); col++)
                {
                    if (visitedField[row, col])
                    {
                        Console.Write(minesField[row, col]);
                    }
                    else
                    {
                        Console.Write("♦");
                    }

                }
                Console.WriteLine();
            }
        }

        //si la case visitée est égale à 0 faire quelque chose....
        static private void checkCase(int x, int y)
        {
            visitedField[y, x] = true;
            if (minesField[y,x] == 0)
            {
                for (int i = -1; i <= 1; i++)
                {
                    if (y+i >=0 && y+i<nbRows)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if ( x+j >= 0 && x+j < nbCols )
                            {
                                if (!visitedField[y + i, x + j])
                                {
                                    checkCase(y + 1, x + j);
                                } 
                            }

                        }
                    }

                }

            }

        }

    }
}
