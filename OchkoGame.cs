namespace MyApp
{
    public class OchkoGame
    {
        public static void Main(string[] args)
        {
            string appealToPlayer = "Хотите играть колодой из 52 карт? (при ответе нет будет сфоримрована колода 36)";
            List<Card> deck = CardManager.GenerateDeck(ConsoleIOManager.GetUserChoice(appealToPlayer));
            OchkoPlaying(deck);
            Console.ReadKey();
        }
        
        // Игровой модуль
        public static void OchkoPlaying(List<Card> deck)
        {
            XmlManager.CreateFile();

            List<Player> players = new List<Player>();
            int numberOfPlayers = ConsoleIOManager.GetNumberOfPlayers();           
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player(ConsoleIOManager.GetUserName(i)));
            }

            XmlManager.addPlayersToXml(players);
            players.ForEach(player => player.DrawCard(deck));
            GameProcess(deck, players);
            DetermineWinners(players);
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
                        Console.WriteLine($"{actualPlayer.Name}, вы выбыли из игры, ваш итоговый счет - {actualPlayer.Score}");
                        actualPlayer.Score = 0;
                       
                    }

                    actualPlayer.IsPlayerInGame = false;
                    playersInGame--;
                }
            }
        }

        //вывод победителей игры
        public static void DetermineWinners(List<Player> playersList)
        {
            List<Player> winners = GetPlayersWithHighestScore(playersList);
            ConsoleIOManager.PrintWinners(winners);
            XmlManager.PrintWinners(winners);
        }

        public static List<Player> GetPlayersWithHighestScore(List<Player> playersList)
        {
            int maxScore = playersList.Max(p => p.Score);
            return playersList.FindAll(p => p.Score == maxScore);
        }
    }
}