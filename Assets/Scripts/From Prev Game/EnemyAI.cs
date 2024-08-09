using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 90f; // Degrees per second
    public float changeDirectionInterval = 3f;
    public float minHeight = 2f; // Minimum flying height
    public float maxHeight = 10f; // Maximum flying height

    private Vector3 currentDirection;
    private float timer = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody not found. Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        timer = changeDirectionInterval;
        currentDirection = GetRandomDirection();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        AdjustHeight();
        UpdateTimer();
    }

    private void Move()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + currentDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogError("Rigidbody is null in Move() method.");
        }
    }

    private void Rotate()
    {
        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void AdjustHeight()
    {
        float currentHeight = transform.position.y;
        if (currentHeight < minHeight)
        {
            currentDirection.y = Mathf.Abs(currentDirection.y); // Move upward
        }
        else if (currentHeight > maxHeight)
        {
            currentDirection.y = -Mathf.Abs(currentDirection.y); // Move downward
        }
    }

    private void UpdateTimer()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            currentDirection = GetRandomDirection();
            timer = changeDirectionInterval;
        }
    }

    private Vector3 GetRandomDirection()
    {
        // Generate random direction within the X-Z plane
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }


}
