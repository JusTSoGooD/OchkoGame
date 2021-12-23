﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Создание колоды в 4 масти, используя метод CardEditor
            List<Card> deck = new List<Card>();
            string CardSuit = "червей";
            CardEditor(deck, CardSuit);
            CardSuit = "бубей";
            CardEditor(deck, CardSuit);
            CardSuit = "пик";
            CardEditor(deck, CardSuit);
            CardSuit = "крести";
            CardEditor(deck, CardSuit);

            //Вызов игрового модуля
            PlayingModule(deck);

            Console.ReadKey();
        }
        //переменная isdeckshort создана для возможности игры колодой из 36 карт
        //Метод для создания колоды (задает численное значение и номинал карты)
        public static void CardEditor(List<Card> methodDeckName, string methodCardSuit, bool isdeckshort = false)
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
        //Игровой модуль
        public static void PlayingModule(List<Card> methodDeckName)
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
                if (!GetUserChoice(requestMessageTemplate))
                {
                    Console.WriteLine($"Игра завершена, ваш итоговый счет - {playerScore}");
                    return;     
                } 
                
            }
        }
        public static bool GetUserChoice(string requestMessage)
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
    public class Card
    {
        private string rank = "Undefined";
        public string Rank
        {
            get
            {
                return rank;
            }
            set
            {
                rank = value;
            }
        }
        private string suit = "Undefined";

        public string Suit
        {
            get
            {
                return suit;
            }
            set
            {
                suit = value;
            }
        }
        private int numvalue = 0;
        public int NumValue
        {
            get
            {
                return numvalue;
            }
            set
            {
                numvalue = value;
            }
        }


    }
    public class Player
    {
        public string name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}