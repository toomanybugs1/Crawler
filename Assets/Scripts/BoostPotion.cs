using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPotion : Item
{
    public int type;

    public override Item Clone()
    {
        return (Item)MemberwiseClone();
    }

    public override void UseItem(Player player)
    {
        PowerController.PowerUp(type);
    }
}
