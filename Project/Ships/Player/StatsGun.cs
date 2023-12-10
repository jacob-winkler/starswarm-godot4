using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public float GetDamage()
    {
        return GetStat("damage");
    }

    public float GetCooldown()
    {
        return GetStat("cooldown");
    }

    public float GetSpread()
    {
        return GetStat("spread");
    }
}
