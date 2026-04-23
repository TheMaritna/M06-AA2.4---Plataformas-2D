using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject ControlersPanel;
    public CanvasGroup mainCanvasGroup;
    private bool controlPanelOn;

    [Header("Camera Animation")]
    public Camera cam;
    public Transform targetPoint;
    public float moveDuration = 2f;
    public float zoomTarget = 3f;
    public float zoomDuration = 2f;

    [Header("Fade")]
    public float fadeDuration = 0.5f;

    private bool isPlaying;

    private void Start()
    {
        Time.timeScale = 1f;
        controlPanelOn = false;
        isPlaying = false;
        mainCanvasGroup.alpha = 1f;
    }

    private void Update()
    {
        ControlersPanel.SetActive(controlPanelOn);
    }

    public void ButtonPlay()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        yield return StartCoroutine(FadeCanvas(1f, 0f));

        Vector3 startPos = cam.transform.position;
        Quaternion startRot = cam.transform.rotation;

        Vector3 endPos = targetPoint.position;
        endPos.z = startPos.z;

        Quaternion endRot = targetPoint.rotation;

        float startSize = cam.orthographicSize;

        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0f, 1f, t / moveDuration);

            Vector3 newPos = Vector3.Lerp(startPos, endPos, lerp);
            newPos.z = startPos.z;

            cam.transform.position = newPos;
            cam.transform.rotation = Quaternion.Slerp(startRot, endRot, lerp);

            yield return null;
        }

        t = 0f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0f, 1f, t / zoomDuration);

            cam.orthographicSize = Mathf.Lerp(startSize, zoomTarget, lerp);

            yield return null;
        }

        SceneManager.LoadScene("L0");
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = t / fadeDuration;

            mainCanvasGroup.alpha = Mathf.Lerp(from, to, lerp);

            yield return null;
        }

        mainCanvasGroup.alpha = to;
    }

    public void ButtonControlers()
    {
        controlPanelOn = !controlPanelOn;
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}