using System.Collections;
using UnityEngine;
using TMPro;

public class CinematicSystem : MonoBehaviour
{
    [System.Serializable]
    public class CinematicStep
    {
        public Transform cameraTarget;
        public float moveTime = 2f;

        [TextArea(2, 4)]
        public string dialogText;

        public float textDuration = 3f;
    }

    public CinematicStep[] steps;

    [Header("References")]
    public Transform cameraTransform;
    public TextMeshProUGUI dialogText;

    [Header("Settings")]
    public float typingSpeed = 0.03f;

    private void Start()
    {
        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            CinematicStep step = steps[i];

            yield return StartCoroutine(MoveCamera(step.cameraTarget.position, step.moveTime));

            if (step.dialogText != "")
            {
                yield return StartCoroutine(TypeText(step.dialogText));
                yield return new WaitForSeconds(step.textDuration);
            }
        }

        dialogText.text = "";
    }

    IEnumerator MoveCamera(Vector3 targetPos, float time)
    {
        Vector3 startPos = cameraTransform.position;
        float elapsed = 0;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            cameraTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / time);
            yield return null;
        }

        cameraTransform.position = targetPos;
    }

    IEnumerator TypeText(string text)
    {
        dialogText.text = "";

        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}