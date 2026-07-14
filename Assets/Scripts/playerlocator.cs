using UnityEngine;
using UnityEngine.SceneManagement;

public class playerlocator : MonoBehaviour
{
    void Start()
    {
        if(SceneManager.GetActiveScene().name=="boss scene 3")
        {
            GameObject.FindWithTag("Player").transform.position = new Vector2(-24, -14);
        }
        else
        {
            GameObject.FindWithTag("Player").transform.position = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
