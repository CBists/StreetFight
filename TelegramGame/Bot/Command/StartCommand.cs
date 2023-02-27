using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.Command;

public class StartCommand : IBotCommand
{
    public void Execute(long chatId, Display display)
    {
        var answer = new Answer();
        answer.Text = "Приветствую в бойцовском клубе, введите имя вашего героя:";
        display.UpdateMainMessage(answer);
    }
}