using System;
using System.Collections.Generic;
using System.Linq;
using static Card;
using static Player;

namespace MyApp
{
    public class OchkoGame
    {
        public static void Main(string[] args)
        {
            //Создание колоды в 4 масти, используя метод CardEditor
            List<Card> deck = new List<Card>();
            //заполнение колоды двумя методами
            AddDifferentCardSuitsToDeck(deck);
            OchkoPlaying(deck);
            Console.ReadKey();
        }
        //Метод для создания колоды (задает численное значение и номинал карты)
        public static void AddDifferentCardsToDeck(List<Card> methodDeckName, string methodCardSuit)
        {
            methodDeckName.Add(new Card() { Rank = "двойка", Suit = methodCardSuit, NumValue = 2 });
            methodDeckName.Add(new Card() { Rank = "тройка", Suit = methodCardSuit, NumValue = 3 });
            methodDeckName.Add(new Card() { Rank = "четверка", Suit = methodCardSuit, NumValue = 4 });
            methodDeckName.Add(new Card() { Rank = "пятерка", Suit = methodCardSuit, NumValue = 5 });
            methodDeckName.Add(new Card() { Rank = "шестерка", Suit = methodCardSuit, NumValue = 6 });
            methodDeckName.Add(new Card() { Rank = "семерка", Suit = methodCardSuit, NumValue = 7 });
            methodDeckName.Add(new Card() { Rank = "восьмерка", Suit = methodCardSuit, NumValue = 8 });
            methodDeckName.Add(new Card() { Rank = "девятка", Suit = methodCardSuit, NumValue = 9 });
            methodDeckName.Add(new Card() { Rank = "десятка", Suit = methodCardSuit, NumValue = 10 });
            methodDeckName.Add(new Card() { Rank = "валет", Suit = methodCardSuit, NumValue = 2 });
            methodDeckName.Add(new Card() { Rank = "дама", Suit = methodCardSuit, NumValue = 3 });
            methodDeckName.Add(new Card() { Rank = "король", Suit = methodCardSuit, NumValue = 4 });
            methodDeckName.Add(new Card() { Rank = "туз", Suit = methodCardSuit, NumValue = 11 });
        }
        public static void AddDifferentCardSuitsToDeck(List<Card> methodDeckName)
        {
            string CardSuit = "червей";
            AddDifferentCardsToDeck(methodDeckName, CardSuit);
            CardSuit = "бубей";
            AddDifferentCardsToDeck(methodDeckName, CardSuit);
            CardSuit = "пик";
            AddDifferentCardsToDeck(methodDeckName, CardSuit);
            CardSuit = "крести";
            AddDifferentCardsToDeck(methodDeckName, CardSuit);
        }
        //Игровой модуль
        public static void OchkoPlaying(List<Card> methodDeckName)
        {
            //ввод пользователей в игру
            List<Player> players = new List<Player>();
            Console.WriteLine("Введите количество игроков");
            int playerCount = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i <= playerCount - 1; i++)
            {
                Console.WriteLine($"Введите имя {i + 1}-ого игрока");
                players.Add(new Player() { Name = Console.ReadLine() });
            }

            //рандомизация раздач
            Random rng = new Random();
            //пример предложения о вытягивании карты
            string requestMessageTemplate = ", хотите ли вы вытянуть еще одну карту?";
            int playersInGame = playerCount;
            int[] winners = new int[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                int randomDeckCardNumber = rng.Next(methodDeckName.Count);
                Console.WriteLine($"{players[i].Name}, ваша карта {methodDeckName[randomDeckCardNumber].Rank} {methodDeckName[randomDeckCardNumber].Suit}");
                players[i].Score = players[i].Score + methodDeckName[randomDeckCardNumber].NumValue;
                methodDeckName.RemoveAt(randomDeckCardNumber);
            }
            while (playersInGame >= 1)
            {

                for (int i = 0; i < playerCount; i++)
                {
                    if (players[i].Score == 0)
                    {
                        continue;
                    }
                    Console.WriteLine($"{players[i].Name}, ваш текущий счет = {players[i].Score}");
                    if (!GettingUserChoice(players[i].Name, requestMessageTemplate))
                    {
                        Console.WriteLine($"{players[i].Name}, игра завершена, ваш итоговый счет - {players[i].Score}");
                      
                        winners[i] = players[i].Score;
                        players[i].Score = 0;
                        playersInGame = playersInGame - 1;
                        continue;
                    }
                    int randomDeckCardNumber = rng.Next(methodDeckName.Count);
                    Console.WriteLine($"{players[i].Name}, ваша карта {methodDeckName[randomDeckCardNumber].Rank} {methodDeckName[randomDeckCardNumber].Suit}");
                    players[i].Score = players[i].Score + methodDeckName[randomDeckCardNumber].NumValue;
                    methodDeckName.RemoveAt(randomDeckCardNumber);
                   

                    if (players[i].Score > 21)
                    {
                        Console.WriteLine($"{players[i].Name}, вы выбыли из игры, ваш итоговый счет - {players[i].Score}");
                        
                        players[i].Score = 0;
                        playersInGame = playersInGame - 1;
                        continue;

                    }
                    else if (players[i].Score == 21)
                    {
                        Console.WriteLine($"{players[i].Name}, вы победили, набрав 21 очко (молодец нахуй)");
                        Console.ReadKey();
                        playersInGame = playersInGame - 1;
                        winners[i] = 21;
                        players[i].Score = 0;
                        continue;

                    }
                    
                   

                }
            }
        }
        public static bool GettingUserChoice(string userName, string requestMessage)
        {
            while (true)
            {
                Console.WriteLine($"{userName}{requestMessage}");
                string userAnswer = Console.ReadLine();
                if (userAnswer.ToLower() == "да")
                {
                    return true;
                }
                else if (userAnswer.ToLower() == "нет")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Хуйню ввел, попробуй еще раз");
                }
            }

        }
    }
}

