using UnityEngine;

public class HouseScalePulse : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private Vector3 scaleAmountMin = new Vector3(0.02f, 0.02f, 0.02f);
    [SerializeField] private Vector3 scaleAmountMax = new Vector3(0.05f, 0.05f, 0.05f);
    [SerializeField] private float speedMin = 0.8f;
    [SerializeField] private float speedMax = 1.6f;

    private Vector3 baseScale;
    private Vector3 scaleAmount;
    private float speed;
    private float phaseOffset;

    private void Awake()
    {
        baseScale = transform.localScale;
        scaleAmount = new Vector3(
            Random.Range(scaleAmountMin.x, scaleAmountMax.x),
            Random.Range(scaleAmountMin.y, scaleAmountMax.y),
            Random.Range(scaleAmountMin.z, scaleAmountMax.z)
        );
        speed = Random.Range(speedMin, speedMax);
        phaseOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float wave = (Mathf.Sin((Time.time + phaseOffset) * speed) + 1f) * 0.5f;
        transform.localScale = baseScale + Vector3.Scale(scaleAmount, Vector3.one * wave);
    }
}