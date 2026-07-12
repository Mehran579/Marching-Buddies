using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class bossmanagerv2 : MonoBehaviour
{
    public bool isswiperight;
    bool busy;
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
                Debug.Log("hehe");
                break;
            case 1:
                animator.SetTrigger("swipe left"); break;
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
        if (isswiperight)
        {
        }
    }
    public IEnumerator rightswiperoutine() 
    {
        righthand.gravityScale = 1f;
        float t = 0.5f;
        float gravity = righthand.gravityScale * Physics2D.gravity.y;
        righthand.linearVelocity = new Vector2((28.5f - righthand.transform.localPosition.x) / t, (-1.68667f - righthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        yield return new WaitForSeconds(t);
        righthand.gravityScale = 0f;
        //righthand.linearVelocity = Vector2.zero;
        righthand.linearVelocity = new Vector2(0, -50f);
        //Debug.Log(righthand.linearVelocity);
        //Debug.Log(righthand.position);
        yield return new WaitForSeconds(1f);
        righthand.linearVelocity = new Vector2(-transform.right.x * 1000f, 0);
        yield return new WaitForSeconds(1f);
        //t = 0.2f;
        //gravity = righthand.gravityScale * Physics2D.gravity.y;
        //righthand.linearVelocity = new Vector2((18.52628f - righthand.transform.localPosition.x) / t, (-4.68667f - righthand.transform.localPosition.y - (0.5f * gravity * t * t)) / t);
        //righthand.MovePosition(vect)
        float speed = Vector2.Distance(righthand.position, new Vector2(18.52628f, -4.68667f))/0.3f;
        while (Vector2.Distance(righthand.position, new Vector2(18.52628f, -4.68667f)) > 0.01f)
        {
            righthand.MovePosition(Vector2.MoveTowards(righthand.position, new Vector2(18.52628f, -4.68667f), speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
        //yield return new WaitForSeconds(t);
        righthand.linearVelocity = Vector2.zero;
        animator.SetTrigger("exit");
    }
}
