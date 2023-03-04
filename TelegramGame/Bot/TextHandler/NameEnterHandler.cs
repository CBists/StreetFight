using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.TextHandler;

public class NameEnterHandler : ITextHandler
{
    public void HandleTextData(Display display, string text)
    {
        var answer = new Answer();
        if (CheckName(text))
        {
            display.RegisterUser(text);
            display.Stage = Stage.NONE;
            answer.Text = "Привет, " + text;
        }
        else
            answer.Text = "Неверный формат ника";
        display.UpdateMainMessage(answer);
    }

    private bool CheckName(string name)
    {
        foreach (var letter in name)
            if (!char.IsLetterOrDigit(letter) && letter != '_')
                return false;
        return true;
    }
}