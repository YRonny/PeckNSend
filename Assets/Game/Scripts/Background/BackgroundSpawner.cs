using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    //Test so i can submit again, ignore this unfdsuinfnoisoin
    [Header("Setup")]
    public GameObject[] buildingPrefabs;
    public float moveSpeed = 2f;
    public float rightDespawnX = 50f;
    public int tilesOnScreen = 5;
    public float Depth;    //How far back the buildings are (Z-axis)
    private Vector3 BuildingRot = new Vector3(0,-90,0);


    public List<BuildingMover> activeTiles = new List<BuildingMover>();
    private float leftmostEdge = float.MinValue;
    private bool isSpawning = false; // Prevents race conditions

    void Start()
    {

        SpawnInitialChain(); //Spawn initial buildings to fill the screen at the start
    }

    void Update()
    {
        UpdateLeftmostEdge();
    }

    void SpawnInitialChain()
    {
        // Start with first tile at the right edge
        SpawnSingleTile(45f);

        // Spawn tile to the left of the previously spawned tile (left edge of a tile is the reference)
        for (int i = 1; i < tilesOnScreen; i++)
        {
            UpdateLeftmostEdge();
            SpawnSingleTile(leftmostEdge);
        }
    }

    void UpdateLeftmostEdge() //Checks what tile is the most left
    {
        leftmostEdge = float.MaxValue;
        foreach (var tile in activeTiles)
        {
            if (tile != null)
            {
                float leftEdge = tile.transform.position.x - (tile.width * 0.5f);
                if (leftEdge < leftmostEdge)
                    leftmostEdge = leftEdge;
            }
        }
    }

    void SpawnSingleTile(float previousLeftEdge)
    {
        if (isSpawning) return;
        isSpawning = true;

        // 1. Pick and instantiate prefab
        GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        GameObject tileObj = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);

        // 2. Measure width
        Renderer rend = tileObj.GetComponentInChildren<Renderer>();
        float actualWidth = rend != null ? rend.bounds.size.x : 3f;

        // 3. Give it moverScript and setup
        BuildingMover mover = tileObj.GetComponent<BuildingMover>();
        if (mover == null) mover = tileObj.AddComponent<BuildingMover>();
        mover.Setup(moveSpeed, rightDespawnX, this, actualWidth);

        // 4. previousLeftEdge - (width/2)
        float centerX = previousLeftEdge - (actualWidth * 0.5f);
        tileObj.transform.position = new Vector3(centerX, 0f, Depth);

        // 5. Add to tracking
        activeTiles.Add(mover);

        // MailboxSystem, can be further implemented when more scripts are added.
        BuildingMailboxSpawner mailboxSpawner = tileObj.GetComponent<BuildingMailboxSpawner>();
        if (mailboxSpawner != null)
        {
            mailboxSpawner.InitMailboxes();
        }


        isSpawning = false;
    }


    public void RequestNewTile()
    {
        if (isSpawning || activeTiles.Count >= tilesOnScreen * 2) return;

        UpdateLeftmostEdge();
        SpawnSingleTile(leftmostEdge);
    }
}
