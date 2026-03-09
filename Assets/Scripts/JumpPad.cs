using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad")]
    public float jumpadForce = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpadForce, ForceMode2D.Impulse);
        }
    }
}