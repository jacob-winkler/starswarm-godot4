using Godot;
using StarSwarm.Project.GSAI_Framework;
using System;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States
{
    public partial class Rest : SpaceCrabState
    {
        private GSAITargetAcceleration _acceleration = new(); 

        public override void _PhysicsProcess(double delta)
        {
            Ship.Agent.ApplySteering(_acceleration, delta);
        }
    }
}