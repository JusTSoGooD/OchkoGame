using System.Xml;

namespace MyApp
{
    public class OchkoGame
    {
        static string path = Directory.GetCurrentDirectory() + @"\\Players.xml";

        public static void Main(string[] args)
        {
            List<Card> deck = CardManager.GenerateDeck();
            OchkoPlaying(deck);
            Console.ReadKey();
        }

        public static void CreateXmlFile()
        {
            XmlDocument xNewPlayersDocument = new XmlDocument();
            XmlDeclaration xmlDec = xNewPlayersDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xNewPlayersDocument.AppendChild(xmlDec);
            XmlElement playersDatabase = xNewPlayersDocument.CreateElement("Database");
            playersDatabase.SetAttribute("name", "Игроки");
            xNewPlayersDocument.AppendChild(playersDatabase);
            xNewPlayersDocument.Save(path);
        }

        //Игровой модуль
        public static void OchkoPlaying(List<Card> deck)
        {
            List<Player> players = new List<Player>();

            if (!File.Exists(path))
            {
                CreateXmlFile();
            }

            XmlDocument xPlayersDocument = new XmlDocument();
            xPlayersDocument.Load(path);
            int numberOfPlayers = ConsoleIOManager.GetNumberOfPlayers();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player(ConsoleIOManager.GetUserName(i)));
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
                players[i].DrawCard(deck);
            }
            GameProcess(deck, players);
            DetermineWinners(players);
        }

        public static void OchkoPlayingWithoutXml(List<Card> deck)
        {
            List<Player> players = new List<Player>();
            int numberOfPlayers = ConsoleIOManager.GetNumberOfPlayers();

            for (int i = 0; i <= numberOfPlayers; i++)
            {
                players.Add(new Player(ConsoleIOManager.GetUserName(i)));
            }

            players.ForEach(player => player.DrawCard(deck));
            GameProcess(deck, players);
            DetermineWinners(players);
        }

        //вывод победителей игры
        public static void DetermineWinners(List<Player> playersList)
        {
            List<Player> winners = GetPlayersWithHighestScore(playersList);
            ConsoleIOManager.PrintWinners(winners);
            PrintWinnersToXml(winners);
        }

        public static void PrintWinnersToXml(List<Player> winners)
        {
            XmlDocument xPlayersDocument = new XmlDocument();
            xPlayersDocument.Load(path);
            XmlElement? xRoot = xPlayersDocument.DocumentElement;

            foreach (Player winner in winners)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    if (attr.Value == winner.Name)
                    {
                        XmlNode? winnerCount = xnode.Attributes.GetNamedItem("winCount");
                        winnerCount.Value = Convert.ToString(Convert.ToInt16(winnerCount.Value) + 1);
                        xPlayersDocument.Save(path);
                    }
                }
            }
        }

        public static List<Player> GetPlayersWithHighestScore(List<Player> playersList)
        {
            int maxScore = playersList.Max(p => p.Score);
            return playersList.FindAll(p => p.Score == maxScore);
        }

        // TODO: Пересмотри внимательно еще раз этот код. Его можно улучшить.
        public static void GameProcess(List<Card> deck, List<Player> playersList)
        {
            int playersInGame = playersList.Count;
            while (playersInGame >= 1)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    Player actualPlayer = playersList[i];
                    //пропуск выбывших/выигравших игроков
                    if (!(actualPlayer.IsPlayerInGame))
                    {
                        continue;
                    }

                    Console.WriteLine($"\n{actualPlayer.Name}, ваш текущий счет = {actualPlayer.Score}");
                    //отказ от продолжения игры
                    bool isNewCardNeeded = ConsoleIOManager.GetUserChoice($"{actualPlayer.Name}, хотите ли вы вытянуть еще одну карту?");
                    if (!isNewCardNeeded)
                    {
                        Console.WriteLine($"{actualPlayer.Name}, игра завершена, ваш итоговый счет - {actualPlayer.Score}");
                        actualPlayer.IsPlayerInGame = false;
                        playersInGame--;
                        continue;
                    }

                    playersList[i].DrawCard(deck);

                    if (actualPlayer.Score < 21)
                    {
                        continue;
                    }

                    if (actualPlayer.Score == 21)
                    {
                        Console.WriteLine($"{actualPlayer.Name}, вы победили, набрав 21 очко (молодец нахуй)");
                    }
                    else
                    {
                        actualPlayer.Score = 0;
                        Console.WriteLine($"{actualPlayer.Name}, вы выбыли из игры, ваш итоговый счет - {actualPlayer.Score}");
                    }

                    actualPlayer.IsPlayerInGame = false;
                    playersInGame--;
                }
            }
        }
    }
}