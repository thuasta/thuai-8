using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position 
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double Angle { get; set; }

    public Position(double x = 0, double z = 0, double angle = 0)
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
        => HashCode.Combine(X, Z, Angle);
}
