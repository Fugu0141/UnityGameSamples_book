using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float shotsPerSecond = 4f;
    [SerializeField] private AudioSource shotAudio;

    private float nextShotTime;

    public void Configure(GameObject prefab, Transform spawnPoint)
    {
        bulletPrefab = prefab;
        muzzle = spawnPoint;
    }

    private void Update()
    {
        if (GameSession.Instance != null && GameSession.Instance.IsGameOver)
            return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.spaceKey.isPressed)
            return;

        if (Time.time < nextShotTime || bulletPrefab == null || muzzle == null)
            return;

        Shoot();
        nextShotTime = Time.time + 1f / Mathf.Max(0.1f, shotsPerSecond);
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        if (shotAudio != null && shotAudio.clip != null)
            shotAudio.Play();
    }
}
