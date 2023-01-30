using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameHub
{
     class BatalhaNaval
    {

        public BatalhaNaval() { }

        public void Run()
        {
            Console.Clear();
            // Intro Text
            Console.WriteLine("Bem-vindo ao navio de guerra!");
            Console.WriteLine("A cor azul [~] representa água inexplorada\n" + "A cor vermelha[*] representa um navio que foi atingido \n"
            + "A cor verde[O] representa seus navios.\n");
            Console.WriteLine("O lado esquerdo é o seu mapa de tiro, o lado direito são os seus tiros de barcos e inimigos.");
            // Texto de introdução

            // Entrada de nome
            Console.WriteLine("Por favor, insira seu nome: ");
            string nome = Console.ReadLine();
            if (nome.Length == 0 || nome == " ")
            {
                Console.WriteLine("Nada digitado, pressione Enter para reiniciar");
                Console.ReadLine();
                Console.Clear();
                Run();
            }
            else if (nome.Length > 1)
                nome = nome.Substring(0, 1).ToUpper() + nome.Substring(1);
            Console.WriteLine($"Bem-vindo {nome}.");
            // Entrada de nome

           // Criação de Conselho
            bool mostrarNavios = false; //mostrar a posição dos navios inimigos
            if (nome == "Iamacheater")
            {
                mostrarNavios = true;
            }
            Dictionary<char, int> Coordenadas = PopulateDictionary();
            PrintHeader();
            for (int h = 0; h < 19; h++)
            {
                Console.Write(" ");
            }
            //Criação de Conselho

            #region Game
             EVFNavyAsset EVFNavyAsset = new EVFNavyAsset();
            EVFNavyAsset EVFEnemyNavyAsset = new EVFNavyAsset();

            PrintMap(EVFNavyAsset.FirePositions, EVFNavyAsset, EVFEnemyNavyAsset, mostrarNavios);

            int jogo;
            for (jogo = 1; jogo < 101; jogo++)
            {
                EVFNavyAsset.StepsTaken++;

                Position position = new Position();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Digite as coordenadas EX:(e.g. A3).");
                string input = Console.ReadLine();
                position = AnalyzeInput(input, Coordenadas);

                if (position.x == -1 || position.y == -1)
                {
                    Console.WriteLine("Coordenadas inválidas!");
                    jogo--;
                    continue;
                }

                if (EVFNavyAsset.FirePositions.Any(EFP => EFP.x == position.x && EFP.y == position.y))
                {
                    Console.WriteLine("Esta coordenada ja foi atacada.");
                    jogo--;
                    continue;
                }


                EVFEnemyNavyAsset.Fire();


                var index = EVFNavyAsset.FirePositions.FindIndex(p => p.x == position.x && p.y == position.y);

                if (index == -1)
                    EVFNavyAsset.FirePositions.Add(position);

                Console.Clear();



                EVFNavyAsset.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                EVFNavyAsset.CheckShipStatus(EVFEnemyNavyAsset.FirePositions);

                EVFEnemyNavyAsset.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                EVFEnemyNavyAsset.CheckShipStatus(EVFNavyAsset.FirePositions);

                PrintHeader();
                for (int h = 0; h < 19; h++)
                {
                    Console.Write(" ");
                }



                PrintMap(EVFNavyAsset.FirePositions, EVFNavyAsset, EVFEnemyNavyAsset, mostrarNavios);

                Commentator(EVFNavyAsset, true);
                Commentator(EVFEnemyNavyAsset, false);
                if (EVFEnemyNavyAsset.IsObliteratedAll || EVFNavyAsset.IsObliteratedAll) { break; }


            }

            Console.ForegroundColor = ConsoleColor.White;

            if (EVFEnemyNavyAsset.IsObliteratedAll && !EVFNavyAsset.IsObliteratedAll)
            {
                Console.WriteLine($"Jogo terminou, você ganha. {nome}");
            }
            else if (!EVFEnemyNavyAsset.IsObliteratedAll && EVFNavyAsset.IsObliteratedAll)
            {
                Console.WriteLine($"Jogo Terminado, você perde. {nome},A única jogada vencedora é não jogar. "); 
            }
            else
            {
                Console.WriteLine("Jogo encerrado, empate.");
            }

            Console.WriteLine("Total de passos dados:{0} ", jogo);
            Console.ReadLine();


        }

        static void PrintStatistic(int x, int y, EVFNavyAsset navyAsset)
        {
            if (x == 1 && y == 10)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Indicador:    ");
            }

            if (x == 2 && y == 10)
            {
                if (navyAsset.IsBattleshipSunk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("encouraçado [5]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("encouraçado [5]");
                }
            }

            if (x == 3 && y == 10)
            {

                if (navyAsset.IsDestroyerSunk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Destruidor [4] ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Destruidor [4] ");
                }
            }

            if (x == 4 && y == 10)
            {

                if (navyAsset.IsDestroyer2Sunk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Destruidor [4] ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Destruidor [4] ");
                }
            }

            if (x > 4 && y == 10)
            {
                for (int i = 0; i < 14; i++)
                {
                    Console.Write(" ");
                }
            }

        }

        static void PrintMap(List<Position> positions, EVFNavyAsset MyNavyAsset, EVFNavyAsset EnemyMyNavyAsset, bool showEnemyShips)
        {
            PrintHeader();
            Console.WriteLine();
            if (!showEnemyShips)
                showEnemyShips = MyNavyAsset.IsObliteratedAll;

            List<Position> SortedLFirePositions = positions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Position> SortedShipsPositions = EnemyMyNavyAsset.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedShipsPositions = SortedShipsPositions.Where(FP => !SortedLFirePositions.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();


            int hitCounter = 0;
            int EnemyshipCounter = 0;
            int myShipCounter = 0;
            int enemyHitCounter = 0;

            char row = 'A';
            try
            {
                for (int x = 1; x < 11; x++)
                {
                    for (int y = 1; y < 11; y++)
                    {
                        bool keepGoing = true;

                        #region row indicator
                        if (y == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("[" + row + "]");
                            row++;
                        }
                        #endregion


                        if (SortedLFirePositions.Count != 0 && SortedLFirePositions[hitCounter].x == x && SortedLFirePositions[hitCounter].y == y)
                        {

                            if (SortedLFirePositions.Count - 1 > hitCounter)
                                hitCounter++;

                            if (EnemyMyNavyAsset.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                            {

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("[*]");

                                keepGoing = false;

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("[X]");

                                keepGoing = false;

                            }

                        }

                        if (keepGoing && showEnemyShips && SortedShipsPositions.Count != 0 && SortedShipsPositions[EnemyshipCounter].x == x && SortedShipsPositions[EnemyshipCounter].y == y)

                        {

                            if (SortedShipsPositions.Count - 1 > EnemyshipCounter)
                                EnemyshipCounter++;

                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("[O]");
                            keepGoing = false;
                        }

                        if (keepGoing)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("[~]");
                        }


                        PrintStatistic(x, y, MyNavyAsset);


                        if (y == 10)
                        {
                            Console.Write("      ");

                            PrintMapOfEnemy(x, row, MyNavyAsset, EnemyMyNavyAsset, ref myShipCounter, ref enemyHitCounter);
                        }
                    }

                    Console.WriteLine();
                }

            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static void PrintMapOfEnemy(int x, char row, EVFNavyAsset MyNavyAsset, EVFNavyAsset EnemyNavyAsset, ref int MyshipCounter, ref int EnemyHitCounter)
        {
            List<Position> EnemyFirePositions = new List<Position>();
            row--;
            Random random = new Random();
            List<Position> SortedLFirePositions = EnemyNavyAsset.FirePositions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Position> SortedLShipsPositions = MyNavyAsset.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedLShipsPositions = SortedLShipsPositions.Where(FP => !SortedLFirePositions.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();


            try
            {

                for (int y = 1; y < 11; y++)
                {
                    bool keepGoing = true;

                    #region row indicator
                    if (y == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("[" + row + "]");
                        row++;
                    }
                    #endregion


                    if (SortedLFirePositions.Count != 0 && SortedLFirePositions[EnemyHitCounter].x == x && SortedLFirePositions[EnemyHitCounter].y == y)
                    {

                        if (SortedLFirePositions.Count - 1 > EnemyHitCounter)
                            EnemyHitCounter++;

                        if (MyNavyAsset.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[*]");

                            keepGoing = false;

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("[X]");

                            keepGoing = false;

                        }

                    }

                    if (keepGoing && SortedLShipsPositions.Count != 0 && SortedLShipsPositions[MyshipCounter].x == x && SortedLShipsPositions[MyshipCounter].y == y)

                    {

                        if (SortedLShipsPositions.Count - 1 > MyshipCounter)
                            MyshipCounter++;

                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("[O]");

                        keepGoing = false;

                    }

                    if (keepGoing)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("[~]");
                    }


                    PrintStatistic(x, y, EnemyNavyAsset);

                }


            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static Position AnalyzeInput(string input, Dictionary<char, int> Coordinates)
        {
            Position pos = new Position();

            char[] inputSplit = input.ToUpper().ToCharArray();


            if (inputSplit.Length < 2 || inputSplit.Length > 4)
            {
                return pos;
            }




            if (Coordinates.TryGetValue(inputSplit[0], out int value))
            {
                pos.x = value;
            }
            else
            {
                return pos;
            }


            if (inputSplit.Length == 3)
            {

                if (inputSplit[1] == '1' && inputSplit[2] == '0')
                {
                    pos.y = 10;
                    return pos;
                }
                else
                {
                    return pos;
                }

            }


            if (inputSplit[1] - '0' > 9)
            {
                return pos;
            }
            else
            {
                pos.y = inputSplit[1] - '0';
            }

            return pos;
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[ ]");
            for (int i = 1; i < 11; i++)
                Console.Write("[" + i + "]");
        }


        static Dictionary<char, int> PopulateDictionary()
        {
            Dictionary<char, int> Coordinate =
                     new Dictionary<char, int>
                     {
                         { 'A', 1 },
                         { 'B', 2 },
                         { 'C', 3 },
                         { 'D', 4 },
                         { 'E', 5 },
                         { 'F', 6 },
                         { 'G', 7 },
                         { 'H', 8 },
                         { 'I', 9 },
                         { 'J', 10 }
                     };

            return Coordinate;
        }

        static void Commentator(EVFNavyAsset navyAsset, bool isMyShip)
        {

            string title = isMyShip ? "Seu" : "Inimigo";

            if (navyAsset.CheckPBattleship && navyAsset.IsBattleshipSunk)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1} afundou", title, nameof(navyAsset.Battleship));
                navyAsset.CheckPBattleship = false;
            }

            if (navyAsset.CheckDestroyer && navyAsset.IsDestroyerSunk)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1} afundou", title, nameof(navyAsset.Destroyer));
                navyAsset.CheckDestroyer = false;
            }

            if (navyAsset.CheckDestroyer2 && navyAsset.IsDestroyer2Sunk)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1} afundou", title, nameof(navyAsset.Destroyer2));
                navyAsset.CheckDestroyer2 = false;
            }
            #endregion Game

        }
    }

}

