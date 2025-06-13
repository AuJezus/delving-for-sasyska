using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CorridorFirstDungeonGenerator dungeonGenerator;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject player;

    public int level = 0;

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        level++;

        // Clear any old objects
        var gameObjectsToClear = GameObject.FindGameObjectsWithTag("Enemy").Concat(GameObject.FindGameObjectsWithTag("Dropable"));
        foreach (var e in gameObjectsToClear)
            Destroy(e);

        // Subscribe to the generator’s completion event
        dungeonGenerator.OnDungeonGenerated += OnDungeonReady;

        // Start generation
        dungeonGenerator.GenerateDungeon();
    }

    private void OnDungeonReady()
    {
        dungeonGenerator.OnDungeonGenerated -= OnDungeonReady;

        var rooms = dungeonGenerator.Rooms;
        var floor = dungeonGenerator.FloorPositions;
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogError("GameManager: No rooms were generated!");
            return;
        }

        // Pick a random room
        int playerRoomIdx = Random.Range(0, rooms.Count);
        var roomSet = rooms[playerRoomIdx];
        var roomTiles = roomSet.ToList();

        // Build list of inner tiles whose 4‐way neighbours are all floor
        var innerTiles = roomTiles.Where(tile =>
          // for each cardinal direction, the adjacent tile is in floorPositions
          Direction2D.cardinalDirectionsList
            .All(dir => floor.Contains(tile + dir))
        ).ToList();

        // Choose from inner tiles if possible, otherwise fallback
        Vector2Int spawnCell = innerTiles.Count > 0
          ? innerTiles[Random.Range(0, innerTiles.Count)]
          : roomTiles[Random.Range(0, roomTiles.Count)];

        player.transform.position = new Vector3(spawnCell.x, spawnCell.y, 0f);

        // Spawn enemies in the other rooms
        enemySpawner.SpawnEnemiesExceptRoom(playerRoomIdx);
    }
}