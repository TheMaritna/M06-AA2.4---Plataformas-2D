using UnityEngine;

public class OneWayPlataform : MonoBehaviour
{
    private Collider2D colsion;
    private void Start()
    {
        colsion = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           colsion.enabled = false; 
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            colsion.enabled = true;

        }
    }
}
