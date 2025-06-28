using UnityEngine;
using UnityEngine.SceneManagement;

public class RightSphereCollision : MonoBehaviour
{
    //public AudioClip collisionSFX;
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("*");
        if (other.CompareTag("Wall"))
        {
            Debug.Log("ゲームオーバー！");
            ScoreManager.Instance.SaveScores();
            SceneManager.LoadScene("ResultScene"); // 遷移先のシーン名を指定
        }
    }
}
