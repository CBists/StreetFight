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
    public UserData User { get; }
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
            isFirstMessage = false;
            messageId = user.MessageId;
            var inv = JsonSerializer.Deserialize<List<int>>(new MemoryStream(Encoding.UTF8.GetBytes(user.Inventory)));
            User = new UserData()
            {
                Agility = user.Agility,
                Inventory = inv is { } ? inv : new List<int>(),
                Money = user.Money,
                Name = user.Name,
                Strange = user.Strange
            };
        }
        else
            User = new UserData();
    }

    public void UpdateMainMessage(Answer answer)
    {
        if (isFirstMessage)
        {
            messageId = _bot.SendMessage(ChatId, answer).Result.MessageId;
            isFirstMessage = false;
            UpdateDataInDb();
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
                UpdateDataInDb();
            }
        }
    }

    public void RegisterUser(string name)
    {
        User.Name = name;
        User.Inventory = new List<int>();
        User.Agility = 3;
        User.Money = 100;
        User.Strange = 3;
        UpdateDataInDb();
        IsNewPlayer = false;
    }

    private void UpdateDataInDb()
    {
        var user = new Data.Entity.UserEntity()
        {
            ChatId = ChatId,
            MessageId = messageId,
            Name = User.Name,
            Agility = User.Agility,
            Inventory = JsonSerializer.Serialize(User.Inventory),
            Money = User.Money,
            Strange = User.Strange
        };
        _db.UpdateUser(user);
    }
}