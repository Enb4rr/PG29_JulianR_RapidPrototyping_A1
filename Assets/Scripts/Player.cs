using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private PlayerHealthUI healthUI;
    
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
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();
        Flash();
        
        if (hitParticles != null)
            Instantiate(hitParticles, transform.position, Quaternion.identity);
        
        if (cameraShake != null)
            cameraShake.Shake(0.15f, 0.2f); // duration, magnitude

        if (currentHealth <= 0)
            Die();
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