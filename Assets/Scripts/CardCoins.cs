public class CardCoins : UsingItems
{
    private CarteManager carteManager;
    private string coinsBoost = "Coins";
    public override void ApplyModifier()
    {
        carteManager = FindObjectOfType<CarteManager>();
        carteManager.activeCardBoost(coinsBoost);
        Inventory.instance.AddCoins(50);
        Inventory.instance.UpdateTextUi();
    }
}
