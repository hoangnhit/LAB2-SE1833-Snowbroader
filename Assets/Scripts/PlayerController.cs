using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;
    AudioSource audioSource;

    [Header("Audio Settings")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip flipSound;
    [SerializeField] AudioClip speedUpSound;
    [SerializeField] AudioClip slowDownSound;

    [Header("Speed Settings")]
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 35f;
    [SerializeField] float accelerationRate = 15f;
    [SerializeField] float decelerationRate = 10f;
    [SerializeField] float brakeRate = 25f;
    [SerializeField] float torqueAmount = 10f;
    [SerializeField] float boostedTorqueAmount = 20f;
    [SerializeField] float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.5f;

    [Header("Effects")]
    [SerializeField] ParticleSystem dustParticles;

    // ===== Magnet hack settings =====
    [Header("🧲 Magnet (Cheat) Settings")]
    [SerializeField] float magnetRadius = 25f;   // Bán kính hút vàng (tăng để hút xa)
    [SerializeField] float magnetSpeed = 80f;    // Tốc độ lực hút (tăng để hút mạnh)
    bool magnetActive = false;                   // Trạng thái nam châm
    [SerializeField] ParticleSystem magnetEffect; // Optional: particle placed as child "MagnetEffect"

    bool canMove = true;
    float currentSpeed;
    bool isSpeedReduced = false;

    // Lộn vòng
    float previousRotation = 0f;
    float totalRotation = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();

        // magnetEffect có thể được kéo vào inspector; fallback tìm child nếu không set
        if (magnetEffect == null)
            magnetEffect = transform.Find("MagnetEffect")?.GetComponent<ParticleSystem>();

        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
            if (groundCheck == null)
            {
                Debug.LogError("GroundCheck object not found!");
                enabled = false;
                return;
            }
        }

        currentSpeed = minSpeed;
        if (surfaceEffector2D != null)
            surfaceEffector2D.speed = currentSpeed;
    }

    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            ControlSpeedSmoothly();
            Jump();
            CheckFlip();
            HandleDustEffect();
        }

        HandleCheatCombos(); // tổ hợp phím hack
        HandleMagnet();      // hút vàng khi bật
    }

    public void DisableControls() => canMove = false;

    void RotatePlayer()
    {
        float torque = Input.GetKey(KeyCode.LeftShift) ? boostedTorqueAmount : torqueAmount;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            rb2d.AddTorque(torque);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            rb2d.AddTorque(-torque);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
            if (jumpSound && audioSource)
                audioSource.PlayOneShot(jumpSound);
        }
    }

    void CheckFlip()
    {
        float currentRotation = transform.eulerAngles.z;
        float deltaRotation = Mathf.DeltaAngle(previousRotation, currentRotation);
        totalRotation += deltaRotation;
        previousRotation = currentRotation;

        if (Mathf.Abs(totalRotation) >= 360f)
        {
            totalRotation = 0f;
            if (flipSound && audioSource)
                audioSource.PlayOneShot(flipSound);
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, grounded ? Color.green : Color.red);
        return grounded;
    }

    // 🌟 Tăng/Giảm tốc độ mượt
    void ControlSpeedSmoothly()
    {
        if (isSpeedReduced || surfaceEffector2D == null) return;

        float targetSpeed = currentSpeed;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetSpeed = Mathf.Min(currentSpeed + accelerationRate * Time.deltaTime, maxSpeed);
            if (speedUpSound && audioSource && !audioSource.isPlaying)
                audioSource.PlayOneShot(speedUpSound);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            targetSpeed = Mathf.Max(currentSpeed - brakeRate * Time.deltaTime, minSpeed);
            if (slowDownSound && audioSource && !audioSource.isPlaying)
                audioSource.PlayOneShot(slowDownSound);
        }
        else
        {
            targetSpeed = Mathf.Max(currentSpeed - decelerationRate * Time.deltaTime, minSpeed);
        }

        currentSpeed = targetSpeed;
        surfaceEffector2D.speed = currentSpeed;
    }

    public void ReduceSpeed()
    {
        isSpeedReduced = true;
        currentSpeed = minSpeed;
        if (surfaceEffector2D != null)
            surfaceEffector2D.speed = currentSpeed;

        Invoke(nameof(ResetSpeed), 3f);
    }

    void ResetSpeed() => isSpeedReduced = false;
    public float GetSpeed() => currentSpeed;

    void HandleDustEffect()
    {
        if (dustParticles == null) return;

        if (currentSpeed > (maxSpeed * 0.7f))
        {
            if (!dustParticles.isPlaying) dustParticles.Play();
        }
        else
        {
            if (dustParticles.isPlaying) dustParticles.Stop();
        }
    }

    // ===== Magnet: hút vật lý (ưu tiên Rigidbody2D coin) =====
    void HandleMagnet()
    {
        if (!magnetActive) return;

        // Tìm coin theo tag "Coin"
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        if (coins == null || coins.Length == 0) return;

        Vector2 myPos = transform.position;
        float r2 = magnetRadius * magnetRadius;

        for (int i = 0; i < coins.Length; i++)
        {
            GameObject c = coins[i];
            if (c == null) continue;

            Vector2 diff = (Vector2)c.transform.position - myPos;
            float distSqr = diff.sqrMagnitude;

            if (distSqr <= r2)
            {
                float distance = Mathf.Sqrt(distSqr);
                // pullStrength tăng khi gần hơn
                float pullStrength = Mathf.Lerp(magnetSpeed * 0.3f, magnetSpeed, 1f - (distance / magnetRadius));

                // debug đường hút (tạm thời, sẽ hiển thị trên Scene view)
                Debug.DrawLine(c.transform.position, myPos, Color.yellow, 0.02f);

                Rigidbody2D coinRb = c.GetComponent<Rigidbody2D>();
                if (coinRb != null)
                {
                    // dùng lực để kéo (mượt và tương thích physics)
                    Vector2 dir = (myPos - (Vector2)c.transform.position).normalized;
                    // scale lực với pullStrength; multiply để có cảm giác mạnh
                    coinRb.AddForce(dir * pullStrength * 5f * Time.deltaTime, ForceMode2D.Force);
                    // (tuỳ coin, bạn có thể muốn giảm gravity khi hút)
                }
                else
                {
                    // fallback: dịch tọa độ (không khuyến khích nếu coin có physics)
                    c.transform.position = Vector2.MoveTowards(c.transform.position, myPos, pullStrength * Time.deltaTime);
                }
            }
        }
    }

    // 🧩 Hack phím
    private void HandleCheatCombos()
    {
        // Hack điểm: H + G
        if (Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.G))
        {
            var gm = FindAnyObjectByType<GameManager>();
            gm?.AddScore(500);
            Debug.Log("💰 Hack điểm (+500)!");
        }

        // Bất tử: K + L
        if (Input.GetKey(KeyCode.K) && Input.GetKeyDown(KeyCode.L))
        {
            PlayerCollision.ToggleInvincible();
            Debug.Log("🛡️ Bật/tắt bất tử: " + PlayerCollision.IsInvincible());
        }

        // Nam châm: N + M
        if (Input.GetKey(KeyCode.N) && Input.GetKeyDown(KeyCode.M))
        {
            magnetActive = !magnetActive;
            Debug.Log("🧲 Magnet = " + magnetActive);

            if (magnetEffect != null)
            {
                if (magnetActive) magnetEffect.Play();
                else magnetEffect.Stop();
            }
        }
    }
}
