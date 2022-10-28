using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] GameObject menuObj;
    
    public void GameOver()
    {
        Time.timeScale = 0f;
        finalScoreText.text = $"Final Score: {ScoreManager.Instance.Score}";
        menuObj.SetActive(true);
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
