using Telegram.Bot.Types;
using TelegramGame.Bot.CallbackHandler;
using TelegramGame.Bot.Command;
using TelegramGame.Bot.Core;
using TelegramGame.Bot.TextHandler;
using TelegramGame.Data;
using TelegramGame.User;

namespace TelegramGame.Bot.UserInteraction;

public class UpdateProcessor
{
    private readonly Dictionary<string, IBotCommand> _commands;
    private readonly Dictionary<string, ICallbackHandler> _callbackHandlers;
    private readonly Dictionary<Stage, ITextHandler> _textHandlers;
    private readonly ITelegramBot _bot;
    private readonly Dictionary<long, Display> _displays = new();
    private readonly DataBase _db;

    public UpdateProcessor(ITelegramBot bot)
    {
        _bot = bot;
        _commands = new()
        {
            { "/start", new StartCommand() },
            { "/menu", new MenuCommand() },
        };
        _textHandlers = new()
        {
            { Stage.ENTER_NAME, new NameEnterHandler() }
        };
        _callbackHandlers = new()
        {
            { "menu", new MenuHandler() },
            { "battle", new BattleHandler() },
        };
        _db = new DataBase();
    }

    public void Process(long chatId, Update update)
    {
        if (!_displays.Keys.Contains(chatId))
        {
            _displays[chatId] = new Display(chatId, _bot, _db);
            _commands["/start"].Execute(_displays[chatId]);
            return;
        }
        string data = "";
        string callbackData = "";
        if (update.Message is { })
            _bot.DeleteMessage(chatId, update.Message.MessageId);
        if (update.Message?.Text is { })
            data = update.Message.Text;
        if (update.CallbackQuery?.Data is { })
            callbackData = update.CallbackQuery.Data;
        HandleText(chatId, data);
        HandleCommand(chatId, data);
        HandleCallback(chatId, callbackData);
        HandleCommand(chatId, callbackData);
    }

    public void HandleCommand(long chatId, string command)
    {
        var prefix = command.ToLower().Split()[0];
        if (_commands.Keys.Contains(prefix))
            _commands[prefix].Execute(_displays[chatId]);
    }

    public void HandleCallback(long chatId, string data)
    {
        var prefix = data.ToLower().Split()[0];
        if (_callbackHandlers.Keys.Contains(prefix))
            _callbackHandlers[prefix].HandleCallback(_displays[chatId], data);
    } 
    public void HandleText(long chatId, string text)
    {
        if (_displays.Keys.Contains(chatId) && _textHandlers.Keys.Contains(_displays[chatId].Stage))
            _textHandlers[_displays[chatId].Stage].HandleTextData(_displays[chatId], text);
    }
}