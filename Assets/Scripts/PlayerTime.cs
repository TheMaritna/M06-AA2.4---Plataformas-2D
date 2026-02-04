using UnityEngine;

public class PlayerTime : MonoBehaviour
{
    public float minTimeScale = 0.05f;
    public float maxTimeScale = 1.0f;
    public float velocityThreshold = 8f;
    public PlayerMovment2D player;
    public float timeValue;
    public static float TIME;

    private void Start()
    {
        if (player == null) player = GetComponent<PlayerMovment2D>();
    }

    private void Update()
    {
        if (player == null || player.rb == null) return;

        float currentVelocity = player.rb.linearVelocity.magnitude;
        timeValue = Mathf.Clamp01(currentVelocity / velocityThreshold);

        TIME = Mathf.Lerp(minTimeScale, maxTimeScale, timeValue);
        Debug.Log(TIME);
    }
}