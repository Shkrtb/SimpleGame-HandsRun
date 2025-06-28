using UnityEngine;

public class WallsManager : MonoBehaviour
{
    private float destroyZ = -5f;

    private BackgroundsManager backgroundsManager;

    [System.Obsolete]
    private void Start()
    {
        // ƒV[ƒ“ã‚É1‚Â‚¾‚¯‚ ‚éBackgroundsManager‚ğŒŸõ
        backgroundsManager = FindObjectOfType<BackgroundsManager>();
        if (backgroundsManager == null)
        {
            Debug.LogError("BackgroundsManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñB");
        }
    }

    void Update()
    {
        if (backgroundsManager == null) return;

        float currentSpeed = backgroundsManager.speed;
        transform.Translate(0, 0, -currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
