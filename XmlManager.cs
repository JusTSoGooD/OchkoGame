using System.Xml.Linq;

public class XmlManager
{
    private static readonly string fullPath = Directory.GetCurrentDirectory() + @"\\Players.xml";

    private const string rootElementName = "Players";
    private const string playerElementName = "Player";

    public static void CreateFile()
    {
        if (!File.Exists(fullPath))
        {
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement rootElement = new XElement(rootElementName);
            XDocument xDoc = new XDocument(declaration, rootElement);
            xDoc.Save(fullPath);
        }        
    }

    public static void addPlayersToXml(List<Player> players)
    {
        XDocument? xDoc = XDocument.Load(fullPath);
        XElement? xPlayersElement = xDoc.Descendants(rootElementName).First();

        foreach (Player player in players)
        {
            if (isPlayerExists(xPlayersElement, player))
            {
                increasePlayerStats(xPlayersElement, player, "Games");
            }
            else
            {
                createPlayer(xPlayersElement, player);
            }
        }

        xDoc.Save(fullPath);
    }

    public static void PrintWinners(List<Player> winners)
    {
        XDocument? xDoc = XDocument.Load(fullPath);
        XElement? xPlayersElement = xDoc.Descendants(rootElementName).First();

        winners.ForEach(winner => increasePlayerStats(xPlayersElement, winner, "Wins"));

        xDoc.Save(fullPath);
    }

    private static void increasePlayerStats(XElement xPlayersElement, Player player, string fieldName)
    {
        XElement xPlayerElement = xPlayersElement.Elements(playerElementName)
            .FirstOrDefault(p => p.Attribute("Name").Value == player.Name);

        int fieldOldValue = int.Parse(xPlayerElement.Element(fieldName).Value);

        xPlayerElement.SetElementValue(fieldName, fieldOldValue + 1);
    }

    private static bool isPlayerExists(XElement xPlayersElement, Player player)
    {
        return xPlayersElement.Descendants(playerElementName)
            .Where(p => p.Attribute("Name").Value == player.Name).Any();
    }

    private static void createPlayer(XElement xPlayersElement, Player player)
    {
        XElement xPlayerElement = new XElement(playerElementName, new XAttribute("Name", player.Name));
        XElement xPlayerGamesElement = new XElement("Games", 1);
        XElement xPlayerWinsElement = new XElement("Wins", 0);

        xPlayerElement.Add(xPlayerGamesElement, xPlayerWinsElement);
        xPlayersElement.Add(xPlayerElement);
    }
}
