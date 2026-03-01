using UnityEngine;

public class damageArea : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteDefault;
    public Sprite spriteAdvertencia;
    public Sprite spriteAtaque;

    [Header("Tiempos")]
    public float tiempoDefault = 2f;
    public float tiempoAdvertencia = 2f;
    public float tiempoAtaque = 1.5f;

    public float velocidadParpadeo = 0.15f;
    public float daño = 1f;

    private SpriteRenderer sr;

    private float timer = 0f;
    private float blinkTimer = 0f;

    private enum Estado { Default, Advertencia, Ataque }
    private Estado estadoActual;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CambiarEstado(Estado.Default);
    }

    void Update()
    {
        float delta = Time.deltaTime * PlayerTime.TIME;
        timer += delta;

        switch (estadoActual)
        {
            case Estado.Default:
                if (timer >= tiempoDefault)
                {
                    CambiarEstado(Estado.Advertencia);
                }
                break;

            case Estado.Advertencia:
                blinkTimer += delta;

                if (blinkTimer >= velocidadParpadeo)
                {
                    blinkTimer = 0f;
                    sr.sprite = sr.sprite == spriteDefault ? spriteAdvertencia : spriteDefault;
                }

                if (timer >= tiempoAdvertencia)
                {
                    CambiarEstado(Estado.Ataque);
                }
                break;

            case Estado.Ataque:
                if (timer >= tiempoAtaque)
                {
                    CambiarEstado(Estado.Default);
                }
                break;
        }
    }

    void CambiarEstado(Estado nuevoEstado)
    {
        estadoActual = nuevoEstado;
        timer = 0f;
        blinkTimer = 0f;

        switch (estadoActual)
        {
            case Estado.Default:
                sr.sprite = spriteDefault;
                break;

            case Estado.Advertencia:
                sr.sprite = spriteDefault;
                break;

            case Estado.Ataque:
                sr.sprite = spriteAtaque;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (estadoActual == Estado.Ataque && other.CompareTag("Player"))
        {
            HelthSystem health = other.GetComponent<HelthSystem>();
            if (health != null)
            {
                health.Hit(daño);
            }
        }
    }
}