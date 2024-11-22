namespace Thuai.Server.GameLogic;

public class Bullet : Item
{
    public double dirx { get; set; }
    public double diry { get; set; }
    public int maxage { get; set; }
    public double size { get; set; }
    public int age { get; set; }
    public double speed { get; set; }
    public int Team { get; set; }
    public int color { get; set; } 

    // 构造函数
    public Bullet(double Xpos, double Ypos, double Dirx, double Diry, int Color)
        : base(Xpos, Ypos)
    {
        Team = Color;
        maxage = 20;
        size = 0.05;
        dirx = Dirx;
        diry = Diry;
        age = 0;
        speed = 1.0 / 20; // 每秒移动一个单位
        Console.Error.WriteLine("bulletgen");
        Game.bulletcnt[Color]++;
    }

    // 销毁子弹
    public void Destroy()
    {
        for (int i = 0; i < Game.bullets.Count; i++)
        {
            if (Game.bullets[i] == this)
            {
                Game.bullets.RemoveAt(i);
                break;
            }
        }
    }

    // 移动子弹
    public void Move()
    {
        Xpos += speed * dirx;
        Ypos += speed * diry;
        if (++age == maxage)
        {
            // 子弹消失
            Destroy();
            return;
        }

        bool crashx = false, crashy = false;
        foreach (var ptr in Game.walls)
        {
            if (ptr.direction) // 竖着的
            {
                if (Ypos >= ptr.Ypos && Ypos <= ptr.Ypos + 1 && Xpos >= ptr.Xpos - size && Xpos <= ptr.Xpos + size) // 撞墙
                {
                    crashx = true;
                    Console.Error.WriteLine("bulletbounce");
                }
            }
            else // 横着的
            {
                if (Xpos >= ptr.Xpos && Xpos <= ptr.Xpos + 1 && Ypos >= ptr.Ypos - size && Ypos <= ptr.Ypos + size) // 撞墙
                {
                    crashy = true;
                    Console.Error.WriteLine("bulletbounce");
                }
            }
        }

        if (crashx || crashy)
        {
            Xpos -= speed * dirx;
            Ypos -= speed * diry;
            if (crashx) dirx = -dirx;
            if (crashy) diry = -diry;
        }
    }

    // 析构函数
    ~Bullet()
    {
        Game.bulletcnt[color]--;
        Console.Error.WriteLine("bulletvanish");
    }
}