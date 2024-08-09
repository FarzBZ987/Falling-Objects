using System.Xml;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private float _scoreValue;

    private float highScore;

    private float scoreValue
    {
        set
        {
            if (!GameManager.gameStarted) return;
            if (value > 0) AudioManager.instance.Beeps();
            _scoreValue = value;
            if (scoreValue > highScore) highScore = scoreValue;
            updateScore();
        }
        get => _scoreValue;
    }

    private void OnEnable()
    {
        Add.onTriggerTouchingEvents += AddScore;
        PowerUp.onTriggerTouchingEvents += AddMagnetScore;
        PlayButton.onClickEvents += ResetScore;
    }

    private void OnDisable()
    {
        PlayButton.onClickEvents -= ResetScore;
        Add.onTriggerTouchingEvents -= AddScore;
        PowerUp.onTriggerTouchingEvents -= AddMagnetScore;
    }

    private void AddScore(float val) => scoreValue += val;

    private void AddMagnetScore() => scoreValue += 5;

    private void ResetScore()
    {
        scoreValue = 0;
    }

    private void updateScore()
    {
        scoreText.text = Mathf.Floor(highScore).ToString() + System.Environment.NewLine + Mathf.Floor(scoreValue).ToString();
    }
}