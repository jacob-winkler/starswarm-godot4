namespace StarSwarm.StarSwarm.Infrastructure;
public interface IKillable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="origin"></param>
    void TakeDamage(float damage, object origin); // origin -- consider adding a custom object to represent attributes about the damage.
}
