using System.Collections;
using UnityEngine;

public class PlayerMovment2D : MonoBehaviour
{
    [Header("Stats")]
    private float horizontal;
    public float speed = 8f;
    public float jumpPower = 16f;
    public bool isFacingRight = true;

    [Header("States")]
    private bool isWalSliding;
    public float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirecion;
    private float wallJumpingCounter;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Coyote Time & Jump Buffer")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float jumpBuferTime = 0.2f;
    private float jumpBuferTimeCounter;

    [Header("Slide System")]
    public float slideStartSpeed = 12f;
    public float slideAcceleration = 25f;
    public float maxSlideSpeed = 30f;
    public float slopeBoostMultiplier = 1.5f;
    public float slideFriction = 12f;

    private bool isSliding;
    private float currentSlideSpeed;

    private bool onSlope;
    private Vector2 slopeNormal;
    public float slideDownForce = 25f;



    [Header("Art & Sound")]
    public Animator animManager;
    private bool isJumpingAnim;

    public AudioSource JumpSound;
    public AudioSource DashSound;
    public ParticleSystem DustRun;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public TrailRenderer tr;
    public Transform SpritePlayer;
    private HelthSystem lifeSystem;

    private bool canScale;
    private bool isCrouching;
    public bool canMove = true;
    public bool haveControl = true;

    public InputSystem_Actions input;
    float carriedVelocity;

    private Transform currentPlatform;
    private Transform lastPlatformPos;
    float platformOffsetX;
    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        lifeSystem = GetComponent<HelthSystem>();
        haveControl = true;
        canMove = true;
        tr.emitting = false;
    }
    private void Update()
    {
        PlayerSpriteManagment();

        if (!haveControl) return;

        Movment();

        // Other logic
        if (!isWallJumping) Flip();
        WallSlide();
        wallJump();
        //animations();
    }

    private void Movment()
    {
        float rawHorizontal = input.Player.Move.ReadValue<Vector2>().x;

        Vector2 aim = input.Player.Aim.ReadValue<Vector2>();
        float aimX = aim.x;
        float aimY = aim.y;

        // Deadzones
        float movementDeadzone = 0.2f;
        float verticalAimLock = 0.2f;

        if (canMove)
        {
            if (Mathf.Abs(aimY) > verticalAimLock && Mathf.Abs(aimX) < verticalAimLock)
            {
                horizontal = 0f;
            }
            else
            {
                horizontal = (Mathf.Abs(rawHorizontal) < movementDeadzone) ? 0f : rawHorizontal;
            }
        }
        else
        {
            horizontal = 0f;
        }

        // Ground & coyote timer
        if (IsGroudnded()) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (input.Player.Jump.WasPressedThisFrame()) jumpBuferTimeCounter = jumpBuferTime;
        else jumpBuferTimeCounter -= Time.deltaTime;

        // START SLIDE
        // Iniciar slide manual
        if (input.Player.Crouch.WasPressedThisFrame() && IsGroudnded() && !isSliding)
        {
            StartSlide();
        }

        // Terminar slide
        if (!input.Player.Crouch.IsPressed() && isSliding)
        {
            StopSlide();
        }


        // Full jump
        if (jumpBuferTimeCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpBuferTimeCounter = 0;
            JumpSound.Play();
        }

        // Cut jump short
        if (input.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
            coyoteTimeCounter = 0f;
    }


    private void animations()
    {
        if (horizontal == 0 && IsGroudnded())
        {
            animManager.enabled = true;
            animManager.SetBool("IdelAnim", true);
            animManager.SetBool("RunAnim", false);
            SpritePlayer.transform.localScale = new Vector2(SpritePlayer.transform.localScale.x, SpritePlayer.transform.localScale.y);
        }
        else if (IsGroudnded())
        {
            animManager.SetBool("IdelAnim", false);
            animManager.SetBool("RunAnim", true);
        }

        if (!IsGroudnded()) animManager.enabled = false;
        if (IsGroudnded()) isJumpingAnim = false;

        if (input.Player.Jump.IsPressed() && !IsGroudnded())
            StartCoroutine(jumpAnim());
    }

    public bool IsGroudnded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool IsPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (hit != null && hit.CompareTag("MovPlat"))
        {
            if (hit.transform != currentPlatform)
            {
                currentPlatform = hit.transform;
                platformOffsetX = transform.position.x - currentPlatform.position.x;
            }
            return true;
        }

        currentPlatform = null;
        return false;
    }
    private bool CheckSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 1.2f, groundLayer);

        if (hit)
        {
            slopeNormal = hit.normal;
            float angle = Vector2.Angle(slopeNormal, Vector2.up);
            return angle > 5f;
        }

        return false;
    }


    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGroudnded() && horizontal != 0)
        {
            isWalSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else isWalSliding = false;
    }

    private void wallJump()
    {
        if (isWalSliding)
        {
            isWallJumping = false;
            wallJumpDirecion = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(stopWallJumping));
        }
        else wallJumpingCounter -= Time.deltaTime;

        if (input.Player.Jump.WasPressedThisFrame() && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirecion * wallJumpingPower.x, wallJumpingPower.y);
            JumpSound.Play();
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpDirecion)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(stopWallJumping), wallJumpingDuration);
        }
    }

    private void stopWallJumping()
    {
        isWallJumping = false;
    }

    private void FixedUpdate()
    {
        bool onPlatform = IsPlatform();

        if (onPlatform && currentPlatform != null)
        {
            if (horizontal != 0)
            {
                platformOffsetX = transform.position.x - currentPlatform.position.x;
            }
            else
            {
                transform.position = new Vector2(
                    currentPlatform.position.x + platformOffsetX,
                    transform.position.y
                );
            }
        }
        if (!haveControl) return;

        onSlope = CheckSlope();

        if (isSliding)
        {
            HandleSlide();
            return;
        }

        if (!isWallJumping)
        {
            if (Mathf.Abs(carriedVelocity) > 0.1f)
            {
                carriedVelocity = Mathf.MoveTowards(
                    carriedVelocity,
                    horizontal * speed,
                    Time.fixedDeltaTime * 20f
                );

                rb.linearVelocity = new Vector2(
                    transform.localScale.x * carriedVelocity,
                    rb.linearVelocity.y
                );
            }
            else
            {
                rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            }
        }

    }

    void HandleSlide()
    {
        if (CheckSlope())
        {
            float slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
            float boost = 1 + (slopeAngle / 45f) * slopeBoostMultiplier;
            currentSlideSpeed += slideAcceleration * boost * Time.fixedDeltaTime;
        }
        else
        {
            currentSlideSpeed += slideAcceleration * Time.fixedDeltaTime;
        }

        currentSlideSpeed = Mathf.Clamp(currentSlideSpeed, 0, maxSlideSpeed);

        rb.linearVelocity = new Vector2(
            transform.localScale.x * currentSlideSpeed,
            rb.linearVelocity.y
        );

        rb.AddForce(Vector2.down * slideDownForce, ForceMode2D.Force);
    }




    private void StartSlide()
    {
        isSliding = true;
        currentSlideSpeed = Mathf.Max(rb.linearVelocity.magnitude, slideStartSpeed);
        tr.emitting = true;
        //animManager.SetBool("Crouch", true);
    }

    void StopSlide()
    {
        isSliding = false;
        carriedVelocity = currentSlideSpeed;
        tr.emitting = false;
        //animManager.SetBool("Crouch", false);
    }




    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator jumpAnim()
    {
        if (!isJumpingAnim)
        {
            isJumpingAnim = true;
            SpritePlayer.transform.localScale = new Vector2(1f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            SpritePlayer.transform.localScale = new Vector2(0.8f, 1);
            yield return new WaitForSeconds(0.2f);
            SpritePlayer.transform.localScale = new Vector2(1, 1);
        }
    }

    public void InpulsFeedBack(float hitImpact, float knockdownTime)
    {
        StartCoroutine(ImpulseRoutine(hitImpact, knockdownTime));
    }

    private IEnumerator ImpulseRoutine(float hitImpact, float knockdownTime)
    {
        haveControl = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * hitImpact, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(-transform.localScale.x, 0f) * hitImpact, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockdownTime);

        haveControl = true;
    }


    private void PlayerSpriteManagment()
    {
        SpritePlayer.transform.position = transform.position;
        SpritePlayer.transform.localScale = new Vector3(transform.localScale.x, SpritePlayer.localScale.y, SpritePlayer.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("scale")) canScale = true;
        if (collision.CompareTag("BossBullet"))
        {
            lifeSystem.Hit(100);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("scale"))
        {
            canScale = false;
            rb.gravityScale = 8;
        }
    }

}
