using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Extension;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.MenuPages;

public class PersonMenuPage : IMenuPage
{
    public Answer Get(Display display, string data)
    {
        var text = new StringBuilder("");
        text.AppendLine($"{display.User.Name}:");
        text.AppendLine($"Сила: {display.User.Strange}");
        text.AppendLine($"Ловкость: {display.User.Agility}");
        text.AppendLine($"Денег: {display.User.Money}");

        var answer = new Answer
        {
            Text = text.ToString(),
            ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                InlineKeyboardButton.WithCallbackData("Назад", "/menu").ListOf(),
                InlineKeyboardButton.WithCallbackData("Инвентарь", "menu inventory").ListOf()
            })
        };
        return answer;
    }
}