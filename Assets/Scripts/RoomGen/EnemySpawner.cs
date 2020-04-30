using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] possibleEnemies;
    public float roomPadding;

    public void GenerateEnemySpawnPointsForRoom(RoomNode room)
    {
        //room is the corner coords, roomObj is the actual room script

        Vector3 randCoords;
        float distanceBetween = Vector2Int.Distance(room.BottomLeftCorner, room.TopRightCorner);
        int enemiesToSpawn;

        //distance is used because we cant easily get the area with the corners, but the distance between
        //two points gives us a bit of information on how far apart these are
        //for now we're gonna spawn 1 enemy for every 10 units of distance, starting at 2
        if (distanceBetween <= 5)
            enemiesToSpawn = 1;
        else
        {
            int tens = (int) (distanceBetween / 10);
            enemiesToSpawn = tens + 1;
        }
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            randCoords = new Vector3(
                Random.Range(room.BottomLeftCorner.x + roomPadding, room.BottomRightCorner.x - roomPadding),
                1,
                Random.Range(room.BottomLeftCorner.y + roomPadding, room.TopLeftCorner.y - roomPadding)
                );

            room.roomObjReference.enemySpawnPoints.Add(randCoords);
        }
    }

    public void SpawnEnemies(Room room)
    {
        foreach (Vector3 position in room.enemySpawnPoints)
        {
            //mark the actual enemy such that the room will know when its killed
            GameObject newEnemy = Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Length)], position, Quaternion.identity);
            Enemy enemComp = newEnemy.GetComponent<Enemy>();
            enemComp.SetRoom(room);
            room.addEnemy();
            Vector3 newPosition = new Vector3(position.x, enemComp.startYPos, position.z);
            newEnemy.transform.position = newPosition;
        }
    }

}
