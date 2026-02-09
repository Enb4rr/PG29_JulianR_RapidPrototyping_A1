using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void UpdateHealth(float normalizedHealth)
    {
        fillImage.fillAmount = normalizedHealth;
    }
}