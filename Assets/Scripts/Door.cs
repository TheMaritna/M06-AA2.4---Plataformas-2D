using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int LevelToLoad = 0;
    private GameObject player;
    private void Update()
    {
        if(player!=null)
            player.transform.position = transform.position;

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
        TransitionManager.instance.LoadScene("L" + LevelToLoad);
    }
}