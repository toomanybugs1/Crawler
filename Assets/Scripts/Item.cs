using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;

    public abstract void UseItem(Player player);
    public abstract Item Clone();
    
}
