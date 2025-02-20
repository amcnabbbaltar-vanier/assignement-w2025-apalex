using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI scoreText;

    public int score = 0; // Store the player's score

    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
