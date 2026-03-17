using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI backCount;
    public TextMeshProUGUI cookieCount;
    public TextMeshProUGUI timerText;
    public Transform cookiesCountTransform;
    public int score;

    [Header("Panels")]
    public GameObject finalPanel;
    public GameObject pausePanel;
    public GameObject uiPanel;

    [Header("Final Stats UI")]
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI finalCookiesText;
    public TextMeshProUGUI finalDeathsText;
    public TextMeshProUGUI finalRankText;
    public TextMeshProUGUI highScoreText;

    [Header("Rank Settings")]
    public float targetLevelTime = 60f;
    public int maxDeathsForPenalty = 10;

    private int totalCookiesInLevel;

    [Header("Animation Panel")]
    public Animator uiAnim;
    public float timeTextDelay;

    [Header("FlotingText")]
    public GameObject floatingTextPrefab;
    public Transform canvas;

    bool panelOpen = false;
    [HideInInspector]public bool levesHasEnd = false;
    bool levelTimeHasStart = false;

    float levelTime;
    int deaths;

    private void Start()
    {
        resetPanels();
        uiPanel.SetActive(true);
        levesHasEnd = false;
        totalCookiesInLevel = GameObject.FindGameObjectsWithTag("cookie").Length;
    }

    private void Update()
    {
        if(levelTimeHasStart)
            levelTime += Time.deltaTime;

        if (PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Move.IsPressed() || PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Crouch.IsPressed())
            levelTimeHasStart = true;

        PlayerData.DATA.cookies = score;

        if (PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Pause.WasPressedThisFrame())
        {
            panelOpen = !panelOpen;

            if (panelOpen)
                changePanel(pausePanel, true, true);
            else
                changePanel(uiPanel, true, false);
        }

        uiAnim.GetComponent<animSinycr>().isActive = uiPanel.active;

        if (PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Reset.WasPressedThisFrame())
        {
            TransitionManager.instance.ResetLevel();
        }
        timerText.text = "Time: " + levelTime.ToString("F2") + "s";
    }

    public void ActualizarTexto(string texto)
    {
        backCount.text = texto;
    }

    public void AddScore(int amount)
    {
        score += amount;
        cookieCount.text = score.ToString();
        AudioManager.instance.PlaySFX("Incremental", 1);
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        cookieCount.text = "0";
    }

    public void SetScore(int value)
    {
        score = value;
        cookieCount.text = score.ToString();
    }

    public void AddDeath()
    {
        deaths++;
    }

    public void FinishLevel()
    {
        StartCoroutine(endLevel());
        changePanel(finalPanel, true, true);

        finalTimeText.text = "Time: " + levelTime.ToString("F2") + "s";
        finalCookiesText.text = "Cookies: " + score;
        finalDeathsText.text = "Deaths: " + deaths;

        string rank = CalculateRank();
        finalRankText.text = "Rank: " + rank;

        string levelName = SceneManager.GetActiveScene().name;
        string key = "HIGHSCORE_TIME_" + levelName;

        float bestTime = PlayerPrefs.GetFloat(key, float.MaxValue);

        if (levelTime < bestTime)
        {
            bestTime = levelTime;
            PlayerPrefs.SetFloat(key, bestTime);
        }

        highScoreText.text = "Best Time: " + bestTime.ToString("F2") + "s";
    }

    string CalculateRank()
    {
        float cookiePercent = 0f;

        if (totalCookiesInLevel > 0)
            cookiePercent = (float)score / totalCookiesInLevel;

        cookiePercent = Mathf.Clamp01(cookiePercent);

        float timePercent = targetLevelTime / levelTime;
        timePercent = Mathf.Clamp01(timePercent);

        float deathPercent = 1f - ((float)deaths / maxDeathsForPenalty);
        deathPercent = Mathf.Clamp01(deathPercent);

        float finalScore =
            (cookiePercent * 0.5f) +
            (timePercent * 0.3f) +
            (deathPercent * 0.2f);

        if (finalScore >= 0.95f) return "S";
        if (finalScore >= 0.85f) return "A";
        if (finalScore >= 0.70f) return "B";
        if (finalScore >= 0.55f) return "C";
        if (finalScore >= 0.40f) return "D";

        return "F";
    }

    public void resetPanels()
    {
        pausePanel.SetActive(false);
        uiPanel.SetActive(false);
        finalPanel.SetActive(false);
    }

    public void changePanel(GameObject panel, bool state, bool animState)
    {
        resetPanels();
        StartCoroutine(animPanel(panel, state, animState));
    }
    public void SpawnText(string message, Vector3 position)
    {
        GameObject obj = Instantiate(floatingTextPrefab, canvas);
        obj.transform.position = position;

        obj.GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }

    public IEnumerator animPanel(GameObject panel, bool state, bool animState)
    {
        uiAnim.SetBool("Panel", animState);
        yield return new WaitForSeconds(timeTextDelay);
        panel.SetActive(state);
    }
    public IEnumerator endLevel()
    {
        yield return new WaitForSeconds(timeTextDelay);
        levesHasEnd = true;
    }
}