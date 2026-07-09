using UnityEngine;

public class playerlocator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
