using UnityEngine;

public class persistentmanager : MonoBehaviour

{
    private static persistentmanager instance;

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
}