using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private CorridorFirstDungeonGenerator dungeonGenerator;
    [SerializeField]
    private GameManager gameManager;


    [Header("Enemy")]
    [SerializeField]
    private GameObject enemyPrefab;

    [Header("Tuning")]
    [Tooltip("How many enemies to spawn in each room (can be fractional)")]
    [SerializeField]
    public float enemiesPerRoom = 1f;

    public float dificultyIncrease = 0.3f;


    public void SpawnEnemiesExceptRoom(int excludedRoomIndex)
    {
        enemiesPerRoom += (gameManager.level - 1) * 0.3f;
        var rooms = dungeonGenerator.Rooms;
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: no rooms to spawn in.");
            return;
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == excludedRoomIndex) continue;
            var tiles = rooms[i].ToList();

            int baseCount = Mathf.FloorToInt(enemiesPerRoom);
            float frac = enemiesPerRoom - baseCount;
            int spawnCount = baseCount + (Random.value < frac ? 1 : 0);

            for (int k = 0; k < spawnCount; k++)
            {
                var cell = tiles[Random.Range(0, tiles.Count)];
                Vector3 pos = new Vector3(cell.x, cell.y, 0f);
                Instantiate(enemyPrefab, pos, Quaternion.identity);
            }
        }
    }
}