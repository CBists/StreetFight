using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.CallbackHandler;

public class BattleHandler: ICallbackHandler
{
    public void HandleCallback(Display display, string data)
    {
        var splitedData = data.Split();
        if (splitedData[1] == "bot")
            display.StartFightWithBot();
        else if (splitedData[1] == "player")
            display.StartFightWithPlayer();
        else if(splitedData[1] == "leave")
            display.LeaveFromQueue();
        else
            display.SetupPackage(Int32.Parse(splitedData[1]));
    }
}