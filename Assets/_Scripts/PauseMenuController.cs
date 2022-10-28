using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject controlMenu;
    public void TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        
        pauseMenu.SetActive(isPaused);
        GameManager.GameStates lastState = GameManager.instance._gameState;
        if (isPaused)
        {
            GameManager.instance._gameState = GameManager.GameStates.Paused;
        }
        else
        {
            GameManager.instance._gameState = GameManager.GameStates.Playing;
            if(controlMenu.activeInHierarchy)
            {
                controlMenu.SetActive(false);
            }
        }
        if (lastState != GameManager.instance._gameState)
        {
            GameManager.instance.CheckStates();
        }
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
