using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.ShopItems;

public class Strange : IShopItem
{
    private int cost = 50;

    public bool Buy(Display display)
    {
        if (display.User.Money >= cost)
        {
            display.User.Money -= cost;
            display.User.Strange += 1;
            display.UpdateDataInDb();
            return true;
        }

        return false;
    }
}