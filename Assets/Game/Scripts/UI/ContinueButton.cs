public class ContinueButton : MenuButton
{
    protected override void SetState()
    {
        _state = new ContinueGameState();
    }
}
