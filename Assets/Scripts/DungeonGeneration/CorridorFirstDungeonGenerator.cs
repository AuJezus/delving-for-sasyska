using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1f)]
    private float roomPercent = 0.8f;

    public HashSet<Vector2Int> FloorPositions { get; private set; }
    public List<HashSet<Vector2Int>> Rooms { get; private set; }
    public event Action OnDungeonGenerated;

    public override void GenerateDungeon()
    {
        Rooms = new List<HashSet<Vector2Int>>();
        FloorPositions = new HashSet<Vector2Int>();
        tilemapVisualizer.Clear();

        base.GenerateDungeon();

        OnDungeonGenerated?.Invoke();
    }

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        var floorPositions = new HashSet<Vector2Int>();
        var potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        var roomFloors = CreateRooms(potentialRoomPositions);
        CreateRoomsAtDeadEnd(FindAllDeadEnds(floorPositions), roomFloors);

        floorPositions.UnionWith(roomFloors);
        FloorPositions = floorPositions;

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateCorridors(
      HashSet<Vector2Int> floorPositions,
      HashSet<Vector2Int> potentialRoomPositions
    )
    {
        var currentPos = startPosition;
        potentialRoomPositions.Add(currentPos);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms
                             .RandomWalkCorridor(currentPos, corridorLength);
            currentPos = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPos);
            floorPositions.UnionWith(corridor);
        }
    }

    private HashSet<Vector2Int> CreateRooms(
      HashSet<Vector2Int> potentialRoomPositions
    )
    {
        var roomPositions = new HashSet<Vector2Int>();
        int createCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        var chosen = potentialRoomPositions
                     .OrderBy(_ => Guid.NewGuid())
                     .Take(createCount);

        foreach (var pos in chosen)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, pos);
            roomPositions.UnionWith(roomFloor);
            Rooms.Add(roomFloor);
        }
        return roomPositions;
    }

    private void CreateRoomsAtDeadEnd(
      List<Vector2Int> deadEnds,
      HashSet<Vector2Int> roomFloors
    )
    {
        foreach (var pos in deadEnds)
        {
            if (!roomFloors.Contains(pos))
            {
                var roomFloor = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(roomFloor);
                Rooms.Add(roomFloor);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(
      HashSet<Vector2Int> floorPositions
    )
    {
        var deadEnds = new List<Vector2Int>();
        foreach (var pos in floorPositions)
        {
            int neighbors = 0;
            foreach (var dir in Direction2D.cardinalDirectionsList)
                if (floorPositions.Contains(pos + dir))
                    neighbors++;
            if (neighbors == 1) deadEnds.Add(pos);
        }
        return deadEnds;
    }
}