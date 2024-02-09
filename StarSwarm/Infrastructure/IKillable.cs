using StarSwarm.Weapons;

namespace StarSwarm.StarSwarm.Infrastructure;
public interface IKillable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="origin"></param>
    void TakeDamage(float damage, DamageType type); // origin -- consider adding a custom object to represent attributes about the damage.
}
