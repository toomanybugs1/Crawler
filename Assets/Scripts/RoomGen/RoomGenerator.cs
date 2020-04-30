using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxPasses;
    private int roomMinLength;
    private int roomMinWidth;

    public RoomGenerator(int maxPasses, int roomMinLength, int roomMinWidth)
    {
        this.maxPasses = maxPasses;
        this.roomMinLength = roomMinLength;
        this.roomMinWidth = roomMinWidth;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float bottomCornerModifier, float topCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach(var space in roomSpaces)
        {
            Vector2Int newBottomLeft = StructureHelper.GenerateBottomLeftCornerBetween(
                    space.BottomLeftCorner, space.TopRightCorner, bottomCornerModifier, roomOffset
                    );
            Vector2Int newTopRight = StructureHelper.GenerateTopRightCornerBetween(
                    space.BottomLeftCorner, space.TopRightCorner, topCornerModifier, roomOffset
                    );

            space.BottomLeftCorner = newBottomLeft;
            space.TopRightCorner = newTopRight;
            space.BottomRightCorner = new Vector2Int(newTopRight.x, newBottomLeft.y);
            space.TopLeftCorner = new Vector2Int(newBottomLeft.x, newTopRight.y);

            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }
}
