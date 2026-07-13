using UnityEngine;

public class turret : MonoBehaviour
{
    public LineRenderer lineRenderer;
    //public GameObject missile;
    public Transform startpoint;
    public Transform endpoint;
    private void Start()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Player") && !other.transform.parent.GetComponent<PlayerManager>().ishidden)
        {
            Debug.Log("why");
            //transform.localScale = new Vector3(-1, 1, 1);
            //GameObject nmissile = Instantiate(missile,transform.position,Quaternion.identity);
            //nmissile.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position).normalized * 14f, ForceMode2D.Impulse);

            lineRenderer.SetPosition(0, startpoint.position);
            lineRenderer.SetPosition(1, endpoint.position);
            lineRenderer.enabled = true;
            Invoke(nameof(restart), 0.4f);
        }
    }
    private void restart()
    {
        lineRenderer.enabled = false;
        GameObject.FindWithTag("Player").transform.position = Vector2.zero;
    }
}
