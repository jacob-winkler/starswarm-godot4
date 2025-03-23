using StarSwarm.Weapons;

namespace StarSwarm.Infrastructure;
public interface IKillable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="origin"></param>
    public void TakeDamage(float damage, DamageType type);
}
