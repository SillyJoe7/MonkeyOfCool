using UnityEngine;
using UnityEngine.UI;  // Needed for UI elements

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI
    private bool isPaused = false;  // Track if the game is paused

    void Update()
    {
        // Check for escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);  // Hide the pause menu
        Time.timeScale = 1f;  // Resume game time
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // Show the pause menu
        Time.timeScale = 0f;  // Stop game time (pause the game)
        isPaused = true;
    }

    public void QuitGame()
    {
        // You can call Application.Quit() to quit the game
        // For testing, use this code:
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
