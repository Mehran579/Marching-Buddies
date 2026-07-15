using UnityEngine;
using UnityEngine.SceneManagement;

public class persistentmanager : MonoBehaviour

{
    private static persistentmanager instance;
    bool release;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "boss scene 3") // Scene where you don't want it
        {
            Destroy(gameObject);
            instance = null;
        }
    }
}