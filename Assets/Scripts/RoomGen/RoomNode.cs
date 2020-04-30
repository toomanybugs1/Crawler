using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : Node
{
    public Room roomObjReference;

    public RoomNode(Vector2Int bottomLeft, Vector2Int topRight, Node parent, int index) : base(parent)
    {
        this.BottomLeftCorner = bottomLeft;
        this.TopRightCorner = topRight;
        this.BottomRightCorner = new Vector2Int(topRight.x, bottomLeft.y);
        this.TopLeftCorner = new Vector2Int(bottomLeft.x, topRight.y);
        this.TreeLayerIndex = index;
    }

    public int Width { get => (int)(TopRightCorner.x - BottomLeftCorner.x); }
    public int Length { get => (int)(TopRightCorner.y - BottomLeftCorner.y); }
    
}
