using Godot;
using StarSwarm.Project.Autoload;
using System;

public class GameInitializer : Node
{
    public ObjectRegistry? ObjectRegistry { get; set; }
    public PlayerCamera? Camera { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ObjectRegistry = GetNode<ObjectRegistry>("/root/Autoload/ObjectRegistry");
        Camera = GetNode<PlayerCamera>("GameWorld/Camera");

        ObjectRegistry.RegisterDistortionParent(GetNode<Viewport>("DistortMaskView/Viewport"));
	    Camera.SetupDistortionCamera();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
