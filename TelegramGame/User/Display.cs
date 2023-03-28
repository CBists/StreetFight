using System.Text;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Data;
using TelegramGame.Game;
using TelegramGame.Game.Entity;

namespace TelegramGame.User;

public class Display
{
    private static BattleCore _battleCore = new BattleCore();
    private int _messageId;
    private bool _isFirstMessage = true;
    private readonly ITelegramBot _bot;
    private readonly DataBase _db;
    
    private long ChatId { get; }
    public UserData User { get; }
    public bool IsNewPlayer = true;
    public Stage Stage = Stage.NONE;
    public BattlePackage Package = new BattlePackage();
    public GameUser? GameUser { get; private set; }
    public Display(long chatId, ITelegramBot bot, DataBase db)
    {
        ChatId = chatId;
        _bot = bot;
        _db = db;
        var user = db.GetUserById(chatId);
        if (user is { })
        {
            IsNewPlayer = false;
            _isFirstMessage = false;
            _messageId = user.MessageId;
            var inv = JsonSerializer.Deserialize<List<int>>(new MemoryStream(Encoding.UTF8.GetBytes(user.Inventory)));
            User = new UserData()
            {
                Agility = user.Agility,
                Inventory = inv ?? new List<int>(),
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
        if (_isFirstMessage)
        {
            _messageId = _bot.SendMessage(ChatId, answer).Result.MessageId;
            _isFirstMessage = false;
            UpdateDataInDb();
        }
        else
        {
            try
            {
                _bot.EditMessage(ChatId, _messageId, answer);
            }
            catch (Exception ex)
            {
                if (ex.Message == "One or more errors occurred. (Bad Request: message to edit not found)")
                    _messageId = _bot.SendMessage(ChatId, answer).Result.MessageId;
                UpdateDataInDb();
            }
        }
    }

    public void RegisterUser(string name)
    {
        User.Name = name;
        User.Inventory = new List<int>();
        User.Agility = 20;
        User.Money = 100;
        User.Strange = 30;
        UpdateDataInDb();
        IsNewPlayer = false;
    }
    
    private void UpdateDataInDb()
    {
        var user = new Data.Entity.UserEntity()
        {
            ChatId = ChatId,
            MessageId = _messageId,
            Name = User.Name,
            Agility = User.Agility,
            Inventory = JsonSerializer.Serialize(User.Inventory),
            Money = User.Money,
            Strange = User.Strange
        };
        _db.UpdateUser(user);
    }

    public void UpdateFightInfo()
    {
        if(GameUser is not {})
            return;
        var enemy = GameUser.GetEnemy();
        StringBuilder text = new("");
        text.AppendLine($"Ваше здоровье: {GameUser.Hp}");
        text.AppendLine($"Здоровье противника: {enemy.Hp}");
        if (GameUser.Hp <= 0 && enemy.Hp <= 0)
            text.AppendLine("\nНичья!");
        else if (GameUser.Hp <= 0)
            text.AppendLine("\nПоражение!");
        else if (enemy.Hp <= 0)
            text.AppendLine("\nПобеда!");
        var answer = new Answer();
        if(GameUser.Hp <= 0 || enemy.Hp <= 0)
        {
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Главное меню", "/menu")
            });
            GameUser = null;
        }
        else
        {
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Голова", "battle 1"),
                InlineKeyboardButton.WithCallbackData("Тело", "battle 2"),
                InlineKeyboardButton.WithCallbackData("Ноги", "battle 3")
            });
        }
        answer.Text = text.ToString();
        UpdateMainMessage(answer);
    }

    public void StartFightWithBot()
    {
        GameUser = _battleCore.RegisterOnFightWithBot(this);
        UpdateFightInfo();
    }

    public void SetupPackage(int action)
    {
        if (Package.Attack == BodyParts.None)
            Package.Attack = (BodyParts)action;
        else
        {
            Package.Def = (BodyParts)action;
            GameUser.SendPackage();
            Package = new BattlePackage();
        }
    }
}