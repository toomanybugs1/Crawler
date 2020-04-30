using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomMinWidth, roomMinLength;
    public int maxPasses;
    public int corridorWidth;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float bottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float topCornerModifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal, wallVWithTorch, wallHWithTorch;
    public GameObject doorObject;
    public GameObject nextRoom;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;

    EnemySpawner enemySpawner;
    ItemSpawner itemSpawner;
    ObjectSpawner objSpawner;
    public List<Room> spawnedRooms;

    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        itemSpawner = GetComponent<ItemSpawner>();
        objSpawner = GetComponent<ObjectSpawner>();
        CreateDungeon();
        foreach (Room room in spawnedRooms)
        {
            enemySpawner.SpawnEnemies(room);
            itemSpawner.SpawnItems(room);
            objSpawner.SpawnObjects(room);
        }
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition =  new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();

        var listOfRooms = generator.CalculateDungeon(maxPasses, roomMinWidth, roomMinLength, bottomCornerModifier, topCornerModifier, roomOffset, corridorWidth, enemySpawner, this, itemSpawner, objSpawner);
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftCorner, listOfRooms[i].TopRightCorner);
        }

        CreateWalls(wallParent);

        GetComponent<ExitSpawner>().SpawnExit();

    }

    private void CreateWalls(GameObject wallParent)
    {
        for (int i = 0; i < possibleWallHorizontalPosition.Count; i++)
        {
            if (i % 8 == 0)
                CreateWall(wallParent, possibleWallHorizontalPosition[i], wallHWithTorch);

            else
                CreateWall(wallParent, possibleWallHorizontalPosition[i], wallHorizontal);
        }
        for (int i = 0; i < possibleWallVerticalPosition.Count; i++)
        {
            if (i % 8 == 0)
                CreateWall(wallParent, possibleWallVerticalPosition[i], wallVWithTorch);

            else
                CreateWall(wallParent, possibleWallVerticalPosition[i], wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeft, Vector2 topRight)
    {
        Vector3 bottomLeftCorner = new Vector3(bottomLeft.x, 0, bottomLeft.y);
        Vector3 bottomRightCorner = new Vector3(topRight.x, 0, bottomLeft.y);
        Vector3 topLeftCorner = new Vector3(bottomLeft.x, 0, topRight.y);
        Vector3 topRightCorner = new Vector3(topRight.x, 0, topRight.y);

        Vector3[] verticies = new Vector3[]
        {
            topLeftCorner,
            topRightCorner,
            bottomLeftCorner,
            bottomRightCorner
        };

        Vector2[] uvs = new Vector2[verticies.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verticies[i].x, verticies[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject floor = new GameObject("Mesh "+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = Vector3.one;
        floor.GetComponent<MeshFilter>().mesh = mesh;
        floor.GetComponent<MeshRenderer>().material = material;
        floor.transform.parent = transform;
        floor.GetComponent<MeshCollider>().sharedMesh = mesh;

        for (int row = (int)bottomLeftCorner.x; row < (int)bottomRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftCorner.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftCorner.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightCorner.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftCorner.z; col < (int)topLeftCorner.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftCorner.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightCorner.z; col < (int)topRightCorner.z; col++)
        {
            var wallPosition = new Vector3(bottomRightCorner.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }

    public void GenerateDoors(List<Node> corridors)
    {
        foreach(CorridorNode node in corridors)
        {
            if (node.connectedRooms[0] != null && node.connectedRooms[1] != null)
            {
                Vector2Int center = StructureHelper.CalculateMiddlePoint(node.BottomLeftCorner, node.TopRightCorner);
                if (node.orientation == RelativePosition.Up || node.orientation == RelativePosition.Down)
                {
                    GameObject newDoor = Instantiate(doorObject, new Vector3(center.x, 0, center.y), Quaternion.Euler(new Vector3(-90, 0, 0)));
                    node.connectedRooms[0].doors.Add(newDoor.transform);
                    node.connectedRooms[1].doors.Add(newDoor.transform);
                }
                else
                {
                    GameObject newDoor = Instantiate(doorObject, new Vector3(center.x, 0, center.y), Quaternion.Euler(new Vector3(-90, 90, 0)));
                    node.connectedRooms[0].doors.Add(newDoor.transform);
                    node.connectedRooms[1].doors.Add(newDoor.transform);
                }
            }
        }
    }
}
