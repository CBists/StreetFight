namespace TelegramGame.Game.Entity;

public class GameBot : IGamePlayer
{
    public int Hp { get; set; }
    public string Name { get; set; }
    public int Power { get; set; }
    public int Rank => (Hp / 3) + Power;
    public event Action<IGamePlayer> SetPackageEvent; 
    public event Func<IGamePlayer, IGamePlayer> GetEnemyEvent;
    public BattlePackage Package { get; private set; }

    public GameBot(int rank)
    {
        var dif = new Random().Next(-2, 3);
        Hp = (rank / 2 + dif) * 3;
        Power = rank / 2 - dif;
        Name = "Bot 1";
    }

    public void Start()
    {
        Package = new BattlePackage()
        {
            Attack = (BodyParts)(new Random().Next(3) + 1),
            Def = (BodyParts)(new Random().Next(3) + 1),
            Ready = true
        };
        SetPackageEvent(this);
    }
    public void SendResult()
    {
        Package = new BattlePackage()
        {
            Attack = (BodyParts)(new Random().Next(3) + 1),
            Def = (BodyParts)(new Random().Next(3) + 1),
            Ready = true
        };
        if(Hp > 0 && GetEnemyEvent(this).Hp > 0)
            SetPackageEvent(this);
    }
    public void EnemyConfirmPackage()
    {
        
    }
}