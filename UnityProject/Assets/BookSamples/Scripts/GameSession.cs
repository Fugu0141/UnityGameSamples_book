using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    [SerializeField] private int startingHp = 3;
    [SerializeField] private bool healthEnabled;
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text hpLabel;
    [SerializeField] private TMP_Text gameOverLabel;

    public int Score { get; private set; }
    public int Hp { get; private set; }
    public bool IsGameOver { get; private set; }

    public void Configure(TMP_Text score, TMP_Text hp, TMP_Text gameOver, bool enableHealth)
    {
        scoreLabel = score;
        hpLabel = hp;
        gameOverLabel = gameOver;
        healthEnabled = enableHealth;
    }

    private void Awake()
    {
        Instance = this;
        Hp = startingHp;
    }

    private void Start()
    {
        if (gameOverLabel != null)
            gameOverLabel.gameObject.SetActive(false);

        RefreshUi();
    }

    private void Update()
    {
        if (!IsGameOver || Keyboard.current == null || !Keyboard.current.rKey.wasPressedThisFrame)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        if (IsGameOver)
            return;

        Score += points;
        RefreshUi();
    }

    public void DamagePlayer()
    {
        if (!healthEnabled || IsGameOver)
            return;

        Hp = Mathf.Max(0, Hp - 1);
        if (Hp == 0)
        {
            IsGameOver = true;
            if (gameOverLabel != null)
                gameOverLabel.gameObject.SetActive(true);
        }

        RefreshUi();
    }

    private void RefreshUi()
    {
        if (scoreLabel != null)
            scoreLabel.text = "SCORE: " + Score;

        if (hpLabel != null)
            hpLabel.text = "HP: " + Hp;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
