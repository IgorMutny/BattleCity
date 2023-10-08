public class NoButton : MenuButton
{
    protected override void SetState()
    {
        _state = new MainMenuState();
    }
}
