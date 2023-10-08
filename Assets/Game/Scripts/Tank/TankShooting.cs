using System.Collections.Generic;
using UnityEngine;

public class TankShooting
{
    private GameObject _bulletSample;
    private Transform _transform;
    private int _bulletsPerShot;

    private List<GameObject> _bullets = new List<GameObject>();
    private float _reloadTime = 0.1f;
    private float _reloadTimeCurrent;
    private int _bulletsPrepared = 0;

    public TankShooting(Transform transform, GameObject bulletSample, int bulletsPerShot)
    {
        _transform = transform;
        _bulletSample = bulletSample;
        _bulletsPerShot = bulletsPerShot;
    }

    public void FixedUpdate()
    {
        FixBulletsList();
        TryCreateBullet();
        TryDecreaseReloadTime();
    }

    public void TryShoot()
    {
        if (_bullets.Count == 0)
        {
            _bulletsPrepared = _bulletsPerShot;
        }
    }

    public bool HasShotBullets()
    {
        return _bullets.Count > 0;
    }

    public void DestroyAllBullets()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            GameObject.Destroy(_bullets[i]);
        }
    }

    private void FixBulletsList()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i] == null)
            {
                _bullets.RemoveAt(i);
            }
        }
    }

    private void TryCreateBullet()
    {
        if (_bulletsPrepared > 0 && _reloadTimeCurrent == 0)
        {
            CreateBullet();
            _reloadTimeCurrent = _reloadTime;
            _bulletsPrepared -= 1;
        }
    }

    private void CreateBullet()
    {
        GameObject bullet = GameObject.Instantiate(_bulletSample, _transform.position, _transform.rotation);
        bullet.transform.Translate(Vector3.up);
        bullet.GetComponent<Bullet>().Init();
        _bullets.Add(bullet);
    }

    private void TryDecreaseReloadTime()
    {
        if (_reloadTimeCurrent > 0)
        {
            _reloadTimeCurrent -= Time.fixedDeltaTime;
            if (_reloadTimeCurrent < 0)
            {
                _reloadTimeCurrent = 0;
            }
        }
    }


}
