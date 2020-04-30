using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator
{
    private int dungeonWidth;
    private int dungeonLength;
    
    List<RoomNode> allSpaceNodes = new List<RoomNode>();

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;

    }
    
    public List<Node> CalculateDungeon(int maxPasses, int roomMinWidth, int roomMinLength, float bottomCornerModifier, float topCornerModifier, int roomOffset, int corridorWidth, EnemySpawner enemSpawner, DungeonCreator dungeonCreator, ItemSpawner itemSpawner, ObjectSpawner objSpawner)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxPasses, roomMinWidth, roomMinLength);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.rootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxPasses, roomMinLength, roomMinWidth);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, bottomCornerModifier, topCornerModifier, roomOffset);
        //first room will be the spawn point for the player
        RoomNode firstRoom = roomList[0];

        UnityEngine.CharacterController player = GameObject.FindWithTag("Player").GetComponent<UnityEngine.CharacterController>();
        Vector2Int firstRoomCenter = StructureHelper.CalculateMiddlePoint(firstRoom.BottomLeftCorner, firstRoom.TopRightCorner);
        Vector3 newPos = new Vector3(firstRoomCenter.x, 0.0f, firstRoomCenter.y);
        player.enabled = false;
        player.transform.position = newPos;
        player.enabled = true;

        //generate spawn points before we add the corridors
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomNode room = roomList[i];

            if (enemSpawner != null)
            {
                GameObject newRoom = new GameObject("RoomObj", typeof(Room), typeof(BoxCollider));
                newRoom.tag = "RoomObject";
                BoxCollider col = newRoom.GetComponent<BoxCollider>();
                col.isTrigger = true;
                col.size = new Vector3(1.2f, 1.2f, 1.2f);

                Vector2Int roomPos = StructureHelper.CalculateMiddlePoint(room.BottomLeftCorner, room.TopRightCorner);
                newRoom.transform.position = new Vector3(roomPos.x, 2, roomPos.y);
                Room roomComp = newRoom.GetComponent<Room>();
                room.roomObjReference = roomComp;
                newRoom.transform.localScale = new Vector3(room.Width, 4, room.Length);
                roomComp.enemySpawnPoints = new List<Vector3>();
                roomComp.itemSpawnPoints = new List<Vector3>();
                roomComp.weaponSpawnPoints = new List<Vector3>();
                roomComp.objectSpawnPoints = new List<Vector3>();
                roomComp.doors = new List<Transform>();
                dungeonCreator.spawnedRooms.Add(roomComp);

                if (i != 0)
                {
                    enemSpawner.GenerateEnemySpawnPointsForRoom(room);
                    itemSpawner.GenerateItemSpawnPointsForRoom(room);
                    objSpawner.GenerateObjectSpawnPointsForRoom(room);
                }
            }
        }

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allSpaceNodes, corridorWidth);
        dungeonCreator.GenerateDoors(corridorList);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}
