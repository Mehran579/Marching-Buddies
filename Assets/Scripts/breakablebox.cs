using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

        if (player == null)
            return;

        // Destroy if dashing or using strength, noo dash shouldn't be able to destroy you have to dash with the strenght active;
        if (player.isdashing && player.canpush)
        {
            Destroy(gameObject);
        }
    }
}
