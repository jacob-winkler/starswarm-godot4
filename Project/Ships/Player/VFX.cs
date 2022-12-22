using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Player
{
    /// <summary>
    /// Represents a container for visual effects and provides a high level interface to create Ships visual effects.
    /// </summary>
    public class VFX : Node2D
    {
        private const float TRAIL_VELOCITY_THRESHOLD = 200;

        private CPUParticles2D _shipTrail;

        public override void _Ready()
        {
            _shipTrail = GetNode<CPUParticles2D>("MoveTrail");
        }

        public void MakeTrail(float currentSpeed)
        {
            _shipTrail.Emitting = currentSpeed > TRAIL_VELOCITY_THRESHOLD;
        }
    }
}
