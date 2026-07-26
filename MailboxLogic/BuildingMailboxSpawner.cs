using UnityEngine;

public class BuildingMailboxSpawner : MonoBehaviour
{
    [Header("Mailbox Anchors")]
    public Transform[] mailboxAnchors;
    

    [Header("Mailbox Prefabs")]
    public GameObject[] airMailboxes;
    public GameObject[] groundMailboxes;

    private bool initialized = false;
    private GameObject _mailBox;

    public void InitMailboxes()
    {
        if (initialized) return;
        initialized = true;

        // Pick 1 random anchor
        if (mailboxAnchors.Length == 0 || airMailboxes.Length == 0 || groundMailboxes.Length == 0) return;

        //Get anchor and prefab from lists
        Transform randomAnchor = mailboxAnchors[Random.Range(0, mailboxAnchors.Length)];

        if (randomAnchor.gameObject.CompareTag("AirAnchor"))
        {
            _mailBox = airMailboxes[Random.Range(0, airMailboxes.Length)];
        }
        else if (randomAnchor.gameObject.CompareTag("GroundAnchor"))
        {
            _mailBox = groundMailboxes[Random.Range(0, groundMailboxes.Length)];
        }

        //Spawn mailbox
        GameObject mailboxObj = Instantiate(_mailBox, randomAnchor.position, _mailBox.transform.rotation, randomAnchor);
                        
        
    }
}
