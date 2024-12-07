using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChuncks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk; 
    PlayerMovement pm;
    private string playerSide;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDistance; //Must be greater than the length and width of the Tilemap
    float opDistance;
    float optCooldown;
    public float optCooldownDuration;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptmizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        if (pm.moveDir != Vector2.zero)
        {
            CheckAndSpawnChunk("Right");
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Top");
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Top-Left");
            CheckAndSpawnChunk("Top-Right");
            CheckAndSpawnChunk("Down-Right");
            CheckAndSpawnChunk("Down-Left");
        }

    }

    private void CheckAndSpawnChunk(string direction)
    {
        Transform directionTransform = currentChunk.transform.Find(direction);
        if (directionTransform != null && !Physics2D.OverlapCircle(directionTransform.position, checkerRadius, terrainMask))
        {
            noTerrainPosition = directionTransform.position;
            ChunkSpawner();
        }
    }

    void ChunkSpawner()
    {
        var rand = Random.Range(0, terrainChuncks.Count);
        latestChunk = Instantiate(terrainChuncks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    private void ChunkOptmizer()
    {
        optCooldown -= Time.deltaTime;
        if (optCooldown <= 0)
        {
            optCooldown = optCooldownDuration;
        }
        else
        {
            return;
        }

        foreach (var chunk in spawnedChunks)
        {
            if (player != null)
            {
                opDistance = Vector3.Distance(player.transform.position, chunk.transform.position);
                if (opDistance > maxOpDistance)
                {
                    chunk.SetActive(false);
                }
                else
                {
                    chunk.SetActive(true);
                }
            }
    }
}
}
