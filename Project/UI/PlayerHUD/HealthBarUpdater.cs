using Godot;
using System;

public partial class HealthBarUpdater : Control
{
    public TextureProgressBar HealthBar { get; set; } = default!;

    public override void _Ready()
    {
        HealthBar = GetNode<TextureProgressBar>("HealthBar");
    }

    public void Initialize(PlayerShip player)
    {
        player.Stats.Connect("StatChanged", new Callable(this, "OnStatChanged"));
        player.Stats.Connect("MaxHealthUpdated", new Callable(this, "OnMaxHealthUpdated"));
        HealthBar.MaxValue = player.Stats.GetMaxHealth();
        HealthBar.Value = HealthBar.MaxValue;
    }

    public void OnStatChanged(String stat, float valueStart, float currentValue)
    {
        if (stat != "health")
            return;

        HealthBar.Value = currentValue;
    }

    public void OnMaxHealthUpdated(float maxHealth) => HealthBar.MaxValue = maxHealth;
}