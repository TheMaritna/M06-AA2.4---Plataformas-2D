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

    public GameObject miniCoinPrefab;
    public int maxCoinsToDrop = 30;
    public float dropSpeed = 10f;
    public float coinLifetime = 2f;
    public bool isRespawn;

    private int scoreBeforeDeath;

    void Start()
    {
        puntoAparicion = transform.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (uiManager.backCount != null)
            uiManager.backCount.gameObject.SetActive(false);
    }

    public void Dead()
    {
        if (!sr.enabled) return;

        scoreBeforeDeath = uiManager.GetScore();

        // Solución: Soltamos las monedas inmediatamente al morir
        ExplodeCoins(transform.position);
        AudioManager.instance.PlaySFX("Error", 1);
        StartCoroutine(SecuenciaMuerte());
    }

    private void ExplodeCoins(Vector3 spawnPos)
    {
        int half = scoreBeforeDeath / 2;
        int coinsToDrop = Mathf.Min(half, maxCoinsToDrop);

        uiManager.SetScore(scoreBeforeDeath - half);

        for (int i = 0; i < coinsToDrop; i++)
        {
            GameObject coin = Instantiate(miniCoinPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                coinRb.AddForce(randomDir * dropSpeed, ForceMode2D.Impulse);
            }

            Destroy(coin, coinLifetime);
        }
    }

    private IEnumerator SecuenciaMuerte()
    {
        rb.simulated = false;
        sr.enabled = false;
        isRespawn = true;

        uiManager.backCount.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            uiManager.ActualizarTexto(i.ToString());
            if (audiosCuentaAtras != null && i <= audiosCuentaAtras.Length)
                audioSource.PlayOneShot(audiosCuentaAtras[i - 1]);

            yield return new WaitForSeconds(0.33f);
        }

        uiManager.ActualizarTexto("");
        uiManager.backCount.gameObject.SetActive(false);

        transform.position = puntoAparicion;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        sr.enabled = true;
        rb.simulated = true;
        isRespawn = false;
    }

    public void ActualizarCheckpoint(Vector2 nuevaPosicion)
    {
        puntoAparicion = nuevaPosicion;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dead"))
            Dead();
    }
}