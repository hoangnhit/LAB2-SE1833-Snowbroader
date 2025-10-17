using System;
using UnityEngine;

/// <summary>
/// Player Controller
/// </summary>
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;
    AudioSource audioSource;  // AudioSource để phát âm thanh

    // Âm thanh khi nhảy và khi lộn vòng
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip flipSound;  // Âm thanh khi lộn vòng

    // The amount of torque applied to the player for rotation
    [SerializeField] float torqueAmount = 10f;
    [SerializeField] float boostedTorqueAmount = 20f; // Tốc độ xoay khi tăng tốc

    // The speed when the player is boosted
    [SerializeField] float boostSpeed = 35f;
    // The normal movement speed of the player
    [SerializeField] float baseSpeed = 20f;
    // The force applied when the player jumps
    [SerializeField] float jumpForce = 10f;
    // Reference to the GroundCheck transform, used to detect if the player is on the ground
    [SerializeField] Transform groundCheck;
    // The layer mask that defines what is considered ground
    [SerializeField] LayerMask groundLayer;
    // The radius of the ground check area, determining how far to check for ground contact
    [SerializeField] float groundCheckRadius = 0.5f;
    [SerializeField] float reducedSpeed = 5f;

    bool canMove = true;
    float currentSpeed; // Track current speed dynamically
    bool isSpeedReduced = false; // Add this at the top with other member variables

    // Biến để kiểm tra lộn vòng
    float previousRotation = 0f;
    float totalRotation = 0f; // Tổng số độ đã xoay

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();  // Lấy AudioSource trên Player
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();

        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
        }

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck object not found! Make sure it exists in the hierarchy.");
            enabled = false;
        }

        currentSpeed = baseSpeed; // Set initial speed
        surfaceEffector2D.speed = currentSpeed;
    }

    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            RespondToBoost();
            Jump();
            CheckFlip(); // Kiểm tra lộn vòng
            AdjustSpeedBasedOnSlope(); // NEW: Dynamically adjust speed
        }
    }

    public void DisableControls()
    {
        canMove = false;
    }

    void RotatePlayer()
    {
        float currentTorque = Input.GetKey(KeyCode.LeftShift) ? boostedTorqueAmount : torqueAmount;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb2d.AddTorque(currentTorque);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb2d.AddTorque(-currentTorque);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);

            // Phát âm thanh khi nhảy
            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    // Hàm kiểm tra và phát âm thanh khi lộn vòng
    void CheckFlip()
    {
        float currentRotation = transform.eulerAngles.z;
        float deltaRotation = Mathf.DeltaAngle(previousRotation, currentRotation);
        totalRotation += deltaRotation;
        previousRotation = currentRotation;

        // Kiểm tra nếu đã lộn đủ 360 độ
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            totalRotation = 0f; // Reset lại tổng số độ đã xoay

            // Phát âm thanh khi lộn vòng
            if (flipSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(flipSound);
            }
        }
    }

    /// <summary>
    /// Check if the player is grounded
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        if (groundCheck == null) return false;

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, grounded ? Color.green : Color.red);
        Debug.Log($"IsGrounded: {grounded}");

        return grounded;
    }

    /// <summary>
    /// Respond to boost
    /// </summary>
    void RespondToBoost()
    {
        if (isSpeedReduced) return;
        if (surfaceEffector2D == null) return;
        float boostFactor = 1.5f;

        if (Input.GetKey(KeyCode.UpArrow) && IsGrounded())
        {
            // currentSpeed = boostSpeed;
            currentSpeed *= boostFactor; // Multiply current speed
            Debug.Log("Boost applied. New speed: " + currentSpeed);
            surfaceEffector2D.speed = currentSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
            surfaceEffector2D.speed = currentSpeed;
        }
    }

    public void ReduceSpeed()
    {
        isSpeedReduced = true;
        currentSpeed = reducedSpeed;
        if (surfaceEffector2D != null)
        {
            surfaceEffector2D.speed = currentSpeed;
        }
        Debug.Log("Speed reduced to: " + currentSpeed);
        Invoke("ResetSpeed", 3f);
    }

    void ResetSpeed()
    {
        isSpeedReduced = false; // Allows speed changes again
        currentSpeed = baseSpeed;

        if (surfaceEffector2D != null)
        {
            surfaceEffector2D.speed = currentSpeed;
        }

        Debug.Log("Speed reset to: " + currentSpeed);
    }
    public float GetSpeed()
    {
        return currentSpeed;
    }


    void AdjustSpeedBasedOnSlope()
    {
        if (surfaceEffector2D == null || isSpeedReduced) return;

        // Get the player's movement direction
        float yVelocity = rb2d.linearVelocity.y;

        float speedFactor = 1.0f;

        if (yVelocity < -0.5f) // Moving downhill
        {
            speedFactor = 1.5f; // Increase speed
        }
        else if (yVelocity > 0.5f) // Moving uphill
        {
            speedFactor = 0.7f; // Reduce speed
        }

        // Update speed based on terrain
        currentSpeed = baseSpeed * speedFactor;

        // If boosting, multiply the adjusted speed
        if (Input.GetKey(KeyCode.UpArrow) && IsGrounded())
        {
            float boostFactor = 1.5f;
            currentSpeed *= boostFactor;
            Debug.Log("Boosted while on slope! New speed: " + currentSpeed);
        }

        surfaceEffector2D.speed = currentSpeed;
    }


}
