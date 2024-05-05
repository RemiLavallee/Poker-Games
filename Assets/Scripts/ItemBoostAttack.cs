public class ItemBoostAttack : UsingItems
{
    private CarteManager carte;

    public override void ApplyModifier()
    {
        carte = FindObjectOfType<CarteManager>();
        carte.attackPower *= 2;
    }

    public override void DisableModifier()
    {
        carte = FindObjectOfType<CarteManager>();
        carte.attackPower /= 2;
    }
}
