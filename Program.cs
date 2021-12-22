using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Card> deck = new List<Card>();
            string CardSuit = "червей";
            CardEditor(deck, CardSuit);
            CardSuit = "бубей";
            CardEditor(deck, CardSuit);
            CardSuit = "пик";
            CardEditor(deck, CardSuit);
            CardSuit = "крести";
            CardEditor(deck, CardSuit);

            Console.WriteLine($"Ваша карта {deck[51].Rank} {deck[51].Suit}");
            
          
            Console.ReadKey();
        }
        //переменная isdeckshort создана для возможности игры колодой из 36 карт
        public static void CardEditor (List <Card> methodDeckName, string methodCardSuit, bool isdeckshort=false)
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