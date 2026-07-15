using UnityEngine;

public class floatingplatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    private Vector3 nextposition;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextposition = pointB.position;

        // transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 19f), 3f * Time.deltaTime);
        //  rb.linearVelocity = new Vector2(0f, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, nextposition, speed * Time.fixedDeltaTime));
        //transform.position = Vector3.MoveTowards(transform.position, nextposition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position,nextposition)<0.01)
        {
            nextposition = (nextposition == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
