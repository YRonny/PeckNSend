using UnityEngine;

public class BuildingMover : MonoBehaviour
{
    [HideInInspector] public float width;
    private float speed, despawnX;
    private BackgroundSpawner spawner;
    private static bool spawnRequested = false; // Global lock

    public void Setup(float moveSpeed, float despawnPoint, BackgroundSpawner manager, float tileWidth)
    {
        speed = moveSpeed;
        despawnX = despawnPoint;
        spawner = manager;
        width = tileWidth;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        // ONLY the rightmost building triggers spawn
        if (transform.position.x > despawnX && !spawnRequested)
        {
            spawnRequested = true;

            if (spawner != null)
            {
                spawner.activeTiles.Remove(this);
                spawner.RequestNewTile();
            }

            Destroy(gameObject);
            spawnRequested = false; // Resets spawnRequested (To go against buildings spawning into eachother)
        }
    }
}
