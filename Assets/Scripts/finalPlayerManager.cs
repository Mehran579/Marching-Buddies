using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class finalPlayerManager : MonoBehaviour
{
    public Tilemap tilemap;
    bool tilescleared;
    //public GameObject[] glowingsprites;
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
        Debug.Log("jump called");
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
            swaplocations();
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && dashflag && candash)
        {
            if (canpush)
            {
                StartCoroutine(dash(dashduration / 2));
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
        //Debug.Log("give access to medsssssss1111");
        //globalvolume.profile.TryGet(out bloom);
        //DontDestroyOnLoad(this);
        //DontDestroyOnLoad(_camera);
        //DontDestroyOnLoad(c_camera);
        //DontDestroyOnLoad(globalvolume);
    }

    private void Update()
    {
        //if (transform.GetChild(0).localPosition.x == 2) //speed head
        //{
        //    candash = true;
        //    canpush = false;
        //    ishidden = false;
        //    ischarged = false;
        //    Jump = 15;
        //    MoveSpeed = speedboost;
        //    //bloom.intensity.value = 0;
        //    UnHide();
        //}
        //else if (transform.GetChild(1).localPosition.x == 2)  //jump head
        //{
        //    candash = false;
        //    canpush = false;
        //    ishidden = false;
        //    ischarged = false;
        //    Jump = jumpboost;
        //    MoveSpeed = 20;
        //    //bloom.intensity.value = 0;
        //    UnHide();
        //}
        //else if (transform.GetChild(2).localPosition.x == 2) //invisi head
        //{
        //    candash = false;
        //    canpush = false;
        //    ishidden = true;
        //    ischarged = false;
        //    Jump = 15;
        //    MoveSpeed = 20;
        //    //bloom.intensity.value = 0;
        //    Hide();
        //}
        //else if (transform.GetChild(3).localPosition.x == 2)     //strenght head
        //{
        //    candash = true;
        //    canpush = true;
        //    ishidden = false;
        //    ischarged = false;
        //    Jump = 15;
        //    MoveSpeed = 20;
        //    //bloom.intensity.value = 0;
        //    UnHide();
        //}
        //else if (transform.GetChild(4).localPosition.x == 2) //charge head
        //{
        //    candash = false;
        //    canpush = false;
        //    ishidden = false;
        //    ischarged = true;
        //    Jump = 15;
        //    MoveSpeed = 20;
        //    //bloom.intensity.value = 3;
        //    UnHide();
        //}
        //if (ischarged)
        //{
        //    foreach (GameObject p in glowingsprites)
        //    {
        //        p.SetActive(true);
        //    }
        //}
        //else
        //{
        //    foreach (GameObject p in glowingsprites)
        //    {
        //        p.SetActive(false);
        //    }
        //}
        if(gameObject.GetComponent<SpriteRenderer>().enabled == true&&!tilescleared)
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
                playerrb.gravityScale = 4f;
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
        Debug.Log(playerrb.linearVelocity.x);
        yield return null;
        Debug.Log(playerrb.linearVelocity.x);

        yield return null;
        Debug.Log(playerrb.linearVelocity.x);

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
    #region Head location swapper
    void swaplocations()
    {
        foreach (Transform child in transform)
        {
            if (child.localPosition.x == 2)
            {
                child.localPosition = new Vector2(-2, child.localPosition.y);
            }
            else
            {
                child.localPosition = new Vector2(child.localPosition.x + 1, child.localPosition.y);
            }
        }
    }
    #endregion
    #region Player Visibility
    void Hide()
    {
        foreach (Transform child in transform)
        {
            Color tempcolor = child.GetComponent<SpriteRenderer>().color;
            tempcolor.a = 0.3f;
            child.GetComponent<SpriteRenderer>().color = tempcolor;
        }
    }
    void UnHide()
    {
        foreach (Transform child in transform)
        {
            Color tempcolor = child.GetComponent<SpriteRenderer>().color;
            tempcolor.a = 1f;
            child.GetComponent<SpriteRenderer>().color = tempcolor;
        }
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
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("final point"))
    //    {
    //        //transform.position = Vector2.zero;
    //        GameObject.FindWithTag("final player").transform.position = collision.transform.position;
    //        GameObject newplayer = GameObject.FindWithTag("final player");
    //        c_camera.GetComponent<CinemachineCamera>().Follow = newplayer.transform;
    //        gameObject.SetActive(false);
    //        Destroy(collision.gameObject);
    //        //StartCoroutine(zoomout());
    //    }
    //}
    #endregion
}



