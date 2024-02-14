using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace StarSwarm.GSAI_Framework;

public partial class GSAIUtils
{
    public static Vector2 ToVector2(Vector3 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }

    public static Vector3 ToVector3(Vector2 vector)
    {
        return new Vector3(vector.X, vector.Y, 0);
    }

    public static Vector3 ClampedV3(Vector3 vector, float limit)
    {
        var lengthSquared = vector.LengthSquared();
        var limitSquared = limit * limit;
        if(lengthSquared > limitSquared)
            vector *= Mathf.Sqrt(limitSquared / lengthSquared);

        return vector;
    }

    public static Vector2 AngleToVector2(float angle)
    {
        return new Vector2(Mathf.Sin(-angle), Mathf.Cos(angle));
    }

    public static float Vector3ToAngle(Vector3 vector)
    {
        return Mathf.Atan2(vector.X, vector.Y);
    }

    public static float Vector2ToAngle(Vector2 vector)
    {
        return Mathf.Atan2(vector.X, -vector.Y);
    }
}
