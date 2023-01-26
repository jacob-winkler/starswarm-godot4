using Godot;
using StarSwarm.Project.GSAI_Framework;
using System;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States
{
    public class Rest : SpaceCrabState
    {
        private GSAITargetAcceleration _acceleration = new GSAITargetAcceleration(); 

        public override void _PhysicsProcess(float delta)
        {
            Ship.Agent.ApplySteering(_acceleration, delta);
        }
    }
}