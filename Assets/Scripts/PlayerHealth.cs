using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject healthBar;
    private Defeat defeat;

    private void Start()
    {
        defeat = FindObjectOfType<Defeat>();
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
        defeat.DefeatCondition();
    }
}
