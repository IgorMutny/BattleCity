public class NewGameButton : MenuButton
{
    protected override void SetState()
    {
        _state = new NewGameMenuState();
    }
}
