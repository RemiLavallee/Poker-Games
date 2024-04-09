public class ReliqueIncreaseDraw : UsingItems
{
    private CarteManager player;
    
    public override void ApplyModifier()
    {
        player = FindObjectOfType<CarteManager>();
        player.hitDraw++;
    }
}