using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    public Text scoreText;
    private float timer = 0f;
    private bool counting = false;

    void Start()
    {
        ScoreManager.Instance.ResetScore();
        Invoke(nameof(StartCounting), 5f); // 5秒後にスコア加算開始
    }

    void StartCounting()
    {
        counting = true;
    }

    void Update()
    {
        if (!counting) return;

        timer += Time.deltaTime;
        if (timer >= 0.2f)
        {
            ScoreManager.Instance.AddScore(1);
            timer = 0f;
        }

        scoreText.text = string.Format("{0}", ScoreManager.Instance.Score);
    }

    public void CollectItem()
    {
        ScoreManager.Instance.AddScore(10);
    }
}
