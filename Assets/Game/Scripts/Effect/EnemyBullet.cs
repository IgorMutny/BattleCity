using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override bool CheckTankHits()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(
            _transform.position, new Vector2(_size, _size), 0, Vector2.zero, 0);

        bool destroyed = false;

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.GetComponent<Player>() != null)
            {
                raycastHit.collider.GetComponent<Player>().TakeDamage();
                destroyed = true;
            }
        }

        return destroyed;
    }
}
