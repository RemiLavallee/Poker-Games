public class ReliqueBoostCoins : UsingItems
{
    private Victory victory;

    public override void ApplyModifier()
    {
        victory = FindObjectOfType<Victory>();
        victory.coinsWin *= 2;
    }
}
