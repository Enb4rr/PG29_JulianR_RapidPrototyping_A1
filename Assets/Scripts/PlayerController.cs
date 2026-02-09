using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate = 0.25f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private float nextFireTime;

    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        CalculateBounds();
    }

    void CalculateBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);

        minBounds = bottomLeft;
        maxBounds = topRight;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (Time.time < nextFireTime)
            return;

        Shoot();
        nextFireTime = Time.time + fireRate;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        transform.position = pos;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
    }
}