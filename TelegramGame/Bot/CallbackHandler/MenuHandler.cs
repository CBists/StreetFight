using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.CallbackHandler.MenuPages;
using TelegramGame.Bot.Command;
using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler;

public class MenuHandler : ICallbackHandler
{
    private Dictionary<string, IMenuPage> _pages = new()
    {
        {"person", new PersonMenuPage()}
    };
    public void HandleCallback(Display display, string data)
    {
        var splitData = data.Split();
        var answer = new Answer
        {
            Text = "В разработке",
            ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("В главное меню", "/menu")
            })
        };
        if (_pages.ContainsKey(splitData[1]))
            answer = _pages[splitData[1]].Get(display, data);
        display.UpdateMainMessage(answer);
    }
}