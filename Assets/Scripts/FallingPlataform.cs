using System.Collections;
using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    private float fallDelay = 1f;
    private float destroyDeleay = 2f;
    private float regenDelay = 1f;
    private Rigidbody2D rb;
    private Vector3 Orposition;

    [Header("Info")]
    [SerializeField] public bool ItRegenerates;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Orposition = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDeleay);
        gameObject.GetComponent<SpriteRenderer>();
        if (ItRegenerates)
        {
            yield return new WaitForSeconds(regenDelay);
            rb.bodyType = RigidbodyType2D.Kinematic;
            Instantiate(gameObject, Orposition, Quaternion.identity);
        }
        Destroy(gameObject);
    }


}
