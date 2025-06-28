using UnityEngine;

public class LightRotator : MonoBehaviour
{
    public Vector3 angleA = new Vector3(50f, 30f, 0f); // 初期角度
    public Vector3 angleB = new Vector3(10f, 60f, 0f); // 目標角度
    public float rotationDuration = 5f; // 回転にかける時間（秒）
    public float waitDuration = 30f;    // 各角度での待機時間（秒）

    private bool isRotating = false;
    private bool toB = true; // 次にどちらへ回転するか

    private void Start()
    {
        transform.eulerAngles = angleA;
        Invoke(nameof(StartRotation), waitDuration);
    }

    void StartRotation()
    {
        if (!isRotating)
        {
            Vector3 from = toB ? angleA : angleB;
            Vector3 to = toB ? angleB : angleA;
            StartCoroutine(RotateOverTime(from, to, rotationDuration));
        }
    }

    System.Collections.IEnumerator RotateOverTime(Vector3 from, Vector3 to, float duration)
    {
        isRotating = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.eulerAngles = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = to;
        toB = !toB; // 次の方向を切り替え
        isRotating = false;
        Invoke(nameof(StartRotation), waitDuration);
    }
}
