using TelegramGame.Bot.Core;
using TelegramGame.User;

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
            answer.Text = "Привет, " + display.GetName();
        }
        display.UpdateMainMessage(answer);
    }
}