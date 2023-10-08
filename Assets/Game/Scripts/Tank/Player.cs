public class Player : Tank
{
    public override void Die(bool isScoreAdded)
    {
        EventBus.Invoke(new PlayerDestroyedEvent());
        EventBus.Invoke(new SoundEvent(AudioController.PlayerDestroyedKey));
        base.Die(isScoreAdded);
    }
}
