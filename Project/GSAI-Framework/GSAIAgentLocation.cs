using Godot;

namespace StarSwarm.Project.GSAI_Framework
{
    /// <summary>
    /// Represents an agent with only a location and an orientation.
    /// </summary>
    public class GSAIAgentLocation : Object
    {
        public Vector3 Position = Vector3.Zero;
        public float Orientation = 0.0F;
    }
}
