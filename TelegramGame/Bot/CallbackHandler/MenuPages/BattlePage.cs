using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Extension;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.MenuPages;

public class BattlePage : IMenuPage
{
    public Answer Get(Display display, string data)
    {
        var text = new StringBuilder("");
        text.AppendLine("Варианты битв:");
        text.AppendLine("Драка с ботом: вы будете драться с ботом, который будет вам равен по силе");
        text.AppendLine(
            "Драка с другим игроком: вы будете драться с дургим реальным игроком, который подберется вам равным по силе.");
        var answer = new Answer
        {
            Text = text.ToString(),
            ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                InlineKeyboardButton.WithCallbackData("С игроком", "battle player").ListOf(),
                InlineKeyboardButton.WithCallbackData("С ботом", "battle bot").ListOf(),
                InlineKeyboardButton.WithCallbackData("Назад", "/menu").ListOf(),
            })
        };
        return answer;
    }
}