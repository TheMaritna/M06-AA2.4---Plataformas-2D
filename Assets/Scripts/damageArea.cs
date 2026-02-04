using UnityEngine;
using System.Collections;

public class damageArea : MonoBehaviour
{
    public Color colorSeguro = Color.green;
    public Color colorPeligro = Color.red;
    public Color colorHit = Color.white;
    public float tiempoAdvertencia = 3f;
    public float tiempoActivo = 2f;
    public float daño = 1f;

    private SpriteRenderer sr;
    private bool estaActivo = false;
    private bool mostrandoHit = false;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mostrandoHit) return;

        timer += Time.deltaTime * PlayerTime.TIME;

        if (!estaActivo)
        {
            float progreso = timer / tiempoAdvertencia;
            sr.color = Color.Lerp(colorSeguro, colorPeligro, progreso);

            if (timer >= tiempoAdvertencia)
            {
                estaActivo = true;
                timer = 0f;
                sr.color = colorHit;
            }
        }
        else
        {
            if (timer >= tiempoActivo)
            {
                estaActivo = false;
                timer = 0f;
                sr.color = colorHit;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (estaActivo && other.CompareTag("Player"))
        {
            HelthSystem health = other.GetComponent<HelthSystem>();
            if (health != null)
            {
                health.Hit(daño);
            }
        }
    }

}