using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public int recoverAmt;

    public override Item Clone()
    {
        return (Item) MemberwiseClone();
    }

    public override void UseItem(Player player)
    {
        player.health += recoverAmt;
    }
}
