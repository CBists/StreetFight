using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.ShopItems;

public interface IShopItem
{
    bool Buy(Display display);
}