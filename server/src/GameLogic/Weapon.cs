namespace Thuai.Server.GameLogic;

public class Weapon
{
    public float AttackSpeed { get; set; } = Constants.INITIAL_ATTACK_SPEED;
    public float BulletSpeed { get; set; } = Constants.INITIAL_BULLET_SPEED;
    public bool IsLaser { get; set; } = false;
    public bool AntiArmor { get; set; } = false;
    public int Damage { get; set; } = Constants.INITIAL_DAMAGE;
    public int MaxBullets { get; set; } = Constants.INITIAL_BULLETS;
    public int CurrentBullets { get; set; } = Constants.INITIAL_BULLETS;
    public int MaximumCooldown => (int)Math.Ceiling(1 / AttackSpeed);
    public bool CanAttack => _currentCoolDown == 0;

    private int _currentCoolDown = 0;

    public void Recover()
    {
        CurrentBullets = MaxBullets;
        _currentCoolDown = 0;
    }

    public void Update()
    {
        if (_currentCoolDown > 0)
        {
            _currentCoolDown--;
        }
    }
}
