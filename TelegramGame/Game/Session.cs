using TelegramGame.Game.Entity;

namespace TelegramGame.Game;

public record RoundResult(int playerResult, int enemyResult);

public class Session
{
    private bool _player1PackageReady = false;
    private IGamePlayer? _player1;
    private bool _player2PackageReady = false;
    private IGamePlayer? _player2;

    public Session(IGamePlayer player1, IGamePlayer player2)
    {
        _player1 = player1;
        _player2 = player2;
    }

    public void SetPlayerPackageEvent(IGamePlayer player)
    {
        if (_player1 == player)
            _player1PackageReady = true;
        else
            _player2PackageReady = true;
        if (_player1PackageReady && _player2PackageReady)
            CalculateResult();
    }

    public IGamePlayer GetEnemyEvent(IGamePlayer player)
    {
        return player.Equals(_player1) ? _player2 : _player1;
    }
    private void CalculateResult()
    {
        if (_player1 is not { } || _player2 is not { })
            return;
        if (_player1.Package.Attack != _player2.Package.Def)
            _player2.Hp -= _player1.Power;
        if (_player1.Package.Def != _player2.Package.Attack)
            _player1.Hp -= _player2.Power;
        _player2PackageReady = false;
        _player1PackageReady = false;
        _player1.SendResult(new RoundResult(_player1.Hp, _player2.Hp));
        _player2.SendResult(new RoundResult(_player2.Hp, _player1.Hp)); // ПЕРЕДЕЛАТЬ!!!
        if (_player1.Hp <= 0 || _player2.Hp <= 0)
        {
            _player1 = null;
            _player2 = null;
        }
    }
}