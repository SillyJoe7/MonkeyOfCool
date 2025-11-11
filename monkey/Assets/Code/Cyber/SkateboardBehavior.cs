using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class SkateboardBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float turnRate = 150f;
    [SerializeField] private bool onlyTurnWhileMoving = true;

    [Header("Drift")]
    [SerializeField] private float driftTurnMultiplier = 1.5f;
    [SerializeField] private float driftFriction = 0.8f;

    [Header("Jump & Ground Check")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Visual Tilt")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float tiltAngle = 15f;
    [SerializeField] private float tiltSmooth = 5f;

    [Header("Drift Effects")]
    [SerializeField] private ParticleSystem[] driftParticles;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField][Range(0.5f, 1.5f)] private float pitchVariation = 0.1f;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float currentSpeed;
    private float speedVelocity;
    private float turnInput;
    private float moveInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isDrifting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isDrifting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            PlayJumpSound();
        }
    }

    void FixedUpdate()
    {
        wasGrounded = isGrounded;
        CheckGround();

        // Detect landing transition
        if (!wasGrounded && isGrounded)
        {
            float impactStrength = Mathf.Clamp(rb.linearVelocity.y * -0.1f, 0.1f, 1f);
            PlayLandingSound(impactStrength);
        }

        HandleMovement();
        HandleTilt();
        HandleDriftParticles();
    }

    void PlayJumpSound()
    {
        if (jumpSound == null || audioSource == null) return;

        // Stop any landing sound currently playing
        if (audioSource.isPlaying && audioSource.clip == landingSound)
            audioSource.Stop();

        audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    void PlayLandingSound(float volume = 1f)
    {
        if (landingSound == null || audioSource == null) return;

        // Stop any jump sound currently playing
        if (audioSource.isPlaying && audioSource.clip == jumpSound)
            audioSource.Stop();

        audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
        audioSource.clip = landingSound;
        audioSource.volume = volume;
        audioSource.Play();
    }

    void HandleDriftParticles()
    {
        if (driftParticles == null || driftParticles.Length == 0) return;

        foreach (var ps in driftParticles)
        {
            if (isDrifting && isGrounded && Mathf.Abs(currentSpeed) > 2f)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
            else
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
        }
    }

    void HandleMovement()
    {
        float targetSpeed = moveInput * maxSpeed;
        float smoothTime = Mathf.Abs(targetSpeed) > Mathf.Abs(currentSpeed)
            ? 1f / acceleration
            : 1f / deceleration;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothTime);

        Vector3 move = transform.forward * currentSpeed * Time.fixedDeltaTime;
        if (isDrifting) move *= driftFriction;
        rb.MovePosition(rb.position + move);

        if (!onlyTurnWhileMoving || Mathf.Abs(currentSpeed) > 0.1f)
        {
            float rotation = turnInput * turnRate * Time.fixedDeltaTime;
            if (isDrifting) rotation *= driftTurnMultiplier;
            Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }
    }

    void HandleTilt()
    {
        if (!visualModel) return;

        float targetZ = -turnInput * tiltAngle;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZ);
        visualModel.localRotation = Quaternion.Slerp(
            visualModel.localRotation,
            targetRot,
            Time.fixedDeltaTime * tiltSmooth
        );
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
