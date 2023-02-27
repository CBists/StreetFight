using System.ComponentModel.Design;
using Telegram.Bot.Types;
using TelegramGame.Bot.CallbackHandler;
using TelegramGame.Bot.Command;
using TelegramGame.Bot.Core;
using TelegramGame.User;

namespace TelegramGame.Bot.UserInteraction;

public class UpdateProcessor
{
    private readonly Dictionary<string, IBotCommand> _commands;
    private readonly Dictionary<string, ICallbackHandler> _callbackHandlers = new();
    private ITelegramBot bot;
    private readonly Dictionary<long, Display> _displays = new();

    public UpdateProcessor(ITelegramBot bot)
    {
        this.bot = bot;
        _commands = new() { { "/start", new StartCommand() } };
    }

    public void ProcessCommands(long chatId, Update update)
    {
        if (!_displays.Keys.Contains(chatId))
            _displays[chatId] = new Display(chatId, bot);
        string data = "";
        if (update.Message?.Text is { })
        {
            data = update.Message.Text;
            bot.DeleteMessage(chatId, update.Message.MessageId);
        }

        if (update.CallbackQuery?.Message?.Text is { })
            data = update.CallbackQuery.Message.Text;
        HandleCommand(chatId, data);
        HandleCallback(chatId, data);
        HandleText(chatId, data);
    }

    public void HandleCommand(long chatId, string command)
    {
        var prefix = command.ToLower().Split()[0];
        if (_commands.Keys.Contains(prefix))
            _commands[prefix].Execute(chatId, _displays[chatId]);
    }

    public void HandleCallback(long chatId, string data)
    {
        var prefix = data.ToLower().Split()[0];
        if (_callbackHandlers.Keys.Contains(prefix))
            _callbackHandlers[prefix].HandleCallback(chatId, data);
    }

    public void HandleText(long chatId, string text)
    {
    }
}