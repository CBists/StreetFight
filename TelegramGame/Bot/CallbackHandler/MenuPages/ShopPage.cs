using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Extension;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.MenuPages;

public class ShopPage : IMenuPage
{
    public Answer Get(Display display, string data)
    {
        var answer = new Answer();
        answer.Text = "Добро пожаловать в магазин!";
        answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
        {
            InlineKeyboardButton.WithCallbackData("Улучшить силу (50 золота)", "shop strange").ListOf(),
            InlineKeyboardButton.WithCallbackData("Улучшить ловкость (100 золота)", "shop agility").ListOf(),
            InlineKeyboardButton.WithCallbackData("Назад", "/menu").ListOf(),
        });
        return answer;
    }
}