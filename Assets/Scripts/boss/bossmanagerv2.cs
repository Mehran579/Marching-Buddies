using System.Collections;
using UnityEngine;

public class bossmanagerv2 : MonoBehaviour
{
    public Vector2 p0;
    public Vector2 p1;
    public Vector2 p2;
    public Vector2 p3;
    public Vector2 rihgthandinitialposition;
    public Vector2 leftthandinitialposition;
    public Transform righthandlandposition;
    public Transform lefthandlandposition;
    public bool isswiperight;
    bool busy;
    bool isshooting;
    bool slowdown;
    bool followplayer;
    bool isfalling;
    public GameObject projectile;
    public GameObject player;
    public Animator animator;
    public Rigidbody2D righthand;
    public Rigidbody2D lefthand;
    private void Update()
    {
        if (busy) return;
        decide();
    }
    private void Start()
    {
        player = GameObject.FindWithTag("final player");
        rihgthandinitialposition = righthand.transform.position;
        leftthandinitialposition = lefthand.transform.position;

    }
    void decide()
    {
        busy = true;
        int choice = Random.Range(0, 3);
        switch (choice)
        {
            case 0:
                animator.SetTrigger("swipe right"); 
                //Debug.Log("hehe");
                break;
            case 1:
                animator.SetTrigger("swipe left");
                //StartCoroutine(leftswiperoutine());
                break;
            case 2:
                animator.SetTrigger("air strike"); break;
            //case 3:
            //    animator.SetTrigger("projectile"); break;
        }
    }
    public IEnumerator endstage()
    {
        animator.SetTrigger("exit");
        yield return new WaitForSecondsRealtime(Random.Range(1f,2.5f));
        busy = false;
    }
    private void FixedUpdate()
    {
        if (!slowdown && !animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("swipe right") && Vector2.Distance(player.transform.position, righthand.position) < 20 && righthand.linearVelocity.x < 0)
            {
                StartCoroutine(slowdownroutine());
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("swipe left") && Vector2.Distance(player.transform.position, lefthand.position) < 20 && lefthand.linearVelocity.x > 0)
            {
                StartCoroutine(slowdownroutine());
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("aerial attack") && Vector2.Distance(player.transform.position, righthand.position) < 15 && isfalling && righthand.linearVelocity.y < 0)
            {
                Debug.Log(Vector2.Distance(player.transform.position, righthand .position));
                StartCoroutine (slowdownroutine());
            }
        }
        if (followplayer )
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(player.transform.position.x+5,righthand.position.y), 40 * Time.fixedDeltaTime));
        }
    }
    #region Right swipe
    public IEnumerator rightswiperoutine() 
    {
        //lefthand.gameObject.GetComponent<Collider2D>().enabled = false;
        righthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        StartCoroutine(ParabolaMove(righthand, righthandlandposition.position,1.5f,3f));
        //righthand.gravityScale = 1f;
        //float t = 0.5f;
        //float gravity = righthand.gravityScale * Physics2D.gravity.y;
        //righthand.linearVelocity = new Vector2((28.5f - righthand.transform.localPosition.x) / t, (-1.68667f - righthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        //yield return new WaitForSeconds(t);
        //righthand.gravityScale = 0f;
        //righthand.linearVelocity = new Vector2(0, -50f);
        yield return new WaitForSeconds(1f);
        righthand.gameObject.GetComponent<TrailRenderer>().enabled = true;
        righthand.linearVelocity = new Vector2(-transform.right.x * 500f, 0);
        yield return new WaitForSeconds(1f);
        righthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        float speed = Vector2.Distance(righthand.position, rihgthandinitialposition)/0.3f;
        while (Vector2.Distance(righthand.position, rihgthandinitialposition) > 0.01f)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, rihgthandinitialposition, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        righthand.linearVelocity = Vector2.zero;
        lefthand.gameObject.GetComponent<Collider2D>().enabled = true;
        statend();
    }
    #endregion
    #region Left swipe
    public IEnumerator leftswiperoutine()
    {
        //righthand.gameObject.GetComponent<Collider2D>().enabled = false;
        lefthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        StartCoroutine(ParabolaMove(lefthand, lefthandlandposition.position, 1.5f, 3f));

        //lefthand.gravityScale = 1f;
        //float t = 0.5f;
        //float gravity = lefthand.gravityScale * Physics2D.gravity.y;
        //lefthand.linearVelocity = new Vector2((-28.5f - lefthand.transform.localPosition.x) / t, (-1.68667f - lefthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        //yield return new WaitForSeconds(t);
        //lefthand.gravityScale = 0f;
        //lefthand.linearVelocity = new Vector2(0, -50f);
        yield return new WaitForSeconds(1f);
        lefthand.gameObject.GetComponent<TrailRenderer>().enabled = true;
        lefthand.linearVelocity = new Vector2(transform.right.x * 500f, 0);
        yield return new WaitForSeconds(1f);
        lefthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        float speed = Vector2.Distance(lefthand.position, leftthandinitialposition) / 0.3f;
        while (Vector2.Distance(lefthand.position, leftthandinitialposition) > 0.01f)
        {
            lefthand.MovePosition(Vector2.MoveTowards(lefthand.position, leftthandinitialposition, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        lefthand.linearVelocity = Vector2.zero;
        //righthand.gameObject.GetComponent<Collider2D>().enabled = true;
        statend();
    }
    #endregion
    IEnumerator ParabolaMove(Rigidbody2D rb, Vector2 endPos, float duration, float height)
    {
        Vector2 startPos = rb.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            // Linear movement
            Vector2 pos = Vector2.Lerp(startPos, endPos, t);

            // Add parabolic offset
            pos.y += 4f * height * t * (1f - t);

            rb.MovePosition(pos);

            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(endPos);
    }
    #region Aerial Attack
    public IEnumerator aerialattack()
    {
        righthand.GetComponent<TrailRenderer>().enabled = false;
        lefthand.GetComponent<TrailRenderer>().enabled = false;
        float maxhandsheight = 20;
        while (Vector2.Distance(righthand.position,new Vector2( lefthand.position.x,lefthand.linearVelocity.y+10)) >= 12)
        {
            while(Vector2.Distance(lefthand.position, new Vector2(righthand.position.x, righthand.linearVelocity.y + 10)) >= 12)
            {
                righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(lefthand.position.x, lefthand.linearVelocity.y + 10), Vector2.Distance(righthand.position, lefthand.position) / 0.5f * Time.fixedDeltaTime));
                lefthand.MovePosition(Vector2.MoveTowards(lefthand.position, new Vector2(righthand.position.x, righthand.linearVelocity.y + 10), Vector2.Distance(righthand.position, lefthand.position) / 0.5f * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
        }
        Debug.Log("isstuckhere");
        lefthand.transform.SetParent(righthand.transform);
        lefthand.simulated = false;
        while (Vector2.Distance(righthand.position,new Vector2(righthand.position.x, maxhandsheight)) > 0.01)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(righthand.position.x, maxhandsheight ), Vector2.Distance(righthand.position, new Vector2(righthand.position.x, maxhandsheight))/0.5f * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("exited");
        for (int i = 0; i < 3; i++)
        {
            followplayer = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            followplayer = false;
            isfalling = true;
            Debug.Log("falling");
            righthand.linearVelocity = new Vector2(0, -250);
            lefthand.GetComponent<TrailRenderer>().enabled = true;
            righthand.GetComponent<TrailRenderer>().enabled = true;
            yield return new WaitForSeconds(0.3f);
            righthand.linearVelocity = Vector2.zero;
            float speed = Vector2.Distance(lefthand.position, new Vector2(-18.52628f, -4.68667f)) / 0.3f;
            while (Vector2.Distance(righthand.position, new Vector2(righthand.position.x, maxhandsheight)) > 0.1)
            {
                Debug.Log("taking a breather here");
                righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(righthand.position.x, maxhandsheight), Vector2.Distance(righthand.position, new Vector2(righthand.position.x, maxhandsheight)) / 0.5f * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
            righthand.GetComponent<TrailRenderer>().enabled = false;
            lefthand.GetComponent<TrailRenderer>().enabled = false;
            
        }
        Debug.Log("Exited for loop");
        lefthand.simulated = true;
        lefthand.transform.SetParent(transform);
        float speedleft = Vector2.Distance(lefthand.position, leftthandinitialposition) / 0.3f;
        while (Vector2.Distance(lefthand.position, leftthandinitialposition) > 0.01f)
        {
            Debug.Log(Vector2.Distance(lefthand.position, new Vector2(-18.52628f, -4.68667f)));
            lefthand.MovePosition(Vector2.MoveTowards(lefthand.position, leftthandinitialposition, speedleft * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        float speedright = Vector2.Distance(righthand.position, rihgthandinitialposition) / 0.3f;
        while (Vector2.Distance(righthand.position, rihgthandinitialposition) > 0.01f)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, rihgthandinitialposition, speedright * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        statend();
    }
    #endregion
    #region Projectile
    public IEnumerator shootprojectile()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 2f));
        isshooting = true;
        Rigidbody2D p = Instantiate(projectile,new Vector2(0,10),Quaternion.identity).GetComponent<Rigidbody2D>();
        float t = 0f;
        float duration = 0.75f;
        p1 = Random.insideUnitCircle * 30f;
        p2 = Random.insideUnitCircle * 30f;
        p3 = player.transform.position;
        while (t < 1f)
        {
            float u = (1f - t);
            Debug.Log("runnig");
            t += Time.fixedDeltaTime / duration;
            t = Mathf.Clamp01(t);
            p.MovePosition(((u * u * u) * p0 + (3f * u * u * t) * p1 + (3f * u * t * t) * p2 + (t * t * t) * p3));
            yield return new WaitForFixedUpdate();
        }
        animator.GetComponent<bossmanagerv2>().StartCoroutine(animator.GetComponent<bossmanagerv2>().endstage());
        isshooting = false;
    }
    #endregion
    #region restart state
    void statend()
    {
        int choice = Random.Range(0, 3);
        switch (choice)
        {
            case 0:
                animator.GetComponent<bossmanagerv2>().StartCoroutine(animator.GetComponent<bossmanagerv2>().endstage());
                break;
            case 1:
                animator.GetComponent<bossmanagerv2>().StartCoroutine(animator.GetComponent<bossmanagerv2>().endstage());
                break;
            case 2:
                animator.SetTrigger("projectile");
                break;
        }
    }
    #endregion
    #region Slow routine
    public IEnumerator slowdownroutine()
    {
        Debug.Log("called slowmo");
        slowdown = true;
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.001f * Time.timeScale;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.001f;
        yield return new WaitForSeconds(2f);
        slowdown = false;
    }
    #endregion
}
