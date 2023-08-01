using Godot;

namespace StarSwarm.Project.Ships.Player;

public partial class StatsGun : Stats
{
    [Export]
    private float _damage = 4.0F;

    [Export]
    private float _cooldown = 0.14F;

    [Export]
    private float _spread = 30.0F;

    public StatsGun()
    {
        UpdateAll();
    }

    public float GetDamage() => GetStat("damage");

    public float GetCooldown() => GetStat("cooldown");

    public float GetSpread() => GetStat("spread");
}