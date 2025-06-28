using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    UdpClient udpClient;
    Thread receiveThread;
    bool isRunning = false;

    public int port = 12345;
    public string leftHandState = "0";
    public string rightHandState = "0";

    public GameObject leftSphere;
    public GameObject rightSphere;

    public AudioClip moveSFX;

    private float previousLeftTargetX = -1.75f;
    private float previousRightTargetX = 1.75f;

    void Start()
    {
        udpClient = new UdpClient(port);
        isRunning = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void OnDestroy()
    {
        isRunning = false;
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(); // スレッドを安全に終了
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }

    void Update()
    {
        // 毎フレーム最新メッセージに応じて球を動かす
        MoveLeftSphere(leftHandState);
        MoveRightSphere(rightHandState);
    }

    void MoveLeftSphere(string state)
    {
        Vector3 pos = leftSphere.transform.position;

        float targetX = pos.x;
        switch (state)
        {
            case "1": targetX = -1.75f; break;
            case "2": targetX = -4.75f; break;
            case "3": targetX = -1.75f; break;
            case "0": default: return;
        }

        // 前回のターゲットと違う場合だけSEを鳴らす
        if (Mathf.Abs(targetX - previousLeftTargetX) > 0.01f)
        {
            AudioManager.Instance.PlaySFX(moveSFX);
            previousLeftTargetX = targetX;
        }

        pos.x = Mathf.Lerp(pos.x, targetX, 0.5f);
        leftSphere.transform.position = pos;
    }

    void MoveRightSphere(string state)
    {
        Vector3 pos = rightSphere.transform.position;

        float targetX = pos.x;
        switch (state)
        {
            case "1": targetX = 1.75f; break;
            case "2": targetX = 4.75f; break;
            case "3": targetX = 1.75f; break;
            case "0": default: return;
        }

        // 前回のターゲットと違う場合だけSEを鳴らす
        if (Mathf.Abs(targetX - previousRightTargetX) > 0.01f)
        {
            AudioManager.Instance.PlaySFX(moveSFX);
            previousRightTargetX = targetX;
        }

        pos.x = Mathf.Lerp(pos.x, targetX, 0.5f);
        rightSphere.transform.position = pos;
    }


    void ReceiveData()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (isRunning)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                if (message.Length == 2)
                {
                    leftHandState = message[0].ToString();
                    rightHandState = message[1].ToString();
                    //Debug.Log($"Left: {leftHandState}, Right: {rightHandState}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("UDP Receive Error: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
