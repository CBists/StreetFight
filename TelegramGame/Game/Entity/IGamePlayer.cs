namespace TelegramGame.Game.Entity;

public interface IGamePlayer
{
    int Hp { get; set; }
    int Power { get; }
    public string Name { get; set; }
    int Rank { get; }
    BattlePackage Package { get; }
    public event Action<IGamePlayer> SetPackageEvent;
    public event Func<IGamePlayer, IGamePlayer> GetEnemyEvent;
    void SendResult();
    void EnemyConfirmPackage();
}