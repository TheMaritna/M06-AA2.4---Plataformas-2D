using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData DATA;
    public int cookies;
    public bool hasKey;
    public UIManager uiManager;
    private void Start()
    {
        DATA = GetComponent<PlayerData>();
        hasKey = false;
    }
}
