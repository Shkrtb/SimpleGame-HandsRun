using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class GameStarter : MonoBehaviour
{
    UdpClient udpClient;
    Thread receiveThread;
    bool isRunning = false;

    public int port = 12345;
    private string leftHandState = "0";
    private string rightHandState = "0";

    public Slider progressSlider;  // アタッチするSlider
    public float holdThreshold = 3f;

    private float holdTimer = 0f;

    void Start()
    {
        udpClient = new UdpClient(port);
        isRunning = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void Update()
    {
        if (leftHandState == "1" && rightHandState == "1")
        {
            progressSlider.transform.localScale = new Vector3(1.05f, 1.05f, 1f);
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdThreshold)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        else
        {
            progressSlider.transform.localScale = new Vector3(1f, 1f, 1f);
            holdTimer = 0f;
        }

        // Sliderの値を更新（0〜1）
        progressSlider.value = Mathf.Clamp01(holdTimer / holdThreshold);
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
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("UDP Receive Error: " + e.Message);
            }
        }
    }

    void OnDestroy()
    {
        isRunning = false;
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();
        }
        if (udpClient != null)
        {
            udpClient.Close();
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
