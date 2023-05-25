using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.Command;

public class HelpCommand : IBotCommand
{
    public void Execute(Display display)
    {
        var answer = new Answer();
        answer.Text = HarvestedMessages.HelpCommandMessages.GetText();
        answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("В главное меню", "/menu")
        });
        display.UpdateMainMessage(answer);
    }
}