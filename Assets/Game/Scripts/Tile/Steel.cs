public class Steel : TileObject
{
    public void TakeDamage(bool isPowered)
    {
        if (isPowered == true)
        {
            Destroy();
        }
    }

}
