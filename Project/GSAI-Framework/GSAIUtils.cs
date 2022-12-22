using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework
{
    public class GSAIUtils
    {
        public static Vector2 ToVector2(Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToVector3(Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
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
    }
}
