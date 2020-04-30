using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSpawner : MonoBehaviour
{
    public GameObject exitObj;

    public void SpawnExit()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("RoomObject");
        Room room = rooms[Random.Range(0, rooms.Length)].GetComponent<Room>();
        Vector3 roomCenter = room.GetCenterPositionInRoom();
        roomCenter.y = 0;

        Instantiate(exitObj, roomCenter, Quaternion.Euler(new Vector3(-90, 0, 0)), null);
    }

}
