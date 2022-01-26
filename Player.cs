using System;

public class Player
{
    public Player(string name)
    {
        Name = name;
        Score = 0;
        IsPlayerInGame = true;
    }

    public string Name { get; set; }

    public int Score { get; set; }

    public bool IsPlayerInGame { get; set; }

    public void DrawCard(List<Card> deck)
    {
        Card newCard = CardManager.GetCard(deck);
        Console.WriteLine($"{Name}, ваша карта {newCard.Rank} {newCard.Suit}");
        Score = Score + newCard.Value;
    }
}
