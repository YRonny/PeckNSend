using UnityEngine;

public class LockTo2DPlane : MonoBehaviour
{
    [SerializeField] private bool _captureInitialZOnStart = true;
    [SerializeField] private float _lockedZ = 0f;

    private void Start()
    {
        if (_captureInitialZOnStart)
        {
            _lockedZ = transform.position.z;
        }

        ForceZPosition();
    }

    private void LateUpdate()
    {
        ForceZPosition();
    }

    private void ForceZPosition()
    {
        Vector3 position = transform.position;
        if (!Mathf.Approximately(position.z, _lockedZ))
        {
            position.z = _lockedZ;
            transform.position = position;
        }
    }
}