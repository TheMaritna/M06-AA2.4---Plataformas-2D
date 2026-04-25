using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject levelsPanel;
    public GameObject controllersPanel;
    public GameObject creditsPanel;

    public CanvasGroup mainCanvasGroup;

    private GameObject currentPanel;

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
        isPlaying = false;
        mainCanvasGroup.alpha = 1f;

        CloseAllPanels();
    }

    void CloseAllPanels()
    {
        levelsPanel.SetActive(false);
        controllersPanel.SetActive(false);
        creditsPanel.SetActive(false);
        currentPanel = null;
    }

    void OpenPanel(GameObject panel)
    {
        if (currentPanel == panel)
        {
            panel.SetActive(false);
            currentPanel = null;
            return;
        }

        CloseAllPanels();
        panel.SetActive(true);
        currentPanel = panel;
    }

    // BOTONES UI
    public void ButtonLevels()
    {
        OpenPanel(levelsPanel);
    }

    public void ButtonControllers()
    {
        OpenPanel(controllersPanel);
    }

    public void ButtonCredits()
    {
        OpenPanel(creditsPanel);
    }

    public void ButtonPlay(int Level)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            if (Level == 0)
                StartCoroutine(PlayAnimation(Level.ToString()));
            else
                TransitionManager.instance.LoadScene("L" + Level.ToString());

        }
    }

    IEnumerator PlayAnimation(string LevelNum)
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

        TransitionManager.instance.LoadScene("L" + LevelNum);
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

    public void ButtonExit()
    {
        Application.Quit();
    }
}