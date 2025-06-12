using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;

    void Start()
    {
        slider.value = 100f;
        playerHealth.onHealthChanged.AddListener(UpdateBar);
    }

    void UpdateBar(int current, int max)
    {
        slider.value = current / (max / 100f);
    }
}