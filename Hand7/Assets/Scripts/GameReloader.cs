using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class GameReloader : MonoBehaviour
{
    UdpClient udpClient;
    Thread receiveThread;
    bool isRunning = false;

    public int port = 12345;
    private string leftHandState = "0";
    private string rightHandState = "0";

    public Slider rockSlider;  // グー用
    public Slider paperSlider; // パー用
    public float holdThreshold = 3f;

    private float rockTimer = 0f;
    private float paperTimer = 0f;

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
        bool isBothRock = (leftHandState == "1" && rightHandState == "1");
        bool isBothPaper = (leftHandState == "2" && rightHandState == "2");

        // グー処理
        if (isBothRock)
        {
            rockSlider.transform.localScale = new Vector3(1.05f, 1.05f, 1f);
            rockTimer += Time.deltaTime;
            if (rockTimer >= holdThreshold)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        else
        {
            rockTimer = 0f;
        }

        // パー処理
        if (isBothPaper)
        {
            paperSlider.transform.localScale = new Vector3(1.05f, 1.05f, 1f);
            paperTimer += Time.deltaTime;
            if (paperTimer >= holdThreshold)
            {
                SceneManager.LoadScene("HomeScene");
            }
        }
        else
        {
            paperTimer = 0f;
        }

        // スライダー更新（0〜1）
        rockSlider.value = Mathf.Clamp01(rockTimer / holdThreshold);
        paperSlider.value = Mathf.Clamp01(paperTimer / holdThreshold);
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
            receiveThread.Abort(); // 非推奨だが一応保険
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
