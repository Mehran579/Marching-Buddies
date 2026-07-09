using UnityEngine;
using UnityEngine.InputSystem;

public class switchbehaviour : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("clicked");
        
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);
            if(hit.collider == GetComponent<Collider2D>())
            {
                if (transform.localPosition.x == 2)
                {
                    Debug.Log("shouldn't switch");
                    return;
                }
                else
                {
                    Debug.Log("should switch" + name);
                    foreach (Transform child in transform.parent)
                    {
                        if (child.localPosition.x == 2)
                        {
                            Vector3 temp = transform.localPosition;
                            transform.localPosition = child.localPosition;
                            child.localPosition = temp;
                        }
                    }
                }
            }
        }
    }
}
