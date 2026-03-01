using System.Collections;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Collect Effect")]
    public GameObject miniCoinPrefab;
    public int miniCoinsAmount = 6;
    public float spreadForce = 2f;
    public float flySpeed = 30f;
    public float randomFlyVariation = 15f;

    private Transform uiTarget;
    private int coinsArrived = 0;
    private PlayerMovment2D player;
    private bool collected = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        anim.speed = PlayerTime.TIME;
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        StartCoroutine(SpawnAndFly());
    }

    IEnumerator SpawnAndFly()
    {
        coinsArrived = 0;

        for (int i = 0; i < miniCoinsAmount; i++)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.08f));

            GameObject mini = Instantiate(miniCoinPrefab, transform.position, Quaternion.identity);
            StartCoroutine(MiniCoinRoutine(mini));
        }
    }

    IEnumerator MiniCoinRoutine(GameObject mini)
    {
        Vector3 randomDir = Random.insideUnitCircle * spreadForce;
        Vector3 startPos = mini.transform.position;
        Vector3 spreadPos = startPos + randomDir;

        float t = 0;

        while (t < 0.2f)
        {
            t += Time.deltaTime * 5f;
            mini.transform.position = Vector3.Lerp(startPos, spreadPos, t);
            yield return null;
        }

        float randomSpeed = flySpeed + Random.Range(-randomFlyVariation, randomFlyVariation);

        while (mini != null && Vector3.Distance(mini.transform.position, uiTarget.position) > 0.1f)
        {
            mini.transform.position = Vector3.MoveTowards(mini.transform.position, uiTarget.position, randomSpeed * Time.deltaTime);
            yield return null;
        }

        coinsArrived++;
        player.ui.AddScore(1);

        Destroy(mini);

        if (coinsArrived >= miniCoinsAmount)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMovment2D>();
            uiTarget = player.ui.cookieCount.transform;
            Collect();
        }
    }
}