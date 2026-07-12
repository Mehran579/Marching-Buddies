using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class bossmanagerv2 : MonoBehaviour
{
    public bool isswiperight;
    bool busy;
    bool slowdown;
    bool followplayer;
    public GameObject player;
    public Animator animator;
    public Rigidbody2D righthand;
    public Rigidbody2D lefthand;
    private void Update()
    {
        if (busy) return;
        decide();
    }
    void decide()
    {
        busy = true;
        int choice = Random.Range(0, 4);
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
            case 3:
                animator.SetTrigger("projectile"); break;
        }
    }
    public IEnumerator endstage()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1f,2.5f));
        busy = false;
    }
    private void FixedUpdate()
    {
        if (!slowdown && !animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("swipe right") && Vector2.Distance(player.transform.position, righthand.position) < 20)
            {
                Debug.Log("called");
                StartCoroutine(slowdownroutine());
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("swipe left") && Vector2.Distance(player.transform.position, lefthand.position) < 20)
            {
                StartCoroutine(slowdownroutine());
            }
        }
        if (followplayer)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, player.transform.right,Time.fixedDeltaTime));
        }
    }
    #region Right swipe
    public IEnumerator rightswiperoutine() 
    {
        lefthand.gameObject.GetComponent<Collider2D>().enabled = false;
        righthand.gameObject.GetComponent<TrailRenderer>().enabled = true;
        righthand.gravityScale = 1f;
        float t = 0.5f;
        float gravity = righthand.gravityScale * Physics2D.gravity.y;
        righthand.linearVelocity = new Vector2((28.5f - righthand.transform.localPosition.x) / t, (-1.68667f - righthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        yield return new WaitForSeconds(t);
        righthand.gravityScale = 0f;
        righthand.linearVelocity = new Vector2(0, -50f);
        yield return new WaitForSeconds(1f);
        righthand.linearVelocity = new Vector2(-transform.right.x * 1000f, 0);
        yield return new WaitForSeconds(1f);
        righthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        float speed = Vector2.Distance(righthand.position, new Vector2(18.52628f, -4.68667f))/0.3f;
        while (Vector2.Distance(righthand.position, new Vector2(18.52628f, -4.68667f)) > 0.01f)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(18.52628f, -4.68667f), speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        righthand.linearVelocity = Vector2.zero;
        lefthand.gameObject.GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("exit");
    }
    #endregion
    #region Left swipe
    public IEnumerator leftswiperoutine()
    {
        righthand.gameObject.GetComponent<Collider2D>().enabled = false;
        lefthand.gameObject.GetComponent<TrailRenderer>().enabled = true;
        lefthand.gravityScale = 1f;
        float t = 0.5f;
        float gravity = lefthand.gravityScale * Physics2D.gravity.y;
        lefthand.linearVelocity = new Vector2((-28.5f - lefthand.transform.localPosition.x) / t, (-1.68667f - lefthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        yield return new WaitForSeconds(t);
        lefthand.gravityScale = 0f;
        lefthand.linearVelocity = new Vector2(0, -50f);
        yield return new WaitForSeconds(1f);
        lefthand.linearVelocity = new Vector2(transform.right.x * 1000f, 0);
        yield return new WaitForSeconds(1f);
        lefthand.gameObject.GetComponent<TrailRenderer>().enabled = false;
        float speed = Vector2.Distance(lefthand.position, new Vector2(-18.52628f, -4.68667f)) / 0.3f;
        while (Vector2.Distance(lefthand.position, new Vector2(-18.52628f, -4.68667f)) > 0.01f)
        {
            lefthand.MovePosition(Vector2.MoveTowards(lefthand.position, new Vector2(-18.52628f, -4.68667f), speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        lefthand.linearVelocity = Vector2.zero;
        righthand.gameObject.GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("exit");
    }
    #endregion
    #region Aerial Attack
    public IEnumerator aerialattack()
    {
        Debug.Log("called");
        while (Vector2.Distance(righthand.position, lefthand.position) >= 10.015f)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, lefthand.position, Vector2.Distance(righthand.position, lefthand.position)/0.5f * Time.fixedDeltaTime));
            lefthand.MovePosition(Vector2.MoveTowards(lefthand.position, righthand.position, Vector2.Distance(righthand.position, lefthand.position)/0.5f * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        lefthand.transform.SetParent(righthand.transform);
        lefthand.simulated = false;
        while (Vector2.Distance(righthand.position,new Vector2(righthand.position.x, righthand.position.x + 4f)) > 0)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(righthand.position.x, righthand.position.x + 4f),20*Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log("exited");
        for (int i = 0; i < 3; i++)
        {
            followplayer = true;
            yield return new WaitForSeconds(Random.Range(1, 3));
            followplayer = false;
            righthand.linearVelocity = new Vector2(0, 1000f);
        }
        int j = 0;
        
    }
    #endregion
    #region Slow routine
    public IEnumerator slowdownroutine()
    {
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
