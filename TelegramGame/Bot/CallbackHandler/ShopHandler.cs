using TelegramGame.Bot.CallbackHandler.MenuPages;
using TelegramGame.Bot.CallbackHandler.ShopItems;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler;

public class ShopHandler : ICallbackHandler
{
    private ShopPage _shopPage = new ShopPage();
    private Dictionary<string, IShopItem> _items = new Dictionary<string, IShopItem>()
    {
        { "strange", new Strange() },
        { "agility", new Agility() }
    };
    public void HandleCallback(Display display, string data)
    {
        var answer = _shopPage.Get(display, data);
        if (_items.TryGetValue(data.Split()[1], out var item))
        {
            if (item.Buy(display))
                answer.Text += "\n\nПокупка успешна";
            else
                answer.Text += "\n\nПокупка не завершена, нехватило золота";
        }
        display.UpdateMainMessage(answer);
    }
}