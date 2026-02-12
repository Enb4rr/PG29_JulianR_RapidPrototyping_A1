using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 4f;

    private Rigidbody2D rb;
    private bool hitSomething;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.up * speed;
        Invoke(nameof(Expire), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Asteroid asteroid = collision.GetComponent<Asteroid>();

        if (asteroid != null)
        {
            hitSomething = true;
            asteroid.DestroyAsteroid();
            Destroy(gameObject);
        }
    }

    private void Expire()
    {
        if (!hitSomething)
            ScoreManager.Instance?.ResetMultiplier();

        Destroy(gameObject);
    }
}