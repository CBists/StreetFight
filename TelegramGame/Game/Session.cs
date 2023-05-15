using TelegramGame.Game.Entity;

namespace TelegramGame.Game;

public class Session
{
    private bool _player1PackageReady = false;
    private readonly IGamePlayer _player1;
    private bool _player2PackageReady = false;
    private readonly IGamePlayer _player2;

    public Session(IGamePlayer player1, IGamePlayer player2)
    {
        _player1 = player1;
        _player2 = player2;
        player1.SetPackageEvent += SetPlayerPackageEvent;
        player2.SetPackageEvent += SetPlayerPackageEvent;
        player1.GetEnemyEvent += GetEnemyEvent;
        player2.GetEnemyEvent += GetEnemyEvent;
    }

    private void SetPlayerPackageEvent(IGamePlayer player)
    {
        if (_player1 == player)
        {
            _player1PackageReady = true;
            _player2.EnemyConfirmPackage();
        }
        else
        {
            _player2PackageReady = true;
            _player1.EnemyConfirmPackage();
        }

        if (_player1PackageReady && _player2PackageReady)
            CalculateResult();
    }

    private IGamePlayer GetEnemyEvent(IGamePlayer player) => player.Equals(_player1) ? _player2 : _player1;

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
        _player1.SendResult();
        _player2.SendResult();
    }
}