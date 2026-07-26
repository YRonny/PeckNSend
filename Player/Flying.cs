using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Flying : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float modelFlipSpeed = 360f;
    [SerializeField] public float mailSpeedModifier = 1;

    public float TurnSpeed => turnSpeed;
    public float MoveSpeed => moveSpeed;
    private Vector3 _moveDirection;
    private Vector2 _moveInput;
    private Vector3 _knockbackVelocity = Vector3.zero;
    
    
    [Header("Spin")]
    [SerializeField] private float spinDuration = 0.8f;
    [SerializeField] private float spinDegreesPerSecond = 1080f;
    [SerializeField] private float spinSlowdown = 4f;


    [Header("Knockback")]
    [SerializeField] private float bounceForce = 3f;
    public float SpinDuration => spinDuration;
    
    private bool isSpinning = false;
    private float spinTimer = 0f;
    private float currentSpinAngle = 0f;
    public bool IsStunned => isSpinning;
   
    private void Awake()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
        float angleDifference = 0;
        if (isSpinning)
        {
            // Spin rotation
            currentSpinAngle += spinDegreesPerSecond * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, currentSpinAngle);

            // Decay knockback and apply it
            _knockbackVelocity = Vector3.MoveTowards(_knockbackVelocity, Vector3.zero, spinSlowdown * Time.deltaTime);
            transform.position += _knockbackVelocity * Time.deltaTime;

            spinTimer -= Time.deltaTime;
            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpinAngle = 0f;
                _knockbackVelocity = Vector3.zero;
            }
            else
                return;
        }

        // Normal movement
        if(_moveInput != Vector2.zero && !isSpinning)
        {
            float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
            
            angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
        float turnSlowMultiplier = Mathf.Lerp(1f, 0.3f, angleDifference / 180f);
        transform.position += transform.up * (moveSpeed* mailSpeedModifier * turnSlowMultiplier * Time.deltaTime);
        
        Transform childModel = transform.GetChild(0);
        Quaternion facingLeft  = Quaternion.AngleAxis(90f,  Vector3.up);
        Quaternion facingRight = Quaternion.AngleAxis(90f, Vector3.down); 

        Quaternion targetChildRotation = transform.up.x < 0 ? facingLeft : facingRight;

        childModel.rotation = Quaternion.RotateTowards(
            childModel.rotation,
            targetChildRotation,
            modelFlipSpeed * Time.deltaTime
        );
        
    } 
    private void FixedUpdate()
    {
        if (isSpinning)
        {
            spinTimer -= Time.fixedDeltaTime;
             //Rb.linearVelocity = Vector3.MoveTowards( Rb.linearVelocity, Vector3.zero, spinSlowdown * Time.fixedDeltaTime);

            if (spinTimer <= 0f)
            {
                isSpinning = false;
                currentSpinAngle = 0f;
               
            }
            return;
        }
        
    }
    public void OnMove(InputValue value)
    {
        //if()
            _moveInput = value.Get<Vector2>();
    }
    public void GetHit(Vector3 hitDirection)
    {
        _knockbackVelocity = hitDirection.normalized * bounceForce;

        var prefab = GameObject.Find(this.gameObject.name + "/DashCollision");
        if (prefab != null)
            prefab.GetComponent<ParticleSystem>().Play();

        StartSpin();
        
    }

    public void StartSpin()
    {
        isSpinning = true;
        spinTimer = spinDuration;
        currentSpinAngle = 0f;
        GetComponent<PickUpMail>().DropMail();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isSpinning) return;
        ContactPoint contact = other.GetContact(0);
        Vector2 reflected = Vector2.Reflect(transform.up, contact.normal);

        if (reflected.sqrMagnitude > 0.0001f)
        {
            transform.up = reflected.normalized;
        }
    }
}
