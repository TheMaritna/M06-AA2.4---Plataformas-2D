using UnityEngine;
using System.Collections;

public class HelthSystem : MonoBehaviour
{
    [Header("Stats")]
    public float MaxLife;
    public float ActualLife;
    public float hitInpact = 2f;
    public bool isDead;

    public SpriteRenderer spriteRenderer;
    private Color original;
    private bool isPlayer;
    public float lifes = 3f;

    [Header("Invulnerability")]
    public bool isInvulnerable = false;
    public float invulnerabilityTime = 1.2f;
    public float flashSpeed = 0.1f;

    private void Awake()
    {
        if(spriteRenderer == null)
        spriteRenderer = GetComponent<SpriteRenderer>();

        original = spriteRenderer.color;
    }
    private void Start()
    {
        ActualLife = MaxLife;
        
        
            


        isPlayer = (gameObject.tag == "Player");
        isDead = false;
    }

    private void Update()
    {
        if (ActualLife <= 0)
            Die();

        if (lifes <= 0 && isPlayer)
            EndGame();
    }

    public void Die()
    {
        if (isPlayer)
        {
            if (lifes > 0)
            {
                GetComponent<Controlers>().Vibrate(0.8f, 0.8f, 0.2f);
            }

            lifes -= 1;
            ActualLife = MaxLife;
        }
        else
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    public void Hit(float Damage)
    {
        if (isInvulnerable) return;

        ActualLife -= Damage;

        StartCoroutine(HitFeedbackRoutine());
        if (isPlayer)
            StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator HitFeedbackRoutine()
    {

        spriteRenderer.color = Color.red;
        if(!isPlayer)
            spriteRenderer.color = Color.white;

        if (isPlayer && ActualLife > 0)
            GetComponent<Controlers>().Vibrate(0.8f, 0.8f, 0.1f);

        if (isPlayer)
            GetComponent<PlayerMovment2D>().InpulsFeedBack(hitInpact, 0.15f);

        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = original;
        if (isPlayer)
            spriteRenderer.color = Color.white;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float timer = 0f;

        while (timer < invulnerabilityTime)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);

            timer += flashSpeed * 2f;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    public void EndGame()
    {
        Debug.Log("Player Die");
        GetComponent<Controlers>().Vibrate(1f, 1f, 1f);
        isDead = true;
    }
}
