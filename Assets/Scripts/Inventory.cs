using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static List<Item> items = new List<Item>();
    public Image[] itemImgHolders;
    Player player;

    public void Start()
    {
        player = GetComponent<Player>();
        SetImages();
    }
    public static void AddItem(Item item)
    {
        if (items.Count >= 5)
            return;
        
        else if (item != null)
        {
            items.Add(item);
            Destroy(item.gameObject);
        }
    }

    public static void EmptyInventory()
    {
        items = new List<Item>();
    }

    public void SetImages()
    {
        for (int i = 0; i < itemImgHolders.Length; i++)
            itemImgHolders[i].enabled = false;

        for (int i = 0; i < items.Count; i++)
        {
            itemImgHolders[i].sprite = items[i].sprite;
            itemImgHolders[i].enabled = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (items.Count >= 1)
            {
                items[0].UseItem(player);
                items.RemoveAt(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (items.Count >= 2)
            {
                items[1].UseItem(player);
                items.RemoveAt(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (items.Count >= 3)
            {
                items[2].UseItem(player);
                items.RemoveAt(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (items.Count >= 4)
            {
                items[3].UseItem(player);
                items.RemoveAt(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (items.Count == 5)
            {
                items[4].UseItem(player);
                items.RemoveAt(4);
            }
        }

        SetImages();
    }
}
