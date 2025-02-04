using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position 
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double Angle { get; set; }

    public Position(double x, double z, double angle)
    {
        this.X = x * Constants.FLOOR_LEN + Constants.POS_BIAS;
        this.Y = Constants.YPOS;
        this.Z = z * Constants.FLOOR_LEN + Constants.POS_BIAS;
        this.Angle = angle;
    }
    public override bool Equals(object obj)
    {
        if (obj is Position other)
        {
            // 比较 X, Y 和 Angle 是否相等
            return X.Equals(other.X) && Z.Equals(other.Z) && Angle.Equals(other.Angle);
        }
        return false;
    }

    public override int GetHashCode()
    {
        // 生成一个基于 X, Y 和 Angle 的哈希码
        int hashCode = 17; // 一个任意的常数
        hashCode = hashCode * 23 + X.GetHashCode();
        hashCode = hashCode * 23 + Y.GetHashCode();
        hashCode = hashCode * 23 + Angle.GetHashCode();
        return hashCode;
    }
}
