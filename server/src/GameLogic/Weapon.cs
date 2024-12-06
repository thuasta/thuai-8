namespace Thuai.Server.GameLogic;

public class Weapon
{
    public double attackSpeed;
    public double bulletSpeed;
    public bool isLaser;
    public bool antiArmor;
    public int damage;
    public int maxBullets;
    public int currentBullets;

    public Weapon()
    {
        this.attackSpeed = Constants.INITIAL_ATTACK_SPEED;
        this.bulletSpeed = Constants.INITIAL_BULLET_SPEED;
        this.isLaser = false;
        this.antiArmor = false;
        this.damage = Constants.INITIAL_DAMAGE;
        this.maxBullets = Constants.MAX_BULLETS;
        this.currentBullets = Constants.MAX_BULLETS;
    }
}