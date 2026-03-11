using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int LevelToLoad = 0;
    private GameObject player;
    private void Update()
    {
        if(player!=null)
            player.transform.position = transform.position;
        if (PlayerData.DATA.uiManager.levesHasEnd && PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Jump.WasPressedThisFrame())
            nextScene();
        else if (PlayerData.DATA.uiManager.levesHasEnd && PlayerData.DATA.GetComponent<PlayerMovment2D>().input.Player.Crouch.WasPressedThisFrame())
            resetScene();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerData.DATA.hasKey)
            {
                AudioManager.instance.PlaySFX("Door", 1);
                player = collision.gameObject;
                OpenDoor();
            }
        }
    }

    public void OpenDoor()
    {
        PlayerData.DATA.uiManager.FinishLevel();
    }
    public void nextScene()
    {
        TransitionManager.instance.LoadScene("L" + LevelToLoad);
    }
    public void resetScene()
    {
        TransitionManager.instance.ResetLevel();
    }
}