using TelegramGame.User;

namespace TelegramGame.Bot.TextHandler;

public interface ITextHandler
{
    void HandleTextData(Display display, string text);
}