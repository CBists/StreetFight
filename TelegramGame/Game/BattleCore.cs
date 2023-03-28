using TelegramGame.Game.Entity;
using TelegramGame.User;

namespace TelegramGame.Game;

public class BattleCore
{
    private List<IGamePlayer> _queue = new();

    public GameUser RegisterOnFightWithBot(Display display)
    {
        var player = new GameUser(display);
        var bot = new GameBot(player.Rank);
        var session = new Session(player, bot);
        player.SetPackageEvent += session.SetPlayerPackageEvent;
        bot.SetPackageEvent += session.SetPlayerPackageEvent;
        player.GetEnemyEvent += session.GetEnemyEvent;
        bot.GetEnemyEvent += session.GetEnemyEvent;
        bot.Start();
        return player;
    }
}