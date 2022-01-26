using System.Text.RegularExpressions;

public class ConsoleIOManager
{
    #region Input
    // Ввод имени пользователя
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

    // TODO: Нет валидации пользовательского ввода на символы кроме цифр.
    // Получение количества игроков
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

    // Получение ответа положительного или отрицательного ответа
    public static bool GetUserChoice(string appealToPLayer)
    {
        while (true)
        {
            Console.WriteLine(appealToPLayer);
            string userAnswer = Console.ReadLine();
            string[] possiblePositiveAnswers = { "да",  "д", "yes", "y", "+" };
            string[] possibleNegativeAnswers = { "нет", "н", "no",  "n", "-" };
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
    #endregion

    #region Output
    public static void PrintWinners(List<Player> winners)
    {
        Console.WriteLine("Список победителей игры:");
        winners.ForEach(winner => Console.WriteLine($"{winner.Name}, набравший {winner.Score} баллов"));
    }
    #endregion
}