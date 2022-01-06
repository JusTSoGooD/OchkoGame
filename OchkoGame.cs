using System;
using System.Collections.Generic;
using System.Linq;
using static Card;
using static Player;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
namespace MyApp
{
    public class OchkoGame
    {
        public static void Main(string[] args)
        {
            List<Card> deck = GenerateDeck();
            OchkoPlaying(deck);
            Console.ReadKey();
        }


        //Метод для создания колоды (задает численное значение и номинал карты)
        public static void AddCardsToDeckBySuit(List<Card> deck, string cardSuit)
        {
            deck.Add(new Card("двойка", cardSuit, 2));
            deck.Add(new Card("тройка", cardSuit, 3));
            deck.Add(new Card("четверка", cardSuit, 4));
            deck.Add(new Card("пятерка", cardSuit, 5));
            deck.Add(new Card("шестерка", cardSuit, 6));
            deck.Add(new Card("семерка", cardSuit, 7));
            deck.Add(new Card("восьмерка", cardSuit, 8));
            deck.Add(new Card("девятка", cardSuit, 9));
            deck.Add(new Card("десятка", cardSuit, 10));
            deck.Add(new Card("валет", cardSuit, 2));
            deck.Add(new Card("дама", cardSuit, 3));
            deck.Add(new Card("король", cardSuit, 4));
            deck.Add(new Card("туз", cardSuit, 11));
        }


        //метод для процесса формирования мастей карт
        public static List<Card> GenerateDeck()
        {
            List<Card> deck = new List<Card>();
            AddCardsToDeckBySuit(deck, "червей");
            AddCardsToDeckBySuit(deck, "бубей");
            AddCardsToDeckBySuit(deck, "пик");
            AddCardsToDeckBySuit(deck, "крести");
            return deck;
        }


        //Метод для процесса вытягивания карт
        public static void DrawCards(List<Card> deck, List<Player> PlayersList, int CycleCounter)
        {
            Random rng = new Random();
            int randomDeckCardNumber = rng.Next(deck.Count);
            Console.WriteLine($"{PlayersList[CycleCounter].Name}, ваша карта {deck[randomDeckCardNumber].Rank} {deck[randomDeckCardNumber].Suit}");
            PlayersList[CycleCounter].Score = PlayersList[CycleCounter].Score + deck[randomDeckCardNumber].Value;
            deck.RemoveAt(randomDeckCardNumber);
        }


        //Игровой модуль
        public static void OchkoPlaying(List<Card> deck)
        {
            //ввод пользователей в игру
            List<Player> players = new List<Player>();

            string path = "C://Users/kuchm/Desktop/Программы c#/Ochko/OchkoGame/users.xml";
            if (!(File.Exists(path)))
            {
                XmlDocument xNewPlayersDocument = new XmlDocument();
                XmlDeclaration xmlDec = xNewPlayersDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xNewPlayersDocument.AppendChild(xmlDec);
                XmlElement playersDatabase = xNewPlayersDocument.CreateElement("Database");
                playersDatabase.SetAttribute("name", "Игроки");
                xNewPlayersDocument.AppendChild(playersDatabase);
                xNewPlayersDocument.Save(path);
            }
            XmlDocument xPlayersDocument = new XmlDocument();
            xPlayersDocument.Load(path);
            int numberOfPlayers = GetNumberOfPlayers();
            for (int i = 0; i <= numberOfPlayers - 1; i++)
            {
                players.Add(new Player() { Name = GetUserName(i), IsPlayerInGame = true });
                bool isPlayerExists = false;
                XmlElement? xRoot = xPlayersDocument.DocumentElement;          
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    if (attr.Value == players[i].Name)
                    {
                        isPlayerExists = true;
                        break;
                    }
                }
                if (!isPlayerExists)
                {
                    XmlElement player = xPlayersDocument.CreateElement("Игрок");
                    player.SetAttribute("name", players[i].Name);
                    player.SetAttribute("winCount", "0");
                    xRoot.AppendChild(player);
                }
            }

            xPlayersDocument.Save(path);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                DrawCards(deck, players, i);
            }
            GameProcess(deck, players);
            WithdrawingGameWinners(players);
        }


        //получение ответа о предложении продолжить игру
        public static bool GetUserChoice(string appealToPLayer)
        {
            while (true)
            {
                Console.WriteLine(appealToPLayer);
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


        //вывод победителей игры
        public static void WithdrawingGameWinners(List<Player> playersList)
        {
            string path = "C://Users/kuchm/Desktop/Программы c#/Ochko/OchkoGame/users.xml";
            XmlDocument xPlayersDocument = new XmlDocument();
            xPlayersDocument.Load(path);
            XmlElement? xRoot = xPlayersDocument.DocumentElement;

            int maxScore = playersList.Max(p => p.Score);
            List<Player> winners = playersList.FindAll(p => p.Score == maxScore);
            Console.WriteLine("Список победителей игры:");
            foreach (Player winner in winners)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    if (attr.Value == winner.Name)
                    {
                        XmlNode? winnerCount = xnode.Attributes.GetNamedItem("winCount");
                        winnerCount.Value = Convert.ToString(Convert.ToInt16(winnerCount.Value)+1);
                        xPlayersDocument.Save(path);  
                    }
                }
                Console.WriteLine($"{winner.Name}, набравший {winner.Score} баллов");
            }
        }


        //Получение количества игроков
        public static int GetNumberOfPlayers()
        {
            while (true)
            {
                Console.WriteLine("Введите количество игроков");
                int playersNumber = Convert.ToInt32(Console.ReadLine());
                if (playersNumber > 10)
                {
                    Console.WriteLine("Введите меньшее количество игроков");
                }
                else
                {
                    return playersNumber;
                }
            }
        }
        public static void GameProcess(List<Card> deck, List<Player> playersList)
        {
            int playersInGame = playersList.Count;
            int numberOfPlayers = playersList.Count;
            while (playersInGame >= 1)
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Player actualPlayer = playersList[i];
                    //пропуск выбывших/выигравших игроков
                    if (!(actualPlayer.IsPlayerInGame))
                    {
                        continue;
                    }
                    Console.WriteLine($"\n{actualPlayer.Name}, ваш текущий счет = {actualPlayer.Score}");
                    string requestMessageTemplate = $", хотите ли вы вытянуть еще одну карту?";
                    //отказ от продолжения игры
                    bool isNewCardNeeded = GetUserChoice($"{actualPlayer.Name}{requestMessageTemplate}");
                    if (!isNewCardNeeded)
                    {
                        Console.WriteLine($"{actualPlayer.Name}, игра завершена, ваш итоговый счет - {actualPlayer.Score}");
                        actualPlayer.IsPlayerInGame = false;
                        playersInGame = playersInGame - 1;
                        continue;
                    }
                    DrawCards(deck, playersList, i);
                    if (actualPlayer.Score > 21)
                    {
                        Console.WriteLine($"{actualPlayer.Name}, вы выбыли из игры, ваш итоговый счет - {actualPlayer.Score}");
                        actualPlayer.Score = 0;
                        actualPlayer.IsPlayerInGame = false;
                        playersInGame = playersInGame - 1;
                        continue;
                    }
                    else if (actualPlayer.Score == 21)
                    {
                        Console.WriteLine($"{actualPlayer.Name}, вы победили, набрав 21 очко (молодец нахуй)");
                        playersInGame = playersInGame - 1;
                        actualPlayer.IsPlayerInGame = false;
                        continue;
                    }
                }
            }
        }

        //Ввод имени пользователя
        public static string GetUserName(int currentPlayerIndex)
        {
            string pattern = @"^([A-Z][a-z]*([ -][A-Z][a-z]*)*)$";
            Regex rgx = new Regex(pattern);
            while (true)
            {
                Console.WriteLine($"Введите имя {currentPlayerIndex + 1}-ого игрока");
                string name = Console.ReadLine();
                if (rgx.IsMatch(name))
                {
                    return name;
                }
                else
                {
                    Console.WriteLine("Имя пользователя не может содержать цифры, " +
                        "символы (кроме дефиса) а также должно начинаться с большой буквы");
                }
            }
        }
    }
}