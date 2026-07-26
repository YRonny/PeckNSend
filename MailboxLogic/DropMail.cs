using PeckNSend.Presenters;
using UnityEngine;

public class DropMail : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private float extraDistance = 0.5f;
    [SerializeField] private LayerMask mailLayer;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem fx;

    private MailboxType _mailboxType;
    private Camera _camera;

    private void Awake()
    {
        Mailbox mailbox = GetComponent<Mailbox>();
        if (mailbox != null)
            _mailboxType = mailbox.mailboxType;

        _camera = Camera.main;

        if (mailLayer == 0)
            mailLayer = LayerMask.GetMask("Mail");
    }

    private void FixedUpdate()
    {
        if (_camera == null)
            return;

        Vector3 origin = transform.position;
        Vector3 direction = (_camera.transform.position - origin).normalized;
        float distanceToCamera = Vector3.Distance(origin, _camera.transform.position);
        float castDistance = distanceToCamera + extraDistance;

        Debug.DrawLine(origin, origin + direction * castDistance, Color.green);

        bool hitSomething = Physics.SphereCast(
            origin,
            sphereRadius,
            direction,
            out RaycastHit hit,
            castDistance,
            mailLayer,
            QueryTriggerInteraction.Ignore
        );

        if (!hitSomething)
            return;

        MailTypeAssigner mailTypeAssigner = hit.transform.GetComponent<MailTypeAssigner>();
        if (mailTypeAssigner == null)
            return;

        if (mailTypeAssigner.mailType != _mailboxType)
            return;

        Transform parent = hit.transform.parent;
        if (parent == null)
            return;

        PickUpMail pickUpMail = parent.GetComponent<PickUpMail>();
        if (pickUpMail == null)
            return;

        if (pickUpMail.Mail != hit.transform.gameObject)
            return;

        int scoreAmount = GetScoreForMail(hit.transform);
        MailScoreReporter.RegisterSuccessfulDelivery(parent.gameObject, scoreAmount);

        if (fx != null)
            fx.Play();

        pickUpMail.ClearHeldMailReference();
        Destroy(hit.transform.gameObject);
    }

    private int GetScoreForMail(Transform mailTransform)
    {
        if (mailTransform.CompareTag("HeavyMail"))
            return 3;

        if (mailTransform.CompareTag("FragileMail"))
            return 1;

        if (mailTransform.CompareTag("StandardMail"))
            return 2;

        return 1;
    }

    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;
        if (cam == null)
            return;

        Vector3 origin = transform.position;
        Vector3 direction = (cam.transform.position - origin).normalized;
        float distanceToCamera = Vector3.Distance(origin, cam.transform.position);
        float castDistance = distanceToCamera + extraDistance;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origin, sphereRadius);
        Gizmos.DrawWireSphere(origin + direction * castDistance, sphereRadius);
        Gizmos.DrawLine(origin, origin + direction * castDistance);
    }
}