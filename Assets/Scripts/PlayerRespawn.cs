using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 puntoAparicion;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    public UIManager uiManager;
    public AudioClip[] audiosCuentaAtras;

    void Start()
    {
        puntoAparicion = transform.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        uiManager.backCount.gameObject.SetActive(false);
    }

    public void ActualizarCheckpoint(Vector2 nuevaPosicion)
    {
        puntoAparicion = nuevaPosicion;
    }

    public void Dead()
    {
        StartCoroutine(SecuenciaMuerte());
    }

    private IEnumerator SecuenciaMuerte()
    {
        rb.simulated = false;
        sr.enabled = false;
        transform.position = puntoAparicion;
        uiManager.backCount.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            uiManager.ActualizarTexto(i.ToString());

            if (audiosCuentaAtras.Length >= i && audioSource != null)
            {
                audioSource.PlayOneShot(audiosCuentaAtras[i - 1]);
            }

            yield return new WaitForSeconds(0.33f);
        }

        uiManager.ActualizarTexto("");
        uiManager.backCount.gameObject.SetActive(false);

        transform.position = puntoAparicion;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        sr.enabled = true;
        rb.simulated = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dead"))
        {
            Dead();
        }
    }
}