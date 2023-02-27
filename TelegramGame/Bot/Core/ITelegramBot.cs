using Telegram.Bot.Types;

namespace TelegramGame.Bot.Core;

public interface ITelegramBot
{
    Task<Message> SendMessage(long chatId, Answer answer);
    Task<Message> EditMessage(long chatId, int messageId, Answer answer);
    void DeleteMessage(long chatId, int messageId);
    
}