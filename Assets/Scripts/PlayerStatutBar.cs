using UnityEngine;
using UnityEngine.UI;

public class PlayerStatutBar : MonoBehaviour
{

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;
    private Slider slider;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (slider.value <= slider.minValue) fillImage.enabled = false;
        if (slider.value > slider.minValue && !fillImage.enabled) fillImage.enabled = true;
        
        var fillValue = playerHealth.currentHealth / playerHealth.maxHealth;
        slider.value = fillValue;

    }
}
