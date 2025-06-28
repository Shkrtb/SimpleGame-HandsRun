using UnityEngine;
using UnityEngine.UI;

public class ResultScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    public GameObject nameInputPanel;
    public InputField nameInputField;
    public Button submitButton;

    public GameObject sceneTransitionController;

    [Header("ハイスコア時に非表示にするUI")]
    public GameObject[] uiToHideOnHighScore; // ScoreText, HighScoreText, Sliders, Texts などを登録

    private int lastScore;
    private int highScore;

    void Start()
    {
        lastScore = PlayerPrefs.GetInt("LastScore", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        scoreText.text = $"Result Score: {lastScore}";

        if (lastScore >= highScore)
        {
            // 新しいハイスコア！
            highScoreText.text = $"High Score: {lastScore} (New!)";
            nameInputPanel.SetActive(true);
            nameInputField.ActivateInputField();

            // UI を非表示
            SetUIActive(false);

            // Submit ボタンイベント登録
            submitButton.onClick.AddListener(SavePlayerName);

            if (sceneTransitionController != null)
                sceneTransitionController.SetActive(false);
        }
        else
        {
            string savedName = PlayerPrefs.GetString("HighScoreName", "NoName");
            highScoreText.text = $"High Score: {highScore} ({savedName})";
            nameInputPanel.SetActive(false);

            SetUIActive(true);

            if (sceneTransitionController != null)
                sceneTransitionController.SetActive(true);
        }
    }

    void SavePlayerName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("HighScoreName", playerName);
            PlayerPrefs.SetInt("HighScore", lastScore); // 念のため保存
            PlayerPrefs.Save();

            highScoreText.text = $"High Score: {lastScore} ({playerName})";
            nameInputPanel.SetActive(false);

            // UI を表示
            SetUIActive(true);

            if (sceneTransitionController != null)
                sceneTransitionController.SetActive(true);
        }
    }

    void SetUIActive(bool isActive)
    {
        // scoreText, highScoreText も含めて一括で制御
        scoreText.gameObject.SetActive(isActive);
        highScoreText.gameObject.SetActive(isActive);

        foreach (GameObject obj in uiToHideOnHighScore)
        {
            if (obj != null)
                obj.SetActive(isActive);
        }
    }
}
