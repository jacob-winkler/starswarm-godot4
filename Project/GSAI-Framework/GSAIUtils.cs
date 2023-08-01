using Godot;

namespace StarSwarm.Project.GSAI_Framework;

public partial class GSAIUtils
{
    public static Vector2 ToVector2(Vector3 vector) => new(vector.X, vector.Y);

    public static Vector3 ToVector3(Vector2 vector) => new(vector.X, vector.Y, 0);

    public static Vector3 ClampedV3(Vector3 vector, float limit)
    {
        var lengthSquared = vector.LengthSquared();
        var limitSquared = limit * limit;
        if (lengthSquared > limitSquared)
            vector *= Mathf.Sqrt(limitSquared / lengthSquared);

        return vector;
    }

    public static Vector2 AngleToVector2(float angle) => new(Mathf.Sin(-angle), Mathf.Cos(angle));

    public static float Vector3ToAngle(Vector3 vector) => Mathf.Atan2(vector.X, vector.Y);

    public static float Vector2ToAngle(Vector2 vector) => Mathf.Atan2(vector.X, -vector.Y);
}