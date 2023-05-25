using System.Text;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramGame.Bot.Core;
using TelegramGame.Data;
using TelegramGame.Extension;
using TelegramGame.Game;
using TelegramGame.Game.Entity;

namespace TelegramGame.User;

public class Display
{
    private static BattleCore battleCore = new BattleCore();
    private int _messageId;
    private bool _isFirstMessage = true;
    private readonly ITelegramBot _bot;
    private readonly DataBase _db;
    private bool _isAttack = true;
    private bool _isFoundEnemy = false;
    private bool _isEnemyPacketReady = false;
    private bool _isPacketReady = false;
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

    public void UpdateDataInDb()
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
        if (GameUser is null)
            return;
        var enemy = GameUser.GetEnemy();
        var answer = new Answer();
        if (GameUser.Hp <= 0 || enemy.Hp <= 0)
        {
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Главное меню", "/menu")
            });
        }
        else
        {
            var action = _isAttack ? "Атака" : "Защита";
            answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                InlineKeyboardButton.WithCallbackData($"{action} Голова", "battle 1").ListOf(),
                InlineKeyboardButton.WithCallbackData($"{action} Тело", "battle 2").ListOf(),
                InlineKeyboardButton.WithCallbackData($"{action} Ноги", "battle 3").ListOf()
            });
        }

        answer.Text = GetTextMessageFight();
        UpdateMainMessage(answer);
    }

    public void StartFightWithPlayer()
    {
        _isFoundEnemy = false;
        GameUser = battleCore.RegisterOnFight(this);
        if (_isFoundEnemy)
            UpdateFightInfo();
        else
            MessageSearchingEnemy();
    }
    public void StartFightWithBot()
    {
        GameUser = battleCore.RegisterOnFightWithBot(this);
        UpdateFightInfo();
    }
    public void SetupPackage(int action)
    {
        if (GameUser is null) return;
        if (_isAttack)
        {
            Package.Attack = (BodyParts)action;
            Package.Ready = false;
            _isAttack = false;
            UpdateFightInfo();
        }
        else
        {
            Package.Def = (BodyParts)action;
            Package.Ready = true;
            _isAttack = true;
            _isPacketReady = true;
            MessageWaitingEnemy();
            GameUser.SendPackage();
        }
    }

    private string GetTextMessageFight()
    {
        var enemy = GameUser.GetEnemy();
        StringBuilder text = new($"{GameUser.Name} vs {enemy.Name}");
        if (Package.Ready && enemy.Package.Ready)
        {
            text.AppendLine("");
            text.AppendLine("");
            text.AppendLine($"Вы ударили {Package.Attack}");
            text.AppendLine($"Противник защитил {enemy.Package.Def}");
            text.AppendLine($"Вы защитили {Package.Def}");
            text.AppendLine($"Противник ударил {enemy.Package.Attack}");
        }
        text.AppendLine("");
        text.AppendLine($"Ваше здоровье: {GameUser.Hp}");
        text.AppendLine($"Здоровье противника: {enemy.Hp}");
        if (GameUser.Hp <= 0 && enemy.Hp <= 0)
            text.AppendLine("\nНичья!");
        else if (GameUser.Hp <= 0)
        {
            text.AppendLine("\nПоражение!");
            text.AppendLine("Результат: -5 золота");
            User.Money = Math.Max(User.Money - 5, 0);
            UpdateDataInDb();
        }
        else if (enemy.Hp <= 0)
        {
            text.AppendLine("\nПобеда!");
            text.AppendLine("Результат: +5 золота");
            User.Money += 5;
            UpdateDataInDb();
        }
        return text.ToString();
    }

    private void MessageSearchingEnemy()
    {
        var text = "Ожидание соперника";
        var answer = new Answer();
        answer.ReplyKeyboardMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
        {
            InlineKeyboardButton.WithCallbackData($"Отменить поиск", "battle leave").ListOf(),
        });
        answer.Text = text;
        UpdateMainMessage(answer);
    }

    private void MessageWaitingEnemy()
    {
        if (_isPacketReady)
        {
            if (_isEnemyPacketReady)
            {
                _isPacketReady = false;
                _isEnemyPacketReady = false;
            }
            else
            {
                var answer = new Answer();
                answer.Text = "Ожидание хода соперника";
                UpdateMainMessage(answer);
            }
        }
    }
    public void FindEnemy()
    {
        _isFoundEnemy = true;
        UpdateFightInfo();
    }

    public void ChangeEnemyStatus()
    {
        _isEnemyPacketReady = true;
        MessageWaitingEnemy();
    }

    public void LeaveFromQueue() => battleCore.LeaveFromQueue(this);
}