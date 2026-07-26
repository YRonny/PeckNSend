using UnityEngine;

public class MailWaveMovement : MonoBehaviour
{
    [Header("Base Movement")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private Vector3 moveDirection = Vector3.right;
    [SerializeField] private Vector3 waveDirection = Vector3.up;

    [Header("Base Wave")]
    [SerializeField] private float baseWaveAmplitude = 0.4f;
    [SerializeField] private float baseWaveFrequency = 5f;

    [Header("Variation")]
    [SerializeField] private float amplitudeVariation = 0.35f;
    [SerializeField] private float frequencyVariation = 1.25f;
    //[SerializeField] private float speedVariation = 1f;
    [SerializeField] private float sidewaysDriftVariation = 0.15f;

    private Vector3 startPosition;
    private Vector3 actualMoveDirection;

    private float aliveTime;
    private float waveAmplitude;
    private float waveFrequency;
    private float actualForwardSpeed;
    private float randomPhase;

    void Start()
    {
        startPosition = transform.position;

        moveDirection.z = 0f;
        waveDirection.z = 0f;

        moveDirection = moveDirection.normalized;
        waveDirection = waveDirection.normalized;

        GenerateWaveProfile();
    }

    void Update()
    {
        aliveTime += Time.deltaTime;

        Vector3 forwardOffset = actualMoveDirection * forwardSpeed * aliveTime;
        Vector3 waveOffset = waveDirection * Mathf.Sin(aliveTime * waveFrequency + randomPhase) * waveAmplitude;

        Vector3 newPosition = startPosition + forwardOffset + waveOffset;
        newPosition.z = startPosition.z;

        transform.position = newPosition;
    }

    private void GenerateWaveProfile()
    {
        waveAmplitude = GetRandomizedValue(baseWaveAmplitude, amplitudeVariation, 0.05f);
        waveFrequency = GetRandomizedValue(baseWaveFrequency, frequencyVariation, 0.1f);
        //actualForwardSpeed = GetRandomizedValue(forwardSpeed, speedVariation, 0.1f);

        randomPhase = Random.Range(0f, Mathf.PI * 2f);

        Vector3 sidewaysOffset = new Vector3(
            Random.Range(-sidewaysDriftVariation, sidewaysDriftVariation),
            Random.Range(-sidewaysDriftVariation, sidewaysDriftVariation),
            0f
        );

        actualMoveDirection = moveDirection + sidewaysOffset;
        actualMoveDirection.z = 0f;
        actualMoveDirection = actualMoveDirection.normalized;
    }

    private float GetRandomizedValue(float baseValue, float variation, float minValue = 0f)
    {
        float result = baseValue + Random.Range(-variation, variation);
        return Mathf.Max(minValue, result);
    }
}