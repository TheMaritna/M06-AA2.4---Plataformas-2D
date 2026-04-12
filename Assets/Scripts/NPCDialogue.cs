using System.Collections;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Transform canvasTransform;

    [TextArea]
    public string message;

    public float typingSpeed = 0.03f;
    public float floatSpeed = 80f;
    public float fadeSpeed = 2f;

    public float delayBeforeFloat = 0.4f; // 👈 NUEVO

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SpawnText();
    }

    void SpawnText()
    {
        TMP_Text newText = Instantiate(dialogueText, canvasTransform);
        StartCoroutine(AnimateText(newText));
    }

    IEnumerator AnimateText(TMP_Text txt)
    {
        RectTransform rect = txt.GetComponent<RectTransform>();

        txt.text = "";
        txt.alpha = 1;

        foreach (char c in message)
        {
            txt.text += c;
            yield return new WaitForSeconds(typingSpeed);
            AudioManager.instance.PlaySFX("typiing", 1);
        }

        // ⏳ pequeño delay antes de subir
        yield return new WaitForSeconds(delayBeforeFloat);
        AudioManager.instance.PlaySFX("textUp", 1);

        while (txt.alpha > 0)
        {
            rect.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;
            txt.alpha -= Time.deltaTime * fadeSpeed;

            yield return null;
        }

        Destroy(txt.gameObject);
    }
}