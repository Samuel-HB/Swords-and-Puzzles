using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject containerPause;
    public GameObject detailsContainer;
    private bool isGamePaused = false;


    private void Start()
    {
        EventManager.pressingPauseGameButton += PauseOrRestartGame;

        detailsContainer.SetActive(false);
        containerPause.SetActive(false);
    }

    private void PauseOrRestartGame()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else {
            PausedGame();
        }
    }
    public void PausedGame()
    {
        Time.timeScale = 0;
        containerPause.SetActive(true);
        isGamePaused = true;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        EventManager.PauseGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        containerPause.SetActive(false);
        SettingsClosed();
        isGamePaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        EventManager.ResumeGame();
    }

    public void SettingsOpen()
    {
        detailsContainer.SetActive(true);
    }

    public void SettingsClosed()
    {
        detailsContainer.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        EventManager.pressingPauseGameButton -= PauseOrRestartGame;
    }
}
