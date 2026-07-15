using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

        if (player == null)
            return;

        // Destroy if dashing or using strength
        if (player.isdashing || player.canpush)
        {
            Destroy(gameObject);
        }
    }
}
