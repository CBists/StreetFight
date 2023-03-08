using System.Net.Sockets;
using TelegramGame.Data.Entity;

namespace TelegramGame.Data;

public class DataBase
{
    private readonly MainContext _db;

    public DataBase()
    {
        _db = new MainContext();
    }

    public void UpdateUser(UserEntity userEntity)
    {
        var user = _db.Users.FirstOrDefault(o => o.ChatId == userEntity.ChatId);
        if (user is { })
            SetNewValue(user, userEntity);
        else
            _db.Users.Add(userEntity);
        Save();
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public UserEntity? GetUserById(long chatId)
    {
        return _db.Users.FirstOrDefault(o => o.ChatId == chatId);
    }

    private void SetNewValue(UserEntity old, UserEntity newValue)
    {
        old.Name = newValue.Name;
        old.Strange = newValue.Strange;
        old.Agility = newValue.Agility;
        old.Money = newValue.Money;
        old.MessageId = newValue.MessageId;
        old.Inventory = newValue.Inventory;
    }
}