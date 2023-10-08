public class ExitButton : MenuButton
{
    protected override void SetState()
    {
        _state = new ExitGameState();
    }
}
