using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlataform : MonoBehaviour
{
    [Header("References")]
    public Transform target1;
    public Transform target2;
    public GameObject plataformPrefab;

    [Header("Settings")]
    public float speed = 2f;

    private GameObject plataformInstance;
    private Transform currentTarget;

    void Start()
    {
        plataformInstance = Instantiate(plataformPrefab, target1.position, Quaternion.identity);
        currentTarget = target2;
    }

    void FixedUpdate()
    {
        if (plataformInstance == null) return;

        plataformInstance.transform.position = Vector3.MoveTowards(plataformInstance.transform.position,currentTarget.position,speed * Time.deltaTime * PlayerTime.TIME);
        if (Vector3.Distance(plataformInstance.transform.position, currentTarget.position) < 0.05f)
        {
            currentTarget = currentTarget == target1 ? target2 : target1;
        }
    }
}
