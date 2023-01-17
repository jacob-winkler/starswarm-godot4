using Godot;
using StarSwarm.Project;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.Ships.Player;
using StarSwarm.Project.Ships.Player.States;
using System;

public class Travel : PlayerState
{
    public bool Reversing = false;
    public LoopingAudioStreamPlayer2D AudioThrusters;

    public override void _Ready()
    {
        base._Ready();
        
        AudioThrusters = GetNode<LoopingAudioStreamPlayer2D>("ThrustersAudioPlayer");
    }

    public override void PhysicsProcess(float delta)
    {
        Debug.Assert(_parent != null, "Failed to process Travel state. Parent State is null.");

        var movement = GetMovement();
        Reversing = movement.y > 0;
        var direction = GSAIUtils.AngleToVector2(((Move)_parent!).Agent.orientation);

        AudioThrusters.GlobalPosition = ((PlayerShip)Owner).GlobalPosition;
        if (movement.y < 0.0 && !AudioThrusters.Playing)
            AudioThrusters.Start();
        else if (Mathf.IsEqualApprox(movement.y, 0.0f) && !AudioThrusters.Ending)
            AudioThrusters.End();

        ((Move)_parent!).LinearVelocity += movement.y * direction * ((Move)_parent!).AccelerationMax * (Reversing ? ((Move)_parent!).ReverseMultiplier : 1) * delta;
        ((Move)_parent!).AngularVelocity += movement.x * ((Move)_parent!).Agent.AngularAccelerationMax * delta;

        ((Move)_parent!).PhysicsProcess(delta);
    }

    public Vector2 GetMovement()
    {
        return new Vector2(
            Input.GetActionStrength("right") - Input.GetActionStrength("left"),
            Input.GetActionStrength("thrust_back") - Input.GetActionStrength("thrust_forwards")
        );
    }

    public override void UnhandledInput(InputEvent inputEvent)
    {
        base.UnhandledInput(inputEvent);
    }
}
