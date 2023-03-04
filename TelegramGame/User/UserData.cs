namespace TelegramGame.User;

public class UserData
{
    public String? Name { get; set; }
    public int Strange { get; set; }
    public int Agility { get; set; }
    public int Money { get; set; }
    public List<int> Inventory { get; set; } = new();
}