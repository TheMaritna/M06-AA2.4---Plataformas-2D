using UnityEngine;

public class Key : MonoBehaviour
{
    private Transform player;
    public float orbitDistance = 1.5f;
    public float orbitSpeed = 180f;
    private float angle;
    private bool collected = false;
    private Vector3 orPos;

    private void Start()
    {
        orPos = transform.position;
    }
    private void Update()
    {
        if (collected && player != null)
        {
            angle += orbitSpeed * Time.deltaTime;

            float rad = angle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitDistance;

            transform.position = (Vector2)player.position + offset;
            if (PlayerData.DATA.GetComponent<PlayerRespawn>().isRespawn)
            {
                transform.position = orPos;
                collected = false;
                PlayerData.DATA.hasKey = false;
                GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collected)
        {
            PlayerData.DATA.hasKey = true;

            player = collision.transform;
            collected = true;

            GetComponent<Collider2D>().enabled = false;
            AudioManager.instance.PlaySFX("Key", 1);

        }
    }
}