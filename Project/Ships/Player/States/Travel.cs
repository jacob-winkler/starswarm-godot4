using Godot;
using StarSwarm.Project;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.Ships.Player;
using StarSwarm.Project.Ships.Player.States;

public partial class Travel : PlayerState
{
    public bool Reversing = false;
    public LoopingAudioStreamPlayer2D AudioThrusters = default!;

    public override void _Ready()
    {
        base._Ready();
        AudioThrusters = GetNode<LoopingAudioStreamPlayer2D>("ThrustersAudioPlayer");
    }

    public override void PhysicsProcess(double delta)
    {
        Debug.Assert(_parent != null, "Failed to process Travel state. Parent State is null.");

        var movement = GetMovement();
        Reversing = movement.Y > 0;
        var direction = GSAIUtils.AngleToVector2(((Move)_parent!).Agent.Orientation);

        AudioThrusters.GlobalPosition = ((PlayerShip)Owner).GlobalPosition;
        if (movement.Y < 0.0 && !AudioThrusters.Playing)
        {
            GD.Print(movement);
            GD.Print(AudioThrusters.Playing);
            AudioThrusters.Start();
        }
        else if (Mathf.IsEqualApprox(movement.Y, 0.0f) && !AudioThrusters.Ending)
        {
            GD.Print(movement);
            GD.Print(AudioThrusters.Ending);
            AudioThrusters.End();
        }

        ((Move)_parent!).LinearVelocity += movement.Y * direction * ((Move)_parent!).AccelerationMax * (Reversing ? ((Move)_parent!).ReverseMultiplier : 1) * (float)delta;
        ((Move)_parent!).AngularVelocity += movement.X * ((Move)_parent!).Agent.AngularAccelerationMax * (float)delta;

        ((Move)_parent!).PhysicsProcess(delta);
    }

    public Vector2 GetMovement() => new Vector2(
            Input.GetActionStrength("right") - Input.GetActionStrength("left"),
            Input.GetActionStrength("thrust_back") - Input.GetActionStrength("thrust_forwards")
        );
}
