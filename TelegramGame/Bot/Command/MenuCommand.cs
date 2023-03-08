using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Extension;
using TelegramGame.User;

namespace TelegramGame.Bot.Command;

public class MenuCommand : IBotCommand
{
    public void Execute(Display display)
    {
        var answer = new Answer { Text = "Главное меню" };
        var buttons = new List<List<InlineKeyboardButton>>();
        foreach (var line in HarvestedMessages.MainMenuCommand.GetButtons().Split(Environment.NewLine))
        {
            var data = line.Split(":");
            buttons.Add(InlineKeyboardButton.WithCallbackData(data[0], data[1]).ListOf());
            
        }

        answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(buttons);
        display.UpdateMainMessage(answer);
    }
}