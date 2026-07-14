using UnityEngine;

public class WalkerAI : MonoBehaviour
{
    [Header("Patrol")]
    public float leftLimit;
    public float rightLimit;
    public float speed = 2f;

    private int direction = 1;

    void Update()
    {
        // Move walker
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        // Turn around at patrol limits
        if (transform.position.x >= rightLimit)
        {
            direction = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (transform.position.x <= leftLimit)
        {
            direction = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}