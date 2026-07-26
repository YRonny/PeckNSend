using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpMail : MonoBehaviour
{
    [Header("Mail")]
    [SerializeField] private float pickupRadius = 1f;
    [SerializeField] private Vector3 heldMailLocalOffset = new Vector3(-0.5f, 0f, 0f);
    [SerializeField] private Vector3 heldMailWorldOffset = new Vector3(0f, -1f, 0f);
    [SerializeField] private Vector3 dropOffset = new Vector3(-1f, 0f, 0f);

    private Flying _flying;
    private GameObject _mail;

    public GameObject Mail => _mail;
    public bool HasMail => _mail != null;

    private void Awake()
    {
        _flying = GetComponent<Flying>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up * pickupRadius, Color.red);

        if (_mail != null)
        {
            _mail.transform.rotation = Quaternion.identity;
            _mail.transform.position = transform.position + heldMailWorldOffset;
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (HasMail)
            DropMail();
        else
            PickUp();
    }

    private void PickUp()
    {
        if (HasMail)
            return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            pickupRadius,
            LayerMask.GetMask("Mail")
        );

        if (hits.Length == 0)
            return;

        Transform closestMail = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Collider hit in hits)
        {
            if (hit.transform.parent != null) continue;
            float distanceSqr = (hit.transform.position - currentPos).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestMail = hit.transform;
            }
        }

        if (closestMail == null)
            return;

        _mail = closestMail.gameObject;
        
        _mail.transform.SetParent(transform);
        _mail.transform.localPosition = heldMailLocalOffset;
        _mail.transform.localRotation = Quaternion.identity;

        Rigidbody rb = _mail.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        MailDespawner despawner = _mail.GetComponent<MailDespawner>();
        if (despawner != null)
            despawner.enabled = false;

        MailWaveMovement waveMovement = _mail.GetComponent<MailWaveMovement>();
        if (waveMovement != null)
            waveMovement.enabled = false;

        MailTypeAssigner typeAssigner = _mail.GetComponent<MailTypeAssigner>();
        if (typeAssigner != null)
            typeAssigner.PlayFx();

        UpdateMailSpeed();
        Debug.Log("Picked up closest mail");
    }

    public void DropMail()
    {
        if (!HasMail)
            return;

        GameObject droppedMail = _mail;
        _mail = null;

        droppedMail.transform.SetParent(null);
        droppedMail.transform.rotation = Quaternion.identity;
        droppedMail.transform.position = transform.position + heldMailLocalOffset;
        droppedMail.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        droppedMail.GetComponent<MailDespawner>().enabled = true;

        UpdateMailSpeed();
        Debug.Log("Dropped mail");
    }

    public void ClearHeldMailReference()
    {
        _mail = null;
        UpdateMailSpeed();
    }

    private void UpdateMailSpeed()
    {
        float birdSpeed = 0.7f;

        if (_mail != null)
        {
            if (_mail.CompareTag("HeavyMail"))
                birdSpeed = 0.5f;
            else if (_mail.CompareTag("FragileMail"))
                birdSpeed = 0.9f;
        }

        if (_flying != null)
            _flying.mailSpeedModifier = birdSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}