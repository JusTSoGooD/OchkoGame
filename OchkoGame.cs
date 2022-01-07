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


        // TODO: Добавь возможность генерации колоды из 36 карт
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


        // TODO: Некорректные аргументы метода.
        // TODO: Передавать в метод счетчик цикла - это не очень хорошо, лучше такого избегать. Даже если это необходимо (такие случаи бывают, но это явно не он), то уж точно плохо называть его 'CycleCounter'.
        // TODO: В контексте метода это название не несет никакой полезной смысловой нагрузки. Напротив, только запутывает и усложняет код, ибо заставляет задумываться о том, в каком вообще цикле гоняется этот метод, за что отвечает этот счетчик, как его использовать и т.п.
        // TODO: Метод должен быть максимально самостоятельным и независимым блоком кода. Если и передавать в метод счетчик, то как вариант названия 'playerIndex'.
        // TODO: В данном случае будет более грамотным решением передавать в метод исключительного необходимого для работы пользователя. Тебе здесь не нужен весь лист и еще и индекс.

        // TODO: Имя метода 'DrawCards'. Почему во множественном числе?
        // TODO: Так же для улучшения читаемости лучше сразу записать вытянутую карту в переменную, и потом использовать именно ее, а не каждый раз писать "deck[randomDeckCardNumber]".

        // TODO: Этот метод перегружен функциональностью: 1.[Взятие карты] 2.[Вывод данных на экран] 3.[Изменение счета пользователя].
        // TODO: Тут явно нарушается принцип единой ответственности (да, он в оригинале относится к объектам, но и при проектировании методов им стоит руководствоваться). Если по-простому "один метод - одно действие". Не стоит пихать все и сразу.
        // TODO: Грамотнее было бы в этом методе оставить исключительно взятие новой карты и возвращать эту карту. Всё. Остальное куда-то в другое место, ему здесь не место.

        // TODO: Ну и на десерт: имена локальных переменных пишутся с маленькой буквы :3

        // TODO: Крч с этим методо не так примерно все, я бы даже сказал, это такой своеобразный антипаттерн, как не нужно писать методы :D 
        // TODO: Но это ок. Это вопрос опыта, который ты как раз сейчас крайне активно и быстро нарабатываешь)

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
            // TODO: Некорректный комментарий: данном блоке кода не водятся пользователи в игру. Тут только создается лист для них.
            // TODO: Кстати, ввод пользователей в игру стоит вынести в отдельный метод.
            //ввод пользователей в игру
            List<Player> players = new List<Player>();

            // TODO: Сделать пусть относительным, чтобы не приходилось каждый раз его изменять.
            string path = "C://Users/kuchm/Desktop/Программы c#/Ochko/OchkoGame/users.xml";
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
                DrawCards(deck, players, i);
            }
            GameProcess(deck, players);
            WithdrawingGameWinners(players);
        }


        // TODO: Лично моя хотелка для удобства: добавь пж возможность вводить пользователем "д/н", "yes/no", "y/n", "+/-". Аналогично в любом регистре.
        // TODO: Это просто хотелка как пользователя, можешь забить и снести этот коммент) 
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