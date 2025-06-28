using UnityEngine;

public class BackgroundsManager : MonoBehaviour
{
    public float speed ;
    private float destroyZ = -80f;

    public float speedUpInterval;
    public float speedUpAmp;
    const float MIN = 0f;

    private void Start()
    {
        speedUpAmp = System.Math.Max(speedUpAmp, MIN);
        InvokeRepeating(nameof(SpeedUpBackgrounds), 10f, speedUpInterval);
    }

    void Update()
    {
        transform.Translate(0, 0, -speed * Time.deltaTime, Space.World);

        if (transform.position.z < destroyZ)
        {
            //Destroy(gameObject);
            transform.Translate(new Vector3(0f, 0f, 240f));
        }
    }

    void SpeedUpBackgrounds()
    {
        speed = speed + speedUpAmp;
    }
}
