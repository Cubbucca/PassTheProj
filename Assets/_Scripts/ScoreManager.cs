using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Animator animator;

    public static event Action<int> OnNewScore;
    
    void Awake()
    {
        Instance = this;
        AddScore(0);
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        scoreText.text = $"${Score}";
        // animator.Play("score_popup"); // I think I broke this... not sure what it was meant to do ><
        
        OnNewScore?.Invoke(Score);
    }
    
}
