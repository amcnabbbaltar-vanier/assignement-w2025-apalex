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

    private GameObject player; // Reference to the player

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager persistent
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;
        elapsedTime += Time.deltaTime;
        timerText.text = "Time: " + elapsedTime.ToString("0.00");
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Assumes the player has the tag "Player"
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            health = 3; // Reset health on game over
            RespawnPlayer(); // Teleport instead of reloading the scene
        }
    }

    public void RespawnPlayer()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null) return; // Prevent errors if player isn't found
        }

        Vector3 respawnPosition = GetRespawnPoint();
        player.transform.position = respawnPosition;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Reset velocity to prevent physics issues
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetRespawnPoint()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Level1")
        {
            return new Vector3(0, 1, 0);
        }
        else if (currentSceneName == "Level2")
        {
            return new Vector3(27, 1, 0);
        }
        else if (currentSceneName == "Level3")
        {
            return new Vector3(0, 1, 0);
        }
        else
        {
            return new Vector3(0, 1, 0);
        }
    }
}
