using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Text Game Object")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;


    public static GameManager Instance { get; private set; }

    [Header("Game Values")]
    public int score = 0;
    public int health = 3;
    public float elapsedTime;


    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>(); // Ensures GameManager exists in case it wasn't in the scene
        }
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;
        elapsedTime = elapsedTime + Time.deltaTime;
        timerText.text = "Time: " + elapsedTime.ToString("0.00");
    }

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive between scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicate instances
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        // Restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        health = 3;
    }
}
