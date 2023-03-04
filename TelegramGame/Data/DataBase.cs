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
        var users = _db.Users.Where(o => userEntity.ChatId == o.ChatId).ToList();
        if (users.Count == 0)
            _db.Add(userEntity);
        else
            users[0] = userEntity;
        _db.SaveChanges();
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public Entity.UserEntity? GetUserById(long chatId)
    {
        var users = _db.Users.Where(o => o.ChatId == chatId).ToList();
        return users.Count != 0 ? users[0] : null;
        
    }
}