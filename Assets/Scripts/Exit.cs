using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Item
{
    public override Item Clone()
    {
        Debug.Log("Does not clone.");
        return null;
    }

    public override void UseItem(Player player)
    {
        GameController.NextLevel();
    }
}
