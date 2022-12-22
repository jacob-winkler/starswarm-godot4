using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Autoload
{
    public class ObjectRegistry : Node
    {
        public Node2D _projectiles = new Node2D();
        private Node2D _effects = new Node2D();

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
    }
}
