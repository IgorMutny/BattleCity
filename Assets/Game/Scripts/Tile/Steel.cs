using UnityEngine;

public class Steel : TileObject, IObstacle, IDamageableObstacle
{
    public bool TakeDamage(Quaternion rotation, bool isPowered)
    {
        if (isPowered == true)
        {
            Destroy();
            return true;
        }
        else
        {
            return false;
        }
    }

}
