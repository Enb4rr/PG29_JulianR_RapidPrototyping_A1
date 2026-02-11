using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private PlayerHealthUI healthUI;
    
    [Header("Shield")]
    [SerializeField] private int maxShield = 3;
    [SerializeField] private GameObject shieldVisual;
    private int currentShield;
    
    [Header("Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    
    [Header("Particles")]
    [SerializeField] private GameObject hitParticles;
    
    [Header("Camera")]
    [SerializeField] private CameraShake cameraShake;

    private int currentHealth;
    
    // Feedback
    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateShieldVisual();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (currentShield > 0)
        {
            currentShield -= amount;

            if (currentShield < 0)
            {
                amount = -currentShield;
                currentShield = 0;
            }
            else
            {
                UpdateShieldVisual();
                Flash();
                return;
            }

            UpdateShieldVisual();
        }

        currentHealth -= amount;
        UpdateHealthUI();
        Flash();

        if (hitParticles != null)
            Instantiate(hitParticles, transform.position, Quaternion.identity);

        if (cameraShake != null)
            cameraShake.Shake(0.15f, 0.2f);

        if (currentHealth <= 0)
            Die();
    }
    
    void UpdateShieldVisual()
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(currentShield > 0);
    }

    void UpdateHealthUI()
    {
        if (healthUI != null)
            healthUI.UpdateHealth((float)currentHealth / maxHealth);
    }

    private void Die()
    {
        Destroy(gameObject);

        GameStartManager.Instance.OnPlayerLost();
    }
    
    void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}