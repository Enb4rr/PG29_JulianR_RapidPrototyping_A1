using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int scoreValue = 10;
    
    [Header("Feedback")]
    [SerializeField] private GameObject destroyParticles;

    private Rigidbody2D rb;
    private CameraShake cameraShake;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraShake = Camera.main.gameObject.GetComponent<CameraShake>();
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    
    public void DestroyAsteroid()
    {
        ScoreManager.Instance?.AddScore(scoreValue);
        
        if (destroyParticles != null)
            Instantiate(destroyParticles, transform.position, Quaternion.identity);
        
        if (cameraShake != null)
            cameraShake.Shake(0.1f, 0.15f);
        
        Destroy(gameObject);
    }
}