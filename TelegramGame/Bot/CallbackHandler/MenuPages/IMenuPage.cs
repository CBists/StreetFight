using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler.MenuPages;

public interface IMenuPage
{
    Answer Get(Display display, string data);
}