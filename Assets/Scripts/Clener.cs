using UnityEngine;

public class Clener : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(collision.gameObject);
        }
    }
}
