namespace TelegramGame.Bot.CallbackHandler;

public interface ICallbackHandler
{
    void HandleCallback(long chatId, string data);
}