using System.Collections;
using UnityEngine;

public class bossmanager : MonoBehaviour
{
    [SerializeField] float max_delay;
    [SerializeField] float min_delay;
    public GameObject player;
    public Animator animator;
    public bool busy;
    public bool slowdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    { 
        if (busy) return;
        decide();
    }
    void decide()
    {
        int choice = Random.Range(0, 4);
        switch (choice)
        {
            case 0:
                //if (player.transform.GetChild(1).localPosition.x == 2) return;
                bool isYes = Random.Range(0, 2) == 1;
                if (isYes)
                {
                    busy = true;
                    animator.SetTrigger("swipe right");
                }
                else
                {
                    busy = true;
                    animator.SetTrigger("swipe left");
                }
                break;
            case 1:
            //    if (player.GetComponent<PlayerManager>().ishidden) return;
                busy = true;
                animator.SetTrigger("projectile");
                break;
            case 2:
            //    if (player.GetComponent<PlayerManager>().candash&&!player.GetComponent<PlayerManager>().canpush) return;
                busy = true;
                animator.SetTrigger("arial attack");
                int choice2 = Random.Range(0, 3);
                switch (choice)
                {
                    case 0:
                        animator.SetTrigger("arial1"); break;
                    case 1:
                        animator.SetTrigger("arial2"); break;
                    case 2:
                        animator.SetTrigger("arial3"); break;
                }
                break;
            case 3:
                break;
        }
    }
    public IEnumerator statedone()
    {
        Debug.Log('c');
        slowdown = false;
        yield return new WaitForSeconds(Random.Range(min_delay,max_delay));
        busy = false;
    }
    public IEnumerator slow()
    {
        slowdown = true;
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
    }
}
