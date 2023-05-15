using TelegramGame.User;

namespace TelegramGame.Game.Entity;

public class GameUser : IGamePlayer
{
    private readonly Display _display;
    public string Name { get; set; }
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
        Name = display.User.Name!;
    }

    public void SendResult() => _display.UpdateFightInfo();
    public void SendPackage() => SetPackageEvent(this);
    public IGamePlayer GetEnemy() => GetEnemyEvent(this);
    public void ResetPackage() => _display.Package = new BattlePackage();
    public void FindEnemy() => _display.FindEnemy();
    public void EnemyConfirmPackage() => _display.ChangeEnemyStatus();
}