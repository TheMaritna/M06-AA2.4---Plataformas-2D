using UnityEngine;

public class Generator : MonoBehaviour
{
    public enum SpawnMode
    {
        Time,
        WhenDestroyed,
        Manual
    }

    public SpawnMode mode;

    public GameObject prefab;
    public float spawnTime = 3f;

    private GameObject currentObject;
    private float timer;

    void Update()
    {
        if (mode == SpawnMode.Time)
        {
            timer += Time.deltaTime * PlayerTime.TIME;

            if (timer >= spawnTime)
            {
                Spawn();
                timer = 0;
            }
        }

        if (mode == SpawnMode.WhenDestroyed)
        {
            if (currentObject == null)
            {
                timer += Time.deltaTime * PlayerTime.TIME;

                if (timer >= spawnTime)
                {
                    Spawn();
                    timer = 0;
                }
            }
        }
    }

    public void Spawn()
    {
        currentObject = Instantiate(prefab, transform.position, Quaternion.identity);
    }

    public void SpawnManual()
    {
        if (mode == SpawnMode.Manual)
        {
            Spawn();
        }
    }
}