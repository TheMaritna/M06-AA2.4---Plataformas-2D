using UnityEngine;
using UnityEngine.UI;

public class UI_Visiblity : MonoBehaviour
{
    private Transform player;
    private Camera cam;
    private Image img;

    [Header("Config")]
    [Range(0f, 1f)]
    public float screenAxis = 0.5f;

    [Range(0f, 1f)]
    public float visibleAlpha = 1f;

    [Range(0f, 1f)]
    public float hiddenAlpha = 0.2f;

    [Header("Transition")]
    public float fadeSpeed = 5f;

    private void Start()
    {
        player = PlayerData.DATA.transform;
        cam = Camera.main;
        img = GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(player.position);

        float targetAlpha = viewportPos.x >= screenAxis ? visibleAlpha : hiddenAlpha;

        Color c = img.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        img.color = c;
    }
}