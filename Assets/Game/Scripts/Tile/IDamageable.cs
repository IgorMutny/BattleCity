using UnityEngine;

public interface IDamageableObstacle
{
    public bool TakeDamage(Quaternion rotation, bool isPowered);
}
