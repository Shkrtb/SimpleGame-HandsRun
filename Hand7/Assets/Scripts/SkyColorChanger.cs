using UnityEngine;

public class SkyColorChanger : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterialA;
    [SerializeField] private Material skyboxMaterialB;
    [SerializeField] private Material skyboxMaterialC;

    private int num = 0;

    public float interval;

    void Start()
    {
        InvokeRepeating(nameof(ChangeSky), 0f, interval);
    }

    void ChangeSky()
    {
        if (num == 0)
        {
            RenderSettings.skybox = skyboxMaterialB;
            num = 1;
        }
        else if (num == 1)
        {
            RenderSettings.skybox = skyboxMaterialC;
            num = 2;
        }
        else
        {
            RenderSettings.skybox = skyboxMaterialA;
            num = 0;
        }
    }
}