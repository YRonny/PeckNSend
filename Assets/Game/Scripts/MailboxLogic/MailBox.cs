using UnityEngine;

public class Mailbox : MonoBehaviour
{
    [Header("Shooting Trigger")]
    public float shootTriggerX = -5f;

    public MailboxType mailboxType;
    private bool hasTriggeredShoot = false;

    //public void AssignRandomType()
    //{
    //    int count = System.Enum.GetValues(typeof(MailboxType)).Length;
    //    mailboxType = (MailboxType)Random.Range(0, count);

    //    ApplyPaletteColor(mailboxType);
    //    Debug.Log($"Mailbox spawned with type: {mailboxType}", this);
    //}

    //void ApplyPaletteColor(MailboxType type)
    //{
    //    Renderer rend = GetComponent<Renderer>();
    //    if (rend == null) return;

    //    Material[] materials = rend.materials;
    //    if (materials.Length < 2)
    //    {
    //        Debug.LogWarning("Mailbox needs 2 materials! (Stick + Box)", this);
    //        return;
    //    }

    //    // Box is Material[1]
    //    Material boxMat = materials[1];

    //    // Get UV offset: X = column, Y = fixed row
    //    Vector2 offset = GetPaletteOffsetForType(type);
    //    boxMat.SetTextureOffset("_MainTex", offset);

    //    Debug.Log($"Mailbox {type} → offset {offset}");
    //}

    // 5x5 grid, use only ROW 1
    //Vector2 GetPaletteOffsetForType(MailboxType type)
    //{
    //    int cols = 5;
    //    int fixedRow = 1;          // Always Row 1

    //    int colIndex = type switch
    //    {
    //        MailboxType.Type0 => 0,
    //        MailboxType.Type1 => 1,
    //        MailboxType.Type2 => 2,
    //        MailboxType.Type3 => 3,
    //        MailboxType.Type4 => 4,
    //        _ => 0
    //    };

    //    float x = (float)colIndex / cols;   // 0/5, 1/5, 2/5, 3/5, 4/5
    //    float y = (float)fixedRow / cols;   // 1/5 = always row 1

    //    return new Vector2(x, y);
    //}

    void Update()
    {
        if (!hasTriggeredShoot && transform.position.x >= shootTriggerX)
        {
            hasTriggeredShoot = true;

            // FIND SHOOTER and tell it our type
            MailShooter shooter = FindAnyObjectByType<MailShooter>();
            shooter?.OnMailboxTriggered(mailboxType);

            //Debug.Log($"✅ Mailbox {mailboxType} → notified shooter");
        }
    }

}
