using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] possibleObjects;
    public float roomPadding;

    public void GenerateObjectSpawnPointsForRoom(RoomNode room)
    {
        //room is the corner coords, roomObj is the actual room script

        Vector3 randCoords;
        float distanceBetween = Vector2Int.Distance(room.BottomLeftCorner, room.TopRightCorner);
        int objectsToSpawn;

        //distance is used because we cant easily get the area with the corners, but the distance between
        //two points gives us a bit of information on how far apart these are
        //for now we're gonna spawn 1 enemy for every 10 units of distance, starting at 2
        if (distanceBetween < 5)
            objectsToSpawn = 1;
        else
        {
            int everyNum = (int)(distanceBetween / 8);
            objectsToSpawn = everyNum + 1;
        }

        for (int i = 0; i < objectsToSpawn; i++)
        {
            randCoords = new Vector3(
                Random.Range(room.BottomLeftCorner.x + roomPadding, room.BottomRightCorner.x - roomPadding),
                0,
                Random.Range(room.BottomLeftCorner.y + roomPadding, room.TopLeftCorner.y - roomPadding)
                );

            room.roomObjReference.objectSpawnPoints.Add(randCoords);
        }
    }

    public void SpawnObjects(Room room)
    {
        foreach (Vector3 position in room.objectSpawnPoints)
        {
            Vector3 randRotation = new Vector3(-90, 0, Random.Range(0, 360));
            Instantiate(possibleObjects[Random.Range(0, possibleObjects.Length)], position, Quaternion.Euler(randRotation));
        }
    }
}
