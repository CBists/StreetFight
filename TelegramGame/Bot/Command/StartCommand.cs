using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.User;
using TelegramGame.HarvestedMessages;

namespace TelegramGame.Bot.Command;

public class StartCommand : IBotCommand
{
    public void Execute(Display display)
    {
        var answer = new Answer();
        if (display.IsNewPlayer)
        {
            answer.Text = "Приветствую в бойцовском клубе, введите имя вашего героя:";
            display.Stage = Stage.ENTER_NAME;
        }
        else
        {
            answer.Text = StartCommandMessages.GetMessageText(display.User.Name);
            var buttonInText = StartCommandMessages.GetMessageButton().Split(":");
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(buttonInText[0], buttonInText[1])
            });
        }
        display.UpdateMainMessage(answer);
    }
}