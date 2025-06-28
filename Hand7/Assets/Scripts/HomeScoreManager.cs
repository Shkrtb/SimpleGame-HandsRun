using UnityEngine;
using UnityEngine.UI;

public class HomeHighScoreDisplay : MonoBehaviour
{
    public Text highScoreText;
    
    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        string highScoreName = PlayerPrefs.GetString("HighScoreName", "NoName");

        highScoreText.text = $"High Score: {highScore} ({highScoreName})";
    }
}
