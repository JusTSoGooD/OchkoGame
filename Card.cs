using System;

public class Card
{
    public Card(string rank, string suit, int value)
    {
        Rank = rank;
        Suit = suit;
        Value = value;
    }

    public string Rank { get; set; }

    public string Suit { get; set; }

    public int Value { get; set; }
}
