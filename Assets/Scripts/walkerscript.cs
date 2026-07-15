using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkerAI : MonoBehaviour
{
    [Header("Patrol")]
    public float leftLimit;
    public float rightLimit;
    public float speed = 2f;

    [Header("Detection")]
    public float visionDistance = 5f;

    private int direction = 1; // 1 = right, -1 = left
    private PlayerManager player;
    public Rigidbody2D rb;
    public float chaseplayer;
    public Collider2D _2ndwalker;
    public Collider2D _3rdwalker;

    void Start()
    {
        //player = FindObjectOfType<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _2ndwalker, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _3rdwalker, true);
    }

    private void Update()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.transform.GetChild(0).GetComponent<Collider2D>(), player.ishidden);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.transform.GetChild(1).GetComponent<Collider2D>(), player.ishidden);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.transform.GetChild(2).GetComponent<Collider2D>(), player.ishidden);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.transform.GetChild(3).GetComponent<Collider2D>(), player.ishidden);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.gameObject.transform.GetChild(4).GetComponent<Collider2D>(), player.ishidden);
        chaseplayer = DetectPlayer();
    }
    void FixedUpdate()
    {
        Patrol();
        //DetectPlayer();
    }

    void Patrol()
    {
        // Move
        //transform.position += Vector3.right * direction * speed * Time.deltaTime;
        // use rigidbody to move transform ignore physics

        //first will add the scenario when walker detects the player;
        if (chaseplayer == 1)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(player.transform.position.x - rb.position.x) * 10, rb.linearVelocity.y);
            transform.localScale = new Vector3(Mathf.Sign(player.transform.position.x - rb.position.x), 1, 1);
        }
        // Turn at right limit
        else if(chaseplayer == 0) 
        {
            //now the normal patrol of the walker;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            if (rb.position.x >= rightLimit)
            {
                direction = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }

        // Turn at left limit
            if (rb.position.x <= leftLimit)
            {
                direction = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if(chaseplayer == 2)      //new behaviour they run away if player is charged;
        {
            rb.linearVelocity = new Vector2(-(Mathf.Sign(player.transform.position.x - rb.position.x) * 2f), rb.linearVelocity.y);
            transform.localScale = new Vector3(-(Mathf.Sign(player.transform.position.x - rb.position.x)), 1, 1);
        }

    }

    float DetectPlayer()
    {
        if (player == null) return 0;

        if (player.ischarged) return 2;
        // Ignore player behind the walker
        if (direction == 1 && player.transform.position.x < transform.position.x)
            return 0;

        if (direction == -1 && player.transform.position.x > transform.position.x)
            return 0;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > visionDistance)
            return 0;

        // Invisible = ignore player
        if (player.ishidden)
            return 0;
        // Charge = destroy walker,,,,,,, no charge = destroy walker on contact;

        // All other powers = kill player, no all other power means now walker have to follow/attack the player kills happens through contact not through line of sight;
        Debug.Log("player detected");
        return 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.ishidden)
            {
                //allowplayer();
                return;
            }
            else if (player.ischarged)
            {
                Destroy(gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (direction == 1)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * visionDistance);
        else
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * visionDistance);
    }
}
