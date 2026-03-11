using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeSpeed = 2f;

    private TextMeshProUGUI text;
    private Color color;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        color = text.color;
    }

    void Update()
    {
        // mover hacia arriba
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // difuminar
        color.a -= fadeSpeed * Time.deltaTime;
        text.color = color;

        // destruir cuando desaparece
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}