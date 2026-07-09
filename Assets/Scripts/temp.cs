using UnityEngine;
using UnityEngine.InputSystem;

public class temp : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            Vector2 world = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);

            Debug.Log(hit.collider ? hit.collider.name : "Nothing hit");
        }
    }
}
