namespace TelegramGame.Data;

public class DataBase
{
    private readonly MainContext _bd;

    public DataBase()
    {
        _bd = new MainContext();
    }

    public void AddUser(Entity.User user)
    {
        _bd.Add(user);
        _bd.SaveChanges();
    }

    public void Save()
    {
        _bd.SaveChanges();
    }

    public Entity.User GetUserById(long chatId)
    {
        return _bd.Users.Where(o => o.ChatId == chatId).ToList()[0];
    }
}