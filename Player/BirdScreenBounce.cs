using UnityEngine;

public class BirdScreenBounce : MonoBehaviour
{
    private Camera _mainCamera;

    [Header("Viewport Padding")]
    [SerializeField] private float _leftPadding = 0.05f;
    [SerializeField] private float _rightPadding = 0.05f;
    [SerializeField] private float _bottomPadding = 0.05f;
    [SerializeField] private float _topPadding = 0.05f;

    [Header("Bounce")]
    [SerializeField] private float _bounceCooldown = 0.05f;

    private float _bounceTimer;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera == null)
        {
            return;
        }

        if (_bounceTimer > 0f)
        {
            _bounceTimer -= Time.deltaTime;
        }

        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);

        float minX = Mathf.Clamp01(_leftPadding);
        float maxX = Mathf.Clamp01(1f - _rightPadding);
        float minY = Mathf.Clamp01(_bottomPadding);
        float maxY = Mathf.Clamp01(1f - _topPadding);

        bool hitLeft = viewportPos.x < minX;
        bool hitRight = viewportPos.x > maxX;
        bool hitBottom = viewportPos.y < minY;
        bool hitTop = viewportPos.y > maxY;

        if (!hitLeft && !hitRight && !hitBottom && !hitTop)
        {
            return;
        }

        viewportPos.x = Mathf.Clamp(viewportPos.x, minX, maxX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, minY, maxY);

        transform.position = _mainCamera.ViewportToWorldPoint(viewportPos);

        if (_bounceTimer > 0f)
        {
            return;
        }

        Vector2 direction = transform.up;

        if (hitLeft)
        {
            direction = Vector2.Reflect(direction, Vector2.right);
        }

        if (hitRight)
        {
            direction = Vector2.Reflect(direction, Vector2.left);
        }

        if (hitBottom)
        {
            direction = Vector2.Reflect(direction, Vector2.up);
        }

        if (hitTop)
        {
            direction = Vector2.Reflect(direction, Vector2.down);
        }

        if (direction.sqrMagnitude > 0.0001f)
        {
            transform.up = direction.normalized;
        }

        _bounceTimer = _bounceCooldown;
    }
}