using Godot;

namespace StarSwarm.Project.GSAI_Framework
{
    /// <summary>
    /// Represents an agent with only a location and an orientation.
    /// </summary>
    public class GSAIAgentLocation : Object
    {
        public Vector3 position = Vector3.Zero;
        public float orientation = 0.0F;
    }
}
