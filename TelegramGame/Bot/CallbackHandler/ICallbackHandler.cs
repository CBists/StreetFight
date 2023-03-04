using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler;

public interface ICallbackHandler
{
    void HandleCallback(Display display, string data);
}