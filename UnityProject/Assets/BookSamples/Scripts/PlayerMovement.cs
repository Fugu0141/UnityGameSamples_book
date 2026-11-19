using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 horizontalLimits = new Vector2(-7.5f, 7.5f);
    [SerializeField] private Vector2 verticalLimits = new Vector2(-4.3f, 4.3f);

    private void Start()
    {
        Debug.Log("Player is ready! WASD / Arrow keys to move.");
    }

    private void Update()
    {
        if (GameSession.Instance != null && GameSession.Instance.IsGameOver)
            return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        Vector2 direction = Vector2.zero;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) direction.x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) direction.x += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) direction.y -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) direction.y += 1f;

        // normalized: diagonal movement should not be faster.
        Vector3 movement = (Vector3)direction.normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, horizontalLimits.x, horizontalLimits.y);
        position.y = Mathf.Clamp(position.y, verticalLimits.x, verticalLimits.y);
        transform.position = position;
    }
}
