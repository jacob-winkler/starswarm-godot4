using Godot;

public partial class DisintegrateEffect : Sprite2D
{
    private float _shaderValue = default!;

    public float Speed { get; set; } = 0.02f;

    public override void _Ready()
    {
        _shaderValue = 1f;
    }

    public override void _PhysicsProcess(double delta)
    {
        _shaderValue -= Speed;

        _shaderValue = Mathf.Clamp(_shaderValue, 0f, 1f);
        (Material as ShaderMaterial)!.SetShaderParameter("value", _shaderValue);

        if (_shaderValue == 0)
            QueueFree();
    }
}