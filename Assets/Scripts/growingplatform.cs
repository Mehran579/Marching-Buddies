using UnityEngine;

public class growingplatform : MonoBehaviour
{
    Vector3 minScale;
    public Vector3 maxScale = new Vector3(4,10,1);
    public float speed = 10f;
    private void Start()
    {
        minScale = transform.localScale;
    }
    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.localScale = Vector3.Lerp(minScale, maxScale, t);
    }
}
