using UnityEngine;

// Chapter 10: inherits the common HP, collision and score behavior from Enemy.
public class FastEnemy : Enemy
{
    [SerializeField] private float horizontalSpeed = 1.5f;

    protected override void Move()
    {
        Vector3 movement = new Vector3(
            Mathf.Sin(Time.time * 3f) * horizontalSpeed,
            -moveSpeed,
            0f
        );

        transform.Translate(movement * Time.deltaTime);
    }
}
