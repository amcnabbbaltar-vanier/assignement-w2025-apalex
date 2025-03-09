using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;
    
    void Start()
    {
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.health = 3; // Reset health
            GameManager.Instance.RespawnPlayer(); // Teleport player to the correct spawn point
        }
        UnPauseGame();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        // Ensure the cursor is visible and unlocked before loading the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Destroy GameManager instance before loading the main menu
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(0);
    }
}
