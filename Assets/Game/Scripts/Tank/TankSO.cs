using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TankSO : ScriptableObject
{
    public int Type;
    public List<Sprite> Sprites;
    public Color DefaultColor;
    public float AnimationTime;
    public int Health;
    public float Speed;
    public GameObject BulletSample;
    public int BulletsPerShot;
    public int ScoreForDestruction;
}
