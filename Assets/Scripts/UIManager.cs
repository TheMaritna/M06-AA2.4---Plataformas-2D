using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI backCount;
    public TextMeshProUGUI cookieCount;
    public Transform cookiesCountTransform;
    public int score;
    private void Update()
    {
        PlayerData.DATA.cookies = score;
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
}