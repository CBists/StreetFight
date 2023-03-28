using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.TextHandler;

public class NameEnterHandler : ITextHandler
{
    public void HandleTextData(Display display, string text)
    {
        var answer = new Answer();
        if (text.All(letter => char.IsLetterOrDigit(letter) || letter == '_'))
        {
            display.RegisterUser(text);
            display.Stage = Stage.NONE;
            answer.Text = HarvestedMessages.StartCommandMessages.GetMessageText(display.User.Name);
            var buttonInText = HarvestedMessages.StartCommandMessages.GetMessageButton().Split(":");
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(buttonInText[0], buttonInText[1])
            });
        }
        else
            answer.Text = "Неверный формат ника";

        display.UpdateMainMessage(answer);
    }
}