using UnityEngine;

public class GrappleZone : MonoBehaviour
{
    public Transform pivot;
    public float minAngularSpeed = 5f;
    public float releaseBoost = 1.2f;
    [Range(0.5f, 1f)] public float speedDecayPerTurn = 0.8f; // 0.8 = pierde 20% de velocidad por vuelta
    public LineRenderer rope;

    private Rigidbody2D playerRb;
    private bool playerInside;
    private bool isGrappling;
    private float radius;
    private float directionSign = 1f;
    private float currentAngularSpeed;
    private float totalRotation;
    private float lastAngle;
    private Vector2 lastVelocity;

    private void Awake()
    {
        if (pivot == null) pivot = transform;

        if (rope != null)
        {
            rope.positionCount = 2;
            rope.enabled = false;
        }
    }

    private void Update()
    {
        if (!playerInside || playerRb == null) return;

        if (PlayerData.DATA.PC.input.Player.Jump.WasPressedThisFrame() && !isGrappling)
        {
            StartGrapple();
        }

        if (PlayerData.DATA.PC.input.Player.Jump.WasReleasedThisFrame() && isGrappling)
        {
            Release();
        }

        UpdateRope();
    }

    private void FixedUpdate()
    {
        if (!isGrappling || playerRb == null) return;

        Vector2 offset = playerRb.position - (Vector2)pivot.position;

        // --- Lógica de reducción de velocidad por vuelta ---
        float currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        float angleDelta = Mathf.DeltaAngle(lastAngle, currentAngle);

        // Acumulamos el giro total (en valor absoluto para que funcione en ambas direcciones)
        totalRotation += Mathf.Abs(angleDelta);

        if (totalRotation >= 360f)
        {
            currentAngularSpeed *= speedDecayPerTurn;
            currentAngularSpeed = Mathf.Max(currentAngularSpeed, minAngularSpeed);
            totalRotation -= 360f; // Reiniciamos el contador de la vuelta
        }
        lastAngle = currentAngle;
        // ----------------------------------------------------

        Vector2 tangent = new Vector2(-offset.y, offset.x).normalized * directionSign;
        float speed = currentAngularSpeed * radius;
        playerRb.linearVelocity = tangent * speed;

        Vector2 correctedPos = (Vector2)pivot.position + offset.normalized * radius;
        playerRb.position = Vector2.Lerp(playerRb.position, correctedPos, 0.2f);

        lastVelocity = playerRb.linearVelocity;
    }

    void StartGrapple()
    {
        isGrappling = true;
        totalRotation = 0f;
        AudioManager.instance.PlaySFX("Graple", 1);

        Vector2 initialVelocity = playerRb.linearVelocity;
        float entrySpeed = initialVelocity.magnitude;

        playerRb.GetComponent<PlayerMovment2D>().externalControl = true;
        playerRb.gravityScale = 0f;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;

        Vector2 offset = playerRb.position - (Vector2)pivot.position;
        radius = offset.magnitude;
        lastAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        float crossProduct = offset.x * initialVelocity.y - offset.y * initialVelocity.x;
        directionSign = Mathf.Sign(crossProduct);

        currentAngularSpeed = Mathf.Max(minAngularSpeed, entrySpeed / radius);

        if (rope != null)
            rope.enabled = true;
    }

    void UpdateRope()
    {
        if (!isGrappling || rope == null) return;

        rope.SetPosition(0, pivot.position);
        rope.SetPosition(1, playerRb.position);
    }

    void Release()
    {
        isGrappling = false;

        playerRb.GetComponent<PlayerMovment2D>().externalControl = false;
        playerRb.gravityScale = 8f;

        Vector2 releaseVelocity = lastVelocity;

        if (playerRb.position.y < pivot.position.y)
        {
            float verticalInfluence = releaseVelocity.y;

            if (verticalInfluence > 0)
            {
                releaseVelocity.y *= 0.5f;
                releaseVelocity.x += Mathf.Sign(releaseVelocity.x) * (verticalInfluence * 0.5f);
            }
        }

        playerRb.linearVelocity = releaseVelocity * releaseBoost;

        if (rope != null)
            rope.enabled = false;

        playerRb = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            if (isGrappling) Release();
        }
    }
}