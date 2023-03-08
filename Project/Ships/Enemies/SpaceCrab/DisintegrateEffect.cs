using Godot;
using System;

public class DisintegrateEffect : Sprite
{
    private Boolean _fadeout { get; set; } = false;
    private float _shaderValue = default!;

    private float _speed = 0.03f;

    public override void _Ready()
    {
         _shaderValue = 1f;
    }

    public override void _PhysicsProcess(float delta)
    {
        _shaderValue -= _speed;

        _shaderValue = Mathf.Clamp(_shaderValue, 0f, 1f);
        (Material as ShaderMaterial)!.SetShaderParam("value", _shaderValue);

        if(_shaderValue == 0)
            QueueFree();
    }
}
