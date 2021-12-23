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
            //Вызов игрового модуля
            OchkoPlaying(deck);
            Console.ReadKey();
        }
        //Метод для создания колоды (задает численное значение и номинал карты)
        public static void AddDifferentCardsToDeck (List<Card> methodDeckName, string methodCardSuit)
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
        public static void AddDifferentCardSuitsToDeck (List <Card> methodDeckName)
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
            //рандомизация раздач
            Random rng = new Random();
            //пример предложения о вытягивании карты
            string requestMessageTemplate = "Хотите ли вы вытянуть еще одну карту?";
            int playerScore = 0;
            while (true)
            {
                int randomDeckCardNumber = rng.Next(methodDeckName.Count);
                Console.WriteLine($"Ваша карта {methodDeckName[randomDeckCardNumber].Rank} {methodDeckName[randomDeckCardNumber].Suit}");
                playerScore = playerScore + methodDeckName[randomDeckCardNumber].NumValue;
                if (playerScore > 21)
                {
                    Console.WriteLine($"Вы проиграли, ваш итоговый счет - {playerScore}");
                    return;
                }
                else if (playerScore == 21)
                {
                    Console.WriteLine("Вы победили, набрав 21 очко (молодец нахуй)");
                    return;
                }
                methodDeckName.RemoveAt(randomDeckCardNumber);
                Console.WriteLine($"Ваш текущий счет = {playerScore}");
                if (!GettingUserChoice(requestMessageTemplate))
                {
                    Console.WriteLine($"Игра завершена, ваш итоговый счет - {playerScore}");
                    return;
                }
            }
        }
        public static bool GettingUserChoice(string requestMessage)
        {
            while (true)
            {
                Console.WriteLine(requestMessage);
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

