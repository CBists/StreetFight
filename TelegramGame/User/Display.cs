using System.Text;
using TelegramGame.Bot.Core;
using TelegramGame.Data;
using System.Text.Json;

namespace TelegramGame.User;

public class Display
{
    public long ChatId { get; private set; }
    private int messageId;
    private bool isFirstMessage = true;
    private readonly ITelegramBot _bot;
    private readonly UserData _user;
    private readonly DataBase _db;
    public bool IsNewPlayer = true;
    public Stage Stage = Stage.NONE;

    public Display(long chatId, ITelegramBot bot, DataBase db)
    {
        ChatId = chatId;
        _bot = bot;
        _db = db;
        var user = db.GetUserById(chatId);
        if (user is { })
        {
            IsNewPlayer = false;
            messageId = user.MessageId;
            var inv = JsonSerializer.Deserialize<List<int>>(new MemoryStream(Encoding.UTF8.GetBytes(user.Inventory)));
            _user = new UserData()
            {
                Agility = user.Agility,
                Inventory = inv is { } ? inv : new List<int>(),
                Money = user.Money,
                Name = user.Name,
                Strange = user.Strange
            };
        }
        else
            _user = new UserData();
    }

    public void UpdateMainMessage(Answer answer)
    {
        if (isFirstMessage)
        {
            messageId = _bot.SendMessage(ChatId, answer).Result.MessageId;
            isFirstMessage = false;
        }
        else
        {
            try
            {
                var a = _bot.EditMessage(ChatId, messageId, answer).Result;
            }
            catch (Exception ex)
            {
                if (ex.Message == "One or more errors occurred. (Bad Request: message to edit not found)")
                    messageId = _bot.SendMessage(ChatId, answer).Result.MessageId;
            }
        }
    }

    public void RegisterUser(string name)
    {
        _user.Name = name;
        _user.Inventory = new List<int>();
        _user.Agility = 3;
        _user.Money = 100;
        _user.Strange = 3;
        UpdateDataInDb();
        IsNewPlayer = false;
    }

    public string? GetName()
    {
        return _user.Name;
    }
    private void UpdateDataInDb()
    {
        var user = new Data.Entity.UserEntity()
        {
            ChatId = ChatId,
            MessageId = messageId,
            Name = _user.Name,
            Agility = _user.Agility,
            Inventory = JsonSerializer.Serialize(_user.Inventory),
            Money = _user.Money,
            Strange = _user.Strange
        };
        _db.UpdateUser(user);
    }
}