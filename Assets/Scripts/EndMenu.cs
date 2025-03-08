using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            // Stop the timer
            GameManager.Instance.enabled = false;

            // Display final score and time
            if (timerText != null) 
                timerText.text = "Time: " + GameManager.Instance.elapsedTime.ToString("0.00") + "s";
            if (scoreText != null) 
                scoreText.text = "Score: " + GameManager.Instance.score;

            // Destroy GameManager after showing the values
            Destroy(GameManager.Instance.gameObject);
        }
        else
        {
            Debug.LogError("GameManager.Instance is NULL. Make sure GameManager exists in the first scene.");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
