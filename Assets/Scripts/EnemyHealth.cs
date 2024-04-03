using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth <= 0) Death();
    }

    private void Death()
    {
        
    }
}
