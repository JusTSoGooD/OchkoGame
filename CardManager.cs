using System;

public class CardManager
{
    // TODO: Добавь возможность генерации колоды из 36 карт
    //метод для процесса формирования мастей карт
    public static List<Card> GenerateDeck(bool isDeckLong)
    {
        List<Card> deck = new List<Card>();
        AddCardsToDeckBySuit(deck, "червей", isDeckLong);
        AddCardsToDeckBySuit(deck, "бубей", isDeckLong);
        AddCardsToDeckBySuit(deck, "пик", isDeckLong);
        AddCardsToDeckBySuit(deck, "крести", isDeckLong);
        return deck;
    }

    //Метод для процесса вытягивания карт
    public static Card GetCard(List<Card> deck)
    {
        int randomDeckCardNumber = new Random().Next(deck.Count);
        Card newCard = deck[randomDeckCardNumber];
        deck.RemoveAt(randomDeckCardNumber);
        return newCard;
    }

    //Метод для создания колоды (задает численное значение и номинал карты)
    private static void AddCardsToDeckBySuit(List<Card> deck, string cardSuit, bool isDeckLong)
    {
        if (isDeckLong)
        {
            deck.Add(new Card("двойка", cardSuit, 2));
            deck.Add(new Card("тройка", cardSuit, 3));
            deck.Add(new Card("четверка", cardSuit, 4));
            deck.Add(new Card("пятерка", cardSuit, 5));
        }
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
}
