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

        playerSide = CheckPlayerSide();

        if (string.IsNullOrEmpty(playerSide))
        {
            return;
        }

        Transform sideTransform = currentChunk.transform.Find(playerSide);
        if (sideTransform != null && !Physics2D.OverlapCircle(sideTransform.position, checkerRadius, terrainMask))
        {
            Debug.Log(playerSide);
            noTerrainPosition = sideTransform.position;
            ChunkSpawner();

            if (playerSide.Contains("Up") && playerSide.Contains("Right"))
            {
                CheckAndSpawnChunk("Up");
                CheckAndSpawnChunk("Right");
            }
            else if (playerSide.Contains("Up") && playerSide.Contains("Left"))
            {
                CheckAndSpawnChunk("Up");
                CheckAndSpawnChunk("Left");
            }
            else if (playerSide.Contains("Down") && playerSide.Contains("Right"))
            {
                CheckAndSpawnChunk("Down");
                CheckAndSpawnChunk("Right");
            }
            else if (playerSide.Contains("Down") && playerSide.Contains("Left"))
            {
                CheckAndSpawnChunk("Down");
                CheckAndSpawnChunk("Left");
            }
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

    private string CheckPlayerSide()
    {
        if (pm.moveDir.x > 0 && pm.moveDir.y == 0) //right
        {
            return "Right";
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0) //left
        {
            return "Left";
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0) //top
        {
            return "Top";
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0) //down
        {
            return "Down";
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0) //top-right
        {
            return "Top-Right";
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0) //top-left
        {
            return "Top-Left";
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0) //down-right
        {
            return "Down-Right";
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0) //down-left
        {
            return "Down-Left";
        }
        else
        {
            return string.Empty;
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
