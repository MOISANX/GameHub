using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using gameHub;

namespace ConsoleGameHub
{
    class Program
    {
            static List<PlayerScore> playerScores = new List<PlayerScore>();
        static void Main(string[] args)
        {
            Dictionary<string, string> players = new Dictionary<string, string>();
             Dictionary<string, int> scores = new Dictionary<string, int>();
            LoadPlayers(players);

            Console.WriteLine("Bem-vindo ao Console Game Hub");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Cadastrar");
            Console.WriteLine("3. Ranque dos jogadores");
            Console.WriteLine("Escolha uma opção:");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Digite seu nome de usuário:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Digite sua senha:");
                    string password = Console.ReadLine();
                    if (players.ContainsKey(username) && players[username] == password)
                    {
                        Console.WriteLine("Login realizado com sucesso!");
                        Console.WriteLine("Escolha um jogo para jogar:");
                        Console.WriteLine("1. Jogo 1");
                        Console.WriteLine("2. Jogo 2");
                   
                        int gameChoice = Convert.ToInt32(Console.ReadLine());
                        switch (gameChoice)
                        {
                            case 1:
                                Console.Title = "JogoDavelha Versão MOISANX";
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Clear();
                                new JogoDaVelha().Menu();
                                break;
                            case 2:
                              new BatalhaNaval().Run();
                                break;
                              
                            default:
                                Console.WriteLine("Opção inválida");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nome de usuário ou senha incorretos");
                    }
                    break;
                case 2:
                    Console.WriteLine("Digite seu nome de usuário:");
                    string newUsername = Console.ReadLine();
                    Console.WriteLine("Digite sua senha:");
                    string newPassword = Console.ReadLine();
                    if (players.ContainsKey(newUsername))
                    {
                        Console.WriteLine("Nome de usuário já está em uso");
                    }
                    else
                    {
                        players.Add(newUsername, newPassword);
                        SavePlayers(players);
                        Console.WriteLine("Cadastro realizado com sucesso!");
                    }
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;

                case 3:
                    ShowRanking();
                    break;
            }
        }

        static void ShowRanking()
        {
            LoadScores();
            playerScores.Sort((a, b) => b.score.CompareTo(a.score));
            Console.WriteLine("Ranque dos jogadores:");
            for (int i = 0; i < playerScores.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + playerScores[i].username + " - " + playerScores[i].score + " pontos");
            }
        }
        static Dictionary<string, int> scores = new Dictionary<string, int>();

        static void LoadScores()
        {
            if (File.Exists("scores.txt"))
            {
                string[] lines = File.ReadAllLines("scores.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    scores[parts[0]] = Convert.ToInt32(parts[1]);
                }
            }
        }

        static void SaveScores()
        {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, int> score in scores)
            {
                lines.Add(score.Key + ":" + score.Value);
            }
            File.WriteAllLines("scores.txt", lines.ToArray());
        }




        static void LoadPlayers(Dictionary<string, string> players)
        {
            if (File.Exists("players.txt"))
            {
                string[] lines = File.ReadAllLines("players.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    players[parts[0]] = parts[1];
                }
            }
        }
        static void SavePlayers(Dictionary<string, string> players)
        {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, string> player in players)
            {
                lines.Add(player.Key + ":" + player.Value);
            }
            File.WriteAllLines("players.txt", lines.ToArray());
        }
    }
}
