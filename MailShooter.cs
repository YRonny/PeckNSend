using System.Collections.Generic;
using UnityEngine;

public class MailShooter : MonoBehaviour
{
    [SerializeField] private GameObject[] mailPrefabs; // 3 mail prefabs
    [SerializeField] private GameObject[] fragileMailPrefabs;
    [SerializeField] private GameObject[] heavyMailPrefabs;

    [Header("Auto Shoot")]
    [SerializeField] private float shootDelay = 1.5f;
    private float shootTimer = 0f;

    private MailboxType currentMailType;
    private bool hasMailType = false;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootDelay && hasMailType)
        {
            ShootMailMatchingMailboxColor();
            Invoke(nameof(ShootMailMatchingMailboxColor), 1f);

            shootTimer = 0f;
            hasMailType = false;
        }
    }

    public void OnMailboxTriggered(MailboxType type) // Called by Mailbox
    {
        currentMailType = type;
        hasMailType = true;
        shootTimer = 0f;
    }

    void ShootMailMatchingMailboxColor()
    {
        GameObject mailPrefab = FindCorrectMail(currentMailType);
        
        // Spawn mail
        GameObject mail = Instantiate(mailPrefab, transform.position, mailPrefab.transform.rotation);

        // Random flight pattern
        Vector2 randomPattern = new Vector2(
            Random.Range(0.8f, 1.4f),
            Random.Range(-0.4f, 0.6f)
        ).normalized + new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.15f, 0.15f));

        Rigidbody rb = mail.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(randomPattern.normalized * 700f, ForceMode.Force);
            rb.AddForce(Vector3.up * 300f, ForceMode.Force);
            rb.AddTorque(Random.Range(-200f, 200f),0,0);
        }

        //Debug.Log($"📬 Shot {currentMailType} colored mail (prefab )");
    }

    private GameObject FindCorrectMail(MailboxType type)
    {
        // Step 1: pick ONE random list
        GameObject[] listToUse = GetRandomArray(mailPrefabs, fragileMailPrefabs, heavyMailPrefabs);

        // Step 2: search inside that list
        for (int i = 0; i < listToUse.Length; i++)
        {
            MailTypeAssigner assigner = listToUse[i].GetComponent<MailTypeAssigner>();

            if (assigner != null && assigner.mailType == type)
            {
                return listToUse[i];
            }
        }

        return null;
    }
    private GameObject[] GetRandomArray(GameObject[] a, GameObject[] b, GameObject[] c)
    {
        GameObject[][] arrays = new GameObject[][] { a, b, c };
        return arrays[Random.Range(0, arrays.Length)];
    }
}