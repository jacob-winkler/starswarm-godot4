using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Utils
{
    /// <summary>
    /// Helper global object that holds helpful mathematics functions that are not .part of Godot
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Returns the radius of a circumcircle for a triangle.
        /// The circumscribed circle or circumcircle of a polygon is a circle that passes through all the vertices of the polygon.
        /// Adapted from an algorithm by [mutoo](https://gist.github.com/mutoo/5617691)
        /// </summary>
        public static float GetTriangleCircumcircleRadius(Vector2[] vertices)
        {
            Debug.Assert(vertices.Length == 3, "Number of vertices is not 3.");

            var a = vertices[0];
            var b = vertices[1];
            var c = vertices[2];

            var A = b.X - a.X;
            var B = b.Y - a.Y;
            var C = c.X - a.X;
            var D = c.Y - a.Y;
            var E = A * (a.X + b.X) + B * (a.Y + b.Y);
            var F = C * (a.X + c.X) + D * (a.Y + c.Y);
            var G = 2 * (A * (c.Y - b.Y) - B * (c.X - b.X));
            float dx;
            float dy;

            if (Mathf.Abs(G) < 0.000001f)
            {
                dx = (Mathf.Max(a.X, Mathf.Max(b.X, c.X))) * 0.5f;
                dy = (Mathf.Max(a.Y, Mathf.Max(b.Y, c.Y))) * 0.5f;
            }
            else
            {
                dx = ((D * E - B * F) / G) - a.X;
                dy = ((A * F - C * E) / G) - a.Y;
            }

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}
