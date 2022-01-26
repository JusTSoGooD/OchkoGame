using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
namespace MyApp
{
    public class OchkoGame
    {
        static string path = Directory.GetCurrentDirectory();

        public static void Main(string[] args)
        {
            List<Card> deck = CardManager.GenerateDeck();
            OchkoPlaying(deck);
            Console.ReadKey();
        }   

        //Игровой модуль
        public static void OchkoPlaying(List<Card> deck)
        {
            List<Player> players = new List<Player>();
            
            if (!File.Exists(path))
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
                players[i].DrawCard(deck);
            }
            GameProcess(deck, players);
            DetermineWinners(players);
        }
         
        //получение ответа о предложении продолжить игру
        public static bool GetUserChoice(string appealToPLayer)
        {
            while (true)
            {
                Console.WriteLine(appealToPLayer);
                string userAnswer = Console.ReadLine();
                string[] possiblePositiveAnswers = { "да", "д", "yes", "y", "+" };
                string[] possibleNegativeAnswers = { "нет", "н", "no", "n", "-" };
                if (possiblePositiveAnswers.Any(a => a.Equals(userAnswer.ToLower())))
                {
                    return true;
                }
                else if (possibleNegativeAnswers.Any(a => a.Equals(userAnswer.ToLower())))
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
        public static void DetermineWinners(List<Player> playersList)
        {
            List<Player> winners = GetPlayersWithHighestScore(playersList);
            PrintWinnersToConsole(winners);
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

        public static void PrintWinnersToConsole(List<Player> winners)
        {
            Console.WriteLine("Список победителей игры:");
            winners.ForEach(winner => Console.WriteLine($"{winner.Name}, набравший {winner.Score} баллов"));
        }

        public static List<Player> GetPlayersWithHighestScore(List<Player> playersList)
        {
            int maxScore = playersList.Max(p => p.Score);
            return playersList.FindAll(p => p.Score == maxScore);
        }

        // TODO: Нет валидации пользовательского воода на символы кроме цифр.
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
                    bool isNewCardNeeded = GetUserChoice($"{actualPlayer.Name}, хотите ли вы вытянуть еще одну карту?");
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