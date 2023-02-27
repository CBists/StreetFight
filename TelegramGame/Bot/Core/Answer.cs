using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramGame.Bot.Core;

public class Answer
{
    public string Text = "";
    public string? PhotoPath = null;
    public InlineKeyboardMarkup  ReplyKeyboardMarkup = new(Array.Empty<InlineKeyboardButton>());
    public bool HasPhoto => PhotoPath is { };
}