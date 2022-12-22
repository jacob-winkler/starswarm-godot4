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

            var A = b.x - a.x;
            var B = b.y - a.y;
            var C = c.x - a.x;
            var D = c.y - a.y;
            var E = A * (a.x + b.x) + B * (a.y + b.y);
            var F = C * (a.x + c.x) + D * (a.y + c.y);
            var G = 2 * (A * (c.y - b.y) - B * (c.x - b.x));
            float dx;
            float dy;

            if (Mathf.Abs(G) < 0.000001f)
            {
                dx = (Mathf.Max(a.x, Mathf.Max(b.x, c.x))) * 0.5f;
                dy = (Mathf.Max(a.y, Mathf.Max(b.y, c.y))) * 0.5f;
            }
            else
            {
                dx = ((D * E - B * F) / G) - a.x;
                dy = ((A * F - C * E) / G) - a.y;
            }

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}
