using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.ShopItems;

public class Agility : IShopItem
{
    private int cost = 100;
    public bool Buy(Display display)
    {
        if (display.User.Money >= cost)
        {
            display.User.Money -= cost;
            display.User.Agility += 1;
            display.UpdateDataInDb();
            return true;
        }

        return false;
    }
}