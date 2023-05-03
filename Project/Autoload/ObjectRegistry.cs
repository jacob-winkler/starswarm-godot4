using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Autoload
{
    /// <summary>
    /// Creates, maintains, and organizes spawned special effects or projectiles; 
    /// objects that should be untied from their spawners' lifespan when freed.
    /// </summary>
    public class ObjectRegistry : Node
    {
        private Node2D _projectiles = new Node2D();
        private Node2D _effects = new Node2D();
        private Viewport _distortions = new Viewport();

        public override void _Ready()
        {
            _projectiles = GetNode<Node2D>("Projectiles");
            _effects = GetNode<Node2D>("Effects");
        }

        public void RegisterEffect(Node effect)
        {
            _effects.AddChild(effect);
        }

        public void RegisterProjectiles(Node projectile)
        {
            _projectiles.AddChild(projectile);
        }

        public void RegisterDistortionEffect(Node2D effect)
        {
            _distortions?.AddChild(effect);
        }

        public void RegisterDistortionParent(Viewport viewport)
        {
            _distortions = viewport;
        }
    }
}
