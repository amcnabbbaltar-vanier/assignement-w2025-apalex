using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 1;

            } else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 0;
            }
        }
    }

    void Restart()
    {

    }

    void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
