using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Transform> doors;
    public List<Vector3> enemySpawnPoints;
    public List<Vector3> itemSpawnPoints;
    public List<Vector3> weaponSpawnPoints;
    public List<Vector3> objectSpawnPoints;
    //active tells us that the player is in range for enemies to move
    bool doorsOpen, active;
    public int enemyNums;


    private void Update()
    {
        if (enemyNums <= 0 && !doorsOpen && doors != null)
        {
            foreach(Transform door in doors)
            {
                if(door != null)
                    Destroy(door.gameObject);
            }

            doorsOpen = true;
        }
    }

    public Vector3 GetCenterPositionInRoom()
    {
        return transform.position;
    }

    public void addEnemy()
    {
        enemyNums++;
    }

    public void subtractEnemy()
    {
        enemyNums--;
    }

    public bool IsActive()
    {
        return active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            active = true;
        }
    }
}
