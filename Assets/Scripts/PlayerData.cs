using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData DATA;
    public int cookies;
    public bool hasKey;
    public UIManager uiManager;
    public PlayerMovment2D PC;

    void Awake()
    {
        DATA = this;
    }
    private void Start()
    {
        hasKey = false;
    }
}
