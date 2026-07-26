using UnityEngine;

public class StreetItemSpawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Options")]
    //[SerializeField] private bool useSpawnPointRotation = true;
    [SerializeField] private Transform parentForSpawnedObjects;

    void Start()
    {
        SpawnAll();
    }

    void SpawnAll()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            //Debug.LogWarning("No prefabs assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            //Debug.LogWarning("No spawn points assigned.");
            return;
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;

            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
            //Quaternion rotation = useSpawnPointRotation ? spawnPoint.rotation : randomPrefab.transform.rotation;

            var childObject =Instantiate(randomPrefab, spawnPoint.position, randomPrefab.transform.rotation, parentForSpawnedObjects);
            childObject.transform.parent = this.transform; // Set the spawner as the parent of the spawned object
        }
    }
}