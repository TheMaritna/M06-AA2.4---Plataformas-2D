using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad")]
    public float jumpadForce = 20f;

    [Header("Animation")]
    private Animator animator;
    private string animationTrigger = "Jump";
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetTrigger(animationTrigger);
            }

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.up * jumpadForce, ForceMode2D.Impulse);
        }
    }
}