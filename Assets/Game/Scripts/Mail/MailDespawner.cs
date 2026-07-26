using UnityEngine;

public class MailDespawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _lifeTime = 100f;
    [SerializeField] private float _despawnDelayAfterDrop = 5f;
    [SerializeField] private float _despawnDelayOffScreen = 2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isDespawning = false;
    private bool _isOffScreenCounting = false;
    private Camera _playerCamera;
    private MailWaveMovement _waveMovement;
    private void OnEnable()
    {
        _isDespawning = false;
        _isOffScreenCounting = false;
        _playerCamera = Camera.main;
        _waveMovement = GetComponent<MailWaveMovement>();
        CancelInvoke();
        //Invoke(nameof(Despawn), _lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void FixedUpdate()
    {
        if (_isDespawning || _playerCamera == null)
            return;

        bool isVisibleToPlayer = IsVisibleToCamera();

        if (!isVisibleToPlayer && !_isOffScreenCounting)
        {
            _isOffScreenCounting = true;
            Invoke(nameof(DespawnFromOffScreen), _despawnDelayOffScreen);
        }
        else if (isVisibleToPlayer && _isOffScreenCounting)
        {
            _isOffScreenCounting = false;
            CancelInvoke(nameof(DespawnFromOffScreen));
        }

        if (_waveMovement.enabled == false && transform.parent == null)
        {
            _isDespawning = true;
            CancelInvoke(nameof(Despawn));
            CancelInvoke(nameof(DespawnFromOffScreen));
            Invoke(nameof(Despawn), _despawnDelayAfterDrop);
        }
    }

    private bool IsVisibleToCamera()
    {
        Vector3 viewportPos = _playerCamera.WorldToViewportPoint(transform.position);

        return viewportPos.z > 0f &&
               viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }

    private void DespawnFromOffScreen()
    {
        if (_isDespawning)
            return;

        _isDespawning = true;
        CancelInvoke(nameof(Despawn));
        Despawn();
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}