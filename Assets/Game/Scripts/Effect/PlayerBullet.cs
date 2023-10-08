using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override bool CheckTankHits()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(
            _transform.position, new Vector2(_size, _size), 0, Vector2.zero, 0);

        bool destroyed = false;

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.GetComponent<Enemy>() != null)
            {
                raycastHit.collider.GetComponent<Enemy>().TakeDamage();
                destroyed = true;
            }
        }

        return destroyed;
    }
}
