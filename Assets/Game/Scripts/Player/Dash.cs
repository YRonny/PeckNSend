using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdDash : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 100f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Haptics")]
    [SerializeField] private float dashStartLow = 0.6f;
    [SerializeField] private float dashStartHigh = 0.9f;
    [SerializeField] private float dashStartTime = 0.1f;

    [SerializeField] private float hitLow = 1f;
    [SerializeField] private float hitHigh = 1f;
    [SerializeField] private float hitTime = 0.5f;

    private Flying bird;
    private PlayerInput playerInput;
    private Gamepad playerGamepad;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector3 dashDirection;
    private bool hasHitDuringDash = false;

    public bool IsDashing => isDashing;

    private void Awake()
    {
        bird = GetComponent<Flying>();
        playerInput = GetComponent<PlayerInput>();
        CacheGamepad();
    }

    private void OnEnable()
    {
        CacheGamepad();
    }

    private void CacheGamepad()
    {
        playerGamepad = null;

        if (playerInput == null) return;

        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad gamepad)
            {
                playerGamepad = gamepad;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.fixedDeltaTime;
        if (!isDashing) return;

        if (dashTimer > 0f) dashTimer -= Time.fixedDeltaTime;
        else return;

        transform.position += transform.up * (dashSpeed * Time.deltaTime);

        if (dashTimer <= 0f)
        {
            isDashing = false;
            StopHaptics();

            if (!hasHitDuringDash)
                bird.StartSpin();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        Vector2 reflected = Vector2.Reflect(transform.up, contact.normal);

        if (reflected.sqrMagnitude > 0.0001f)
            transform.up = reflected.normalized;

        if (!isDashing) return;

        Flying otherBird = other.gameObject.GetComponentInParent<Flying>();
        if (otherBird == null) return;

        hasHitDuringDash = true;

        otherBird.GetHit(bird.transform.up);
        DisableColliderTemporarily(otherBird);

        PlayHaptics(hitLow, hitHigh, hitTime);

        isDashing = false;
        cooldownTimer = dashCooldown;
    }

    private void DisableColliderTemporarily(Flying target)
    {
        MonoBehaviour mb = target;
        if (mb == null) return;

        Collider col = mb.GetComponent<Collider>();
        if (col != null)
            mb.StartCoroutine(ReEnableCollider(col, target.SpinDuration));
    }

    private IEnumerator ReEnableCollider(Collider col, float delay)
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;
        if (isDashing || bird.IsStunned) return;
        if (cooldownTimer > 0f) return;

        dashDirection = transform.up;
        GetComponent<AudioSource>().Play();

        isDashing = true;
        hasHitDuringDash = false;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;

        PlayHaptics(dashStartLow, dashStartHigh, dashStartTime);
    }

    private void PlayHaptics(float low, float high, float duration)
    {
        if (playerGamepad == null) return;
        StartCoroutine(HapticsBurst(low, high, duration));
    }

    private IEnumerator HapticsBurst(float low, float high, float duration)
    {
        playerGamepad.SetMotorSpeeds(low, high);
        yield return new WaitForSeconds(duration);
        playerGamepad.ResetHaptics();
    }

    private void StopHaptics()
    {
        if (playerGamepad == null) return;
        playerGamepad.ResetHaptics();
    }

    private void OnDisable()
    {
        StopHaptics();
    }
}