public class ItemHitHp : UsingItems
{
    private EnemyHealth enemyHealth;

    public override void ApplyModifier()
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
        enemyHealth.currentHealth -= 20;
    }
}
