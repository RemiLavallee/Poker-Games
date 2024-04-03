using UnityEngine;
using UnityEngine.UI;

public class EnemyStatutBar : MonoBehaviour
{

    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Image fillImage;
    private Slider slider;

    private void Awake()
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (slider.value <= slider.minValue) fillImage.enabled = false;
        if (slider.value > slider.minValue && !fillImage.enabled) fillImage.enabled = true;
        
        var fillValue = enemyHealth.currentHealth / enemyHealth.maxHealth;
        slider.value = fillValue;

    }
}
