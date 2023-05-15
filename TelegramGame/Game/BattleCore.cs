using TelegramGame.Game.Entity;
using TelegramGame.User;

namespace TelegramGame.Game;

public class BattleCore
{
    private List<GameUser> _queue = new();

    public GameUser RegisterOnFightWithBot(Display display)
    {
        var player = new GameUser(display);
        var bot = new GameBot(player.Rank);
        var session = new Session(player, bot);
        player.FindEnemy();
        bot.Start();
        return player;
    }

    public GameUser RegisterOnFight(Display display)
    {
        var player = new GameUser(display);
        if (_queue.Count == 1)
        {
            var enemy = _queue[0];
            _queue.RemoveAt(0);
            var session = new Session(player, enemy);
            enemy.FindEnemy();
            player.FindEnemy();
        }
        else
            _queue.Add(player);
        return player;
    }

    public void LeaveFromQueue(Display display)
    {
        _queue = new List<GameUser>();
    }
}