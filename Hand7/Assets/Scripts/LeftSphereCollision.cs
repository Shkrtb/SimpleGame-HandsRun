using UnityEngine;
using UnityEngine.SceneManagement;
public class LeftSphereCollision : MonoBehaviour
{
    //public AudioClip collisionSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("ゲームオーバー！");
            ScoreManager.Instance.SaveScores();
            SceneManager.LoadScene("ResultScene"); // 遷移先のシーン名を指定
        }
    }
}
