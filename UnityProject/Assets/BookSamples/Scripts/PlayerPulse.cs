using UnityEngine;

// Optional polish for Chapter 11: simple color animation.
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerPulse : MonoBehaviour
{
    [SerializeField] private Color baseColor = new Color(0.2f, 0.9f, 0.95f);
    [SerializeField] private float pulseSpeed = 3f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float brightness = 0.8f + 0.2f * Mathf.Sin(Time.time * pulseSpeed);
        spriteRenderer.color = baseColor * brightness;
    }
}
