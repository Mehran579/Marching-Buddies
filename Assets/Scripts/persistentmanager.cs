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
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "boss scene 3"&&!release && transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).SetParent(null);
            }
            release = true;
        }
    }
}