using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject barrel;
    public GameObject[] possibleWeapons;
    public float roomPadding;

    public void GenerateItemSpawnPointsForRoom(RoomNode room)
    {
        //we don't care about the room size, each room will simply
        //have a chance of getting an item or sword

        float itemRoll = Random.value;
        float weaponRoll = Random.value;

        int itemsToSpawn = 0;
        int weaponsToSpawn = 0;

        if (itemRoll <= 0.2)
        {
            itemsToSpawn++;

            if (itemRoll <= 0.1)
                itemsToSpawn++;
        }

        if (weaponRoll <= 0.2)
        {
            weaponsToSpawn++;

            if (itemRoll <= 0.05)
                weaponsToSpawn++;
        }

        Vector3 randCoords;
        for (int i = 0; i < itemsToSpawn; i++)
        {
            randCoords = new Vector3(
            Random.Range(room.BottomLeftCorner.x + roomPadding, room.BottomRightCorner.x - roomPadding),
            1,
            Random.Range(room.BottomLeftCorner.y + roomPadding, room.TopLeftCorner.y - roomPadding)
            );
            room.roomObjReference.itemSpawnPoints.Add(randCoords);
        }

        for (int i = 0; i < weaponsToSpawn; i++)
        {
            randCoords = new Vector3(
            Random.Range(room.BottomLeftCorner.x + roomPadding, room.BottomRightCorner.x - roomPadding),
            1,
            Random.Range(room.BottomLeftCorner.y + roomPadding, room.TopLeftCorner.y - roomPadding)
            );
            room.roomObjReference.weaponSpawnPoints.Add(randCoords);
        }
    }

    public void SpawnItems(Room room)
    {
        for(int i = 0; i < room.itemSpawnPoints.Count; i++)
        {
            GameObject newItem = Instantiate(barrel, room.itemSpawnPoints[i], Quaternion.Euler(Vector3.right * -90));
        }

        for (int i = 0; i < room.weaponSpawnPoints.Count; i++)
        {
            GameObject newItem = Instantiate(possibleWeapons[Random.Range(0, possibleWeapons.Length)], room.weaponSpawnPoints[i], Quaternion.identity);
        }
    }

}

