using Godot;
using StarSwarm.Project.Autoload;

//  The main camera that follows the location of the player. It manages
//  zooming out when the map button is pressed, and manages the creation
//  of a duplicate itself that will live in the minimap viewport, and will follow
//  the original's position in the world using a `RemoteTransform2D`.

//  The camera supports zooming and camera shake.
public partial class PlayerCamera : Camera2D
{

    public const float SHAKE_EXPONENT = 1.8f;

    [Export]
    public float MaxZoom = 5.0f;
    
    [Export]
    public float DecayRate = 1.0f;
    [Export]
    public Vector2 MaxOffset = new Vector2(100.0f, 100.0f);
    
    [Export]
    public float MaxRotation = 0.1f;

    private float _shakeAmount = 0.0f;
    public float ShakeAmount { 
        get { return _shakeAmount; } 
        set {
            _shakeAmount = Mathf.Clamp(value, 0.0f, 1.0f);
            SetPhysicsProcess(_shakeAmount != 0.0f);
        }
    }

    public float NoiseY = 0.0f;
 
    private Vector2 _startZoom;
    private Vector2 _startPosition = Vector2.Zero;

    public ObjectRegistry ObjectRegistry;
    public Events Events;
    public RemoteTransform2D RemoteMap;
    public RemoteTransform2D RemoteDistort;
    public FastNoiseLite Noise = new FastNoiseLite();


    public override void _Ready() 
    {
        _startZoom = Zoom;

        Events = GetNode<Events>("/root/Events");
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        RemoteMap = GetNode<RemoteTransform2D>("RemoteMap");
        RemoteDistort = GetNode<RemoteTransform2D>("RemoteDistort");

        SetPhysicsProcess(false);

        // Events.Connect("map_toggled", this, "_toggle_map");
        // Events.Connect("explosion_occurred", this, "_on_Events_explosion_occurred");

        GD.Randomize();
        Noise.Seed = (int)GD.Randi();
        Noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
        Noise.Frequency = 24;
        Noise.FractalOctaves = 2;
    }


    public override void _PhysicsProcess(double delta)
    {
        ShakeAmount -= DecayRate * (float)delta;
        Shake();
    }


    public void Shake()
    {
        var amount = Mathf.Pow(ShakeAmount, SHAKE_EXPONENT);

        NoiseY += 1.0f;
        Rotation = MaxRotation * amount * Noise.GetNoise2D(Noise.Seed, NoiseY);
        Offset = new Vector2(
            MaxOffset.X * amount * Noise.GetNoise2D(Noise.Seed * 2, NoiseY),
            MaxOffset.Y * amount * Noise.GetNoise2D(Noise.Seed * 3, NoiseY)
        );
    }
}
