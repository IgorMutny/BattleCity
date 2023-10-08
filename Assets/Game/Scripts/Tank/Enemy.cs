public class Enemy : Tank
{
    public override void Die(bool isScoreAdded)
    {
        EventBus.Invoke(new EnemyDestroyedEvent(this, _type));
        EventBus.Invoke(new SoundEvent(AudioController.EnemyDestroyedKey));
        base.Die(isScoreAdded);
    }
}
