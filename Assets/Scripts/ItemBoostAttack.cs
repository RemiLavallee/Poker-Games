public class ItemBoostAttack : UsingItems
{
    private CarteManager carte;

    private void Start()
    {
       // carte = FindObjectOfType<CarteManager>();
    }

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
