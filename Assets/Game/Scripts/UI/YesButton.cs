public class YesButton : MenuButton
{
    protected override void SetState()
    {
        _state = new NewGameState();
    }
}

