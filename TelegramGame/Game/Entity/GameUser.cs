using TelegramGame.User;

namespace TelegramGame.Game.Entity;

public class GameUser : IGamePlayer
{
    private readonly Display _display;
    public int Hp { get; set; }
    public int Power { get; set; }
    public int Rank => (Hp / 3) + Power;
    public BattlePackage Package => _display.Package;
    public event Action<IGamePlayer> SetPackageEvent; 
    public event Func<IGamePlayer, IGamePlayer> GetEnemyEvent;

    public GameUser(Display display)
    {
        Hp = display.User.Strange * 3;
        Power = display.User.Agility;
        _display = display;
    }

    public void SendResult(RoundResult result)
    {
        _display.UpdateFightInfo();
    }

    public void SendPackage()
    {
        SetPackageEvent(this);
    }
    public IGamePlayer GetEnemy()
    {
        return GetEnemyEvent(this);
    }

    public void ResetPackage()
    {
        _display.Package = new BattlePackage();
    }
}