using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;
    //public AudioSource sfxSource;

    public AudioClip homeBGM;
    public AudioClip gameBGM;
    public AudioClip resultBGM;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!bgmSource.isPlaying)
        {
            Debug.LogWarning("BGMÇ™çƒê∂Ç≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    void PlayBGMForScene(string sceneName)
    {
        AudioClip clip = null;
        switch (sceneName)
        {
            case "HomeScene": clip = homeBGM; break;
            case "GameScene": clip = gameBGM; break;
            case "ResultScene": clip = resultBGM; break;
        }

        if (clip != null && bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // àÍéûìIÇ»AudioSourceïtÇ´GameObjectÇê∂ê¨
        GameObject tempGO = new GameObject("sfxAudio");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        //aSourceì‡ê›íËí«â¡ÇÕÇ±Ç±

        aSource.Play();

        // çƒê∂èIóπå„Ç…é©ìÆÇ≈çÌèú
        Destroy(tempGO, clip.length);
    }
}
