using Godot;
using System;

public class DisintegrateEffect : Sprite
{
    private float _shaderValue = default!;

    public float Speed { get; set; } = 0.03f;

    public override void _Ready()
    {
        _shaderValue = 1f;
    }

    public override void _PhysicsProcess(float delta)
    {
        _shaderValue -= Speed;

        _shaderValue = Mathf.Clamp(_shaderValue, 0f, 1f);
        (Material as ShaderMaterial)!.SetShaderParam("value", _shaderValue);

        if(_shaderValue == 0)
            QueueFree();
    }
}
