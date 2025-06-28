using UnityEngine;

public class SkyboxFader : MonoBehaviour
{
    public Material blendSkyboxMaterial;
    public Cubemap[] skyboxCubemaps; // 2つの空テクスチャを Cubemap 形式で入れる
    public float fadeDuration ;
    public float interval ;

    private int currentIndex = 0;
    private int nextIndex => (currentIndex + 1) % skyboxCubemaps.Length;
    private float fadeTimer = 0f;
    private bool isFading = false;

    void Start()
    {
        RenderSettings.skybox = blendSkyboxMaterial;
        blendSkyboxMaterial.SetTexture("_Skybox1", skyboxCubemaps[currentIndex]);
        blendSkyboxMaterial.SetTexture("_Skybox2", skyboxCubemaps[nextIndex]);
        blendSkyboxMaterial.SetFloat("_Blend", 0f);

        InvokeRepeating(nameof(StartFade), interval, interval + fadeDuration);
    }

    void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float blendValue = Mathf.Clamp01(fadeTimer / fadeDuration);
            blendSkyboxMaterial.SetFloat("_Blend", blendValue);

            if (blendValue >= 1f)
            {
                // 完了
                currentIndex = nextIndex;
                isFading = false;
                fadeTimer = 0f;

                blendSkyboxMaterial.SetTexture("_Skybox1", skyboxCubemaps[currentIndex]);
                blendSkyboxMaterial.SetTexture("_Skybox2", skyboxCubemaps[nextIndex]);
                blendSkyboxMaterial.SetFloat("_Blend", 0f);
            }
        }
    }

    void StartFade()
    {
        isFading = true;
        fadeTimer = 0f;
    }
}
