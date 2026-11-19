using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] protected float moveSpeed = 2.5f;
    [SerializeField] private int scoreValue = 100;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Move();

        if (transform.position.y < -5.5f)
            Destroy(gameObject);
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime));
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
            return;

        if (GameSession.Instance != null)
            GameSession.Instance.AddScore(scoreValue);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() == null)
            return;

        if (GameSession.Instance != null)
            GameSession.Instance.DamagePlayer();

        Destroy(gameObject);
    }
}
