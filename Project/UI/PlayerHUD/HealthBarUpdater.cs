using Godot;
using System;

public class HealthBarUpdater : Control
{
    public TextureProgress HealthBar { get; set; } = default!;
    public override void _Ready()
    {
        HealthBar = GetNode<TextureProgress>("HealthBar");
    }

    public void Initialize(PlayerShip player)
    {
        player.Stats.Connect("StatChanged", this, "OnHealthUpdated");
    }

    public void OnStatChanged(String stat, float valueStart, float currentValue)
    {
        if (stat != "health")
            return;

        HealthBar.Value = currentValue;
    }

    public void OnMaxHealthUpdated(float maxHealth)
    {
        HealthBar.MaxValue = maxHealth;
    }
}
