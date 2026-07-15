using System.Collections;
using Unity.Cinemachine;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class finalPlayerManager : MonoBehaviour
{
    //public List <Vector3> headslocalposition = new List<Vector3>();
    //public List <GameObject> stances = new List<GameObject>();
    //public GameObject[] stances;
    [Header("Health")]
    bool canhit = true;
    int health = 3;
    public GameObject[] playerhealth;
    public Animator bossAnimator;
    public GameObject _camera;

    public Slider enemyhealth;
    public float hitstopduration;

    public Tilemap tilemap;
    bool tilescleared;
    bool showhead;
    GameObject currentstance;
    public GameObject glowingsprite;
    //public GameObject _camera;
    //public GameObject c_camera;
    public Rigidbody2D playerrb;
    public Volume globalvolume;
    private Bloom bloom;
    [Header("Stats")]
    public float MoveSpeed;
    public float Jump;
    [Header("Abilities Active")]
    public bool candash;
    public bool canpush;
    //public bool candoublejump;
    public bool ishidden;
    public bool ischarged;
    public float speedboost;
    public float jumpboost;
    [Header("Dash")]
    public float dashSpeed;
    public float dashduration;
    public float dashcooldown;
    public bool isdashing;
    bool dashflag = true;
    [Header("Coyote time")]
    public float coyotetime;
    float coyotetimer;
    [Header("Jump input buffer")]
    public float jumpinput_time;
    float jumpinput_timer;
    [Space(10)]
    Vector2 movement;
    [Header("Ground Check")]
    public float raycastradius;
    public float castdistance;
    public LayerMask groundlayer;




    #region Player Inputs
    public void OnMove(InputAction.CallbackContext context)       //Movement input
    {
        movement = context.ReadValue<Vector2>();
        if (context.performed)
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }
    }
    public void OnJump(InputAction.CallbackContext context)       //Jump Input
    {
        //Debug.Log("jump called");
        if (context.performed)
        {
            jumpinput_timer = jumpinput_time;
            if (coyotetimer > 0)
            {
                jump();
            }
        }
        else if (context.canceled && playerrb.linearVelocity.y > 0)
        {
            playerrb.linearVelocity = new Vector2(playerrb.linearVelocity.x, playerrb.linearVelocity.y * 0.5f);
            coyotetimer = 0;
        }
    }
    public void OnShift(InputAction.CallbackContext context)
    {
        if (isdashing) return;
        if (context.performed)
        {
            showstances();
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && dashflag && candash)
        {
            if (canpush)
            {
                StartCoroutine(dash(dashduration / 2.5f));
            }
            else
            {
                StartCoroutine(dash(dashduration));
            }
        }
    }
    #endregion
    private void Start()
    {
        _camera.SetActive(true);
        //foreach (Transform child in transform)
        //{
        //    //Debug.Log("disabling");
        //    child.gameObject.SetActive(false);
        //    //if(child.gameObject.name != "glowsprite")
        //    //{
        //        //headslocalposition.Add(child.position);
        //        //stances.Add(child.gameObject);
        //    //}
        //}
        //Debug.Log("give access to medsssssss1111");
        //globalvolume.profile.TryGet(out bloom);
        //DontDestroyOnLoad(this);
        //DontDestroyOnLoad(_camera);
        //DontDestroyOnLoad(c_camera);
        //DontDestroyOnLoad(globalvolume);
    }

    private void Update()
    {
        //if(Mathf.Sign(movement.x) > 0)
        //{
        //    for (int i = 0; i < stances.Count; i++)
        //    {
        //        GameObject child = stances[i];
        //        Vector3 pos = headslocalposition[i];

        //        child.transform.localPosition = pos;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < stances.Count; i++)
        //    {
        //        GameObject child = stances[i];
        //        Vector3 pos = headslocalposition[i];

        //        child.transform.localPosition = new Vector3(-pos.x,pos.y,pos.z);
        //    }
        //}
        if (Mouse.current.leftButton.wasPressedThisFrame && showhead)
        {
            //Debug.Log("disabling in update");
            Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);
            foreach(Transform child in transform)
            {
                if (child.name == "glowsprite") continue;
                if ( hit.collider!=null && hit.collider.name == child.name)
                {
                    child.localScale = new Vector3(6.5f, 6.5f, 6.5f );
                }
                else
                {
                    child.localScale = new Vector3(5, 5, 5);
                }
            }
            showstances();
        }
        if (transform.GetChild(4).localScale.x == 6.5f) //speed head
        {
            candash = true;
            canpush = false;
            ishidden = false;
            ischarged = false;
            Jump = 30;
            MoveSpeed = speedboost;
            //bloom.intensity.value = 0;
            UnHide();
        }
        else if (transform.GetChild(0).localScale.x == 6.5f)  //jump head
        {
            candash = false;
            canpush = false;
            ishidden = false;
            ischarged = false;
            Jump = jumpboost;
            MoveSpeed = 30;
            //bloom.intensity.value = 0;
            UnHide();
        }
        else if (transform.GetChild(1).localScale.x == 6.5f) //invisi head
        {
            candash = false;
            canpush = false;
            ishidden = true;
            ischarged = false;
            Jump = 30;
            MoveSpeed = 30;
            //bloom.intensity.value = 0;
            Hide();
        }
        else if (transform.GetChild(2).localScale.x == 6.5f)     //strenght head
        {
            candash = true;
            canpush = true;
            ishidden = false;
            ischarged = false;
            Jump = 30;
            MoveSpeed = 30;
            //bloom.intensity.value = 0;
            UnHide();
        }
        else if (transform.GetChild(3).localScale.x == 6.5f) //charge head
        {
            candash = false;
            canpush = false;
            ishidden = false;
            ischarged = true;
            Jump = 30;
            MoveSpeed = 30;
            //bloom.intensity.value = 3;
            UnHide();
        }
        if (ischarged)
        {
            glowingsprite.SetActive(true);
        }
        else
        {
            glowingsprite.SetActive(false);
        }
        if (gameObject.GetComponent<SpriteRenderer>().enabled == true&&!tilescleared)
        {
            for (int x = -73; x < 83; x++)
            {
                for (int y = -6; y < 49; y++)
                {
                    //Debug.Log(new Vector3Int(x, y));
                    tilemap.SetTile(new Vector3Int(x,y),null);
                }
            }
            tilescleared = true;
        }
        if(health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void FixedUpdate()
    {
        if (isdashing) return;
        playerrb.linearVelocity = new Vector2(movement.x * MoveSpeed, playerrb.linearVelocity.y);
        if (isGrounded())
        {
            coyotetimer = coyotetime;
            if (jumpinput_timer > 0)
            {
                jump();
            }
        }
        else if (!isGrounded())
        {
            coyotetimer -= Time.deltaTime;
            if (playerrb.linearVelocity.y < 0)
            {
                playerrb.gravityScale = 20f;
            }
            else
            {
                playerrb.gravityScale = 7f;
            }
        }
        jumpinput_timer -= Time.deltaTime;
    }
    void jump()
    {
        coyotetimer = 0;
        jumpinput_timer = 0;
        playerrb.linearVelocity = new Vector2(playerrb.linearVelocity.x, Jump);

    }
    #region Dash
    IEnumerator dash(float dashduration)
    {
        isdashing = true;
        dashflag = false;
        playerrb.linearVelocity = new Vector2(dashSpeed * transform.localScale.x, playerrb.linearVelocity.y);
        float oggraivty = playerrb.gravityScale;
        playerrb.gravityScale = 0;
        //Debug.Log(playerrb.linearVelocity.x);
        yield return null;
        //Debug.Log(playerrb.linearVelocity.x);

        yield return null;
        //Debug.Log(playerrb.linearVelocity.x);

        yield return new WaitForSeconds(dashduration);
        playerrb.linearVelocity = new Vector2(0, playerrb.linearVelocity.y);
        isdashing = false;
        playerrb.gravityScale = oggraivty;
        yield return new WaitForSeconds(dashcooldown);
        dashflag = true;
    }
    #endregion
    #region Ground Check
    public bool isGrounded()
    {
        if (Physics2D.CircleCast(transform.position, raycastradius, -transform.up, castdistance, groundlayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - transform.up * castdistance, raycastradius);
    }
    #endregion
    #region Stance swapper
    void showstances()
    {
        showhead = !showhead;
        foreach (Transform child in transform)
        {
            if (child.name == "glowsprite") continue;
            child.gameObject.SetActive(showhead);
        }
    }
    #endregion
    #region Player Visibility
    void Hide()
    {
            Color tempcolor = GetComponent<SpriteRenderer>().color;
            tempcolor.a = 0.3f;
            GetComponent<SpriteRenderer>().color = tempcolor;
     
    }
    void UnHide()
    {
            Color tempcolor = GetComponent<SpriteRenderer>().color;
            tempcolor.a = 1f;
            GetComponent<SpriteRenderer>().color = tempcolor;
    }
    #endregion
    #region Player Collisions
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("obs"))
    //    {
    //        if (canpush)
    //        {
    //            collision.gameObject.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    //        }
    //        else
    //        {
    //            collision.gameObject.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX;
    //        }
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("obs"))
    //    {
    //        collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    //    }
    //}1`
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("enemy head child is ",bossAnimator.transform.GetChild(0));
        if (!canhit) return;
        if (collision.CompareTag("boss hands")&&collision.transform != bossAnimator.transform.GetChild(0))
        {
            //Debug.Log("damage taken");
            hitstop(hitstopduration);
            health -= 1;
            Destroy(playerhealth[health]);
        }
        if(collision.gameObject == bossAnimator.transform.GetChild(0).gameObject && ischarged)
        {
            Debug.Log("bosshit");
            enemyhealth.value -= 1;
        }
        if(collision.CompareTag("boss_projectile"))
        {
            Destroy(collision.gameObject, 0.2f);
            if (!ishidden)
            {
                health -= 1;
                Destroy(playerhealth[health]);
            }
        }
    }
    #endregion
    #region HIt stop
    public IEnumerator hitstop(float duration)
    {
        float fixeddelta = Time.fixedDeltaTime;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixeddelta;
    }
    #endregion
    #region hitcooldown
    public IEnumerator hitcooldown()
    {
        canhit = true;
        yield return new WaitForSecondsRealtime(1f);
        canhit = false;
    }
    
    #endregion
}



