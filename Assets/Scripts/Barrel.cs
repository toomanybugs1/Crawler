using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject[] items;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Vector3 spawnPos = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Instantiate(items[Random.Range(0, items.Length)], spawnPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
