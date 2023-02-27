using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.Command;

public interface IBotCommand
{
    void Execute(long chatId, Display display);
}