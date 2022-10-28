using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    [Serializable]
    public enum GameStates
    {
        Paused = 0,
        Playing = 1,
        GameOver = 2
    }

    [SerializeField] int maxTime;
    [SerializeField] float timeRemaining;
    [SerializeField] TextMeshProUGUI timeText;
    public GameStates _gameState;

    [SerializeField] PauseMenuController pauseMenu;
    [SerializeField] GameOverController goverMenu;
    [SerializeField] ShoppingCart player;

    private AudioSource audioSource;
    private WaitForSeconds waitForSecondsForAlertSound = new WaitForSeconds(1f);
    private Coroutine alertSoundCoroutine;

    void Start()
    {
        timeRemaining = maxTime;
        player = FindObjectOfType<ShoppingCart>();
        _gameState = GameStates.Playing;
        CheckStates();
    }

    void Update()
    {
        if (timeRemaining > 0f && _gameState == GameStates.Playing)
        {
            timeRemaining = Mathf.Clamp(timeRemaining, 0, timeRemaining - Time.deltaTime);
            string mins = TimeSpan.FromSeconds(timeRemaining).Minutes.ToString("00");
            string secs = TimeSpan.FromSeconds(timeRemaining).Seconds.ToString("00");
            if(timeRemaining <= 10f && timeText.color != Color.red)
            {
                timeText.color = Color.red;
            }

            timeText.text = $"Timer: {mins}:{secs}";

        }
        if (timeRemaining <= 0f && _gameState != GameStates.GameOver)
        {
            _gameState = GameStates.GameOver;
            CheckStates();
        }
        // Add alert sound to timer
        if (timeRemaining != 0 && timeRemaining <= 10f && alertSoundCoroutine == null) {
            alertSoundCoroutine = StartCoroutine(PlayTimerSound());
        }
        else if (timeRemaining == 0 || timeRemaining > 10f && alertSoundCoroutine != null) { // not the best way to do this
            StopCoroutine(alertSoundCoroutine);
            alertSoundCoroutine = null;
        }
    }

    IEnumerator PlayTimerSound() {
        while (timeRemaining != 0 && timeRemaining <= 10f) {
            audioSource.Play();
            yield return waitForSecondsForAlertSound;
        }
    }

    public void CheckStates()
    {
        switch (_gameState)
        {
            case GameStates.Playing:
                player.ToggleSkids(false);
                pauseMenu.TogglePause(false);
                break;
            case GameStates.Paused:
                player.ToggleSkids(true);
                pauseMenu.TogglePause(true);
                break;
            case GameStates.GameOver:
                player.ToggleSkids(true);
                goverMenu.GameOver();
                break;
            default:
                break;
        }
    }
}
