﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorNode : Node
{
    private Node structure1;
    private Node structure2;
    private int corridorWidth;
    private int modifierDistanceFromWall = 1;
    public RelativePosition orientation;
    public Room[] connectedRooms = new Room[2];

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        this.structure1 = node1;
        this.structure2 = node2;
        this.corridorWidth = corridorWidth;
        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        var relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        orientation = relativePositionOfStructure2;
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.structure1, this.structure2);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.structure2, this.structure1);
                break;
            default:
                break;
        }
    }

    private void ProcessRoomInRelationRightOrLeft(Node structure1, Node structure2)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeaves(structure1);
        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeaves(structure2);

        var sortedLeftStructure = leftStructureChildren.OrderByDescending(child => child.TopRightCorner.x).ToList();
        if (sortedLeftStructure.Count == 1)
        {
            leftStructure = sortedLeftStructure[0];
        }
        else
        {
            int maxX = sortedLeftStructure[0].TopRightCorner.x;
            sortedLeftStructure = sortedLeftStructure.Where(children => Math.Abs(maxX - children.TopRightCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructure.Count);
            leftStructure = sortedLeftStructure[index];
        }

        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child => GetValidYForNeighourLeftRight(
                leftStructure.TopRightCorner,
                leftStructure.BottomRightCorner,
                child.TopLeftCorner,
                child.BottomLeftCorner
                ) != -1
            ).OrderBy(child => child.BottomRightCorner.x).ToList();

        if (possibleNeighboursInRightStructureList.Count <= 0)
        {
            rightStructure = structure2;
        }
        else
        {
            rightStructure = possibleNeighboursInRightStructureList[0];
        }
        int y = GetValidYForNeighourLeftRight(leftStructure.TopLeftCorner, leftStructure.BottomRightCorner,
            rightStructure.TopLeftCorner,
            rightStructure.BottomLeftCorner);
        while (y == -1 && sortedLeftStructure.Count > 1)
        {
            sortedLeftStructure = sortedLeftStructure.Where(
                child => child.TopLeftCorner.y != leftStructure.TopLeftCorner.y).ToList();
            leftStructure = sortedLeftStructure[0];
            y = GetValidYForNeighourLeftRight(leftStructure.TopLeftCorner, leftStructure.BottomRightCorner,
            rightStructure.TopLeftCorner,
            rightStructure.BottomLeftCorner);
        }
        BottomLeftCorner = new Vector2Int(leftStructure.BottomRightCorner.x, y);
        TopRightCorner = new Vector2Int(rightStructure.TopLeftCorner.x, y + this.corridorWidth);
        connectedRooms[0] = ((RoomNode)leftStructure).roomObjReference;
        connectedRooms[1] = ((RoomNode)rightStructure).roomObjReference;
    }

    private int GetValidYForNeighourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)
                ).y;
        }
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)
                ).y;
        }
        if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)
                ).y;
        }
        if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)
                ).y;
        }
        return -1;
    }

    private void ProcessRoomInRelationUpOrDown(Node structure1, Node structure2)
    {
        Node bottomStructure = null;
        List<Node> structureBottmChildren = StructureHelper.TraverseGraphToExtractLowestLeaves(structure1);
        Node topStructure = null;
        List<Node> structureAboveChildren = StructureHelper.TraverseGraphToExtractLowestLeaves(structure2);

        var sortedBottomStructure = structureBottmChildren.OrderByDescending(child => child.TopRightCorner.y).ToList();

        if (sortedBottomStructure.Count == 1)
        {
            bottomStructure = structureBottmChildren[0];
        }
        else
        {
            int maxY = sortedBottomStructure[0].TopLeftCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Mathf.Abs(maxY - child.TopLeftCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        var possibleNeighboursInTopStructure = structureAboveChildren.Where(
            child => GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftCorner,
                bottomStructure.TopRightCorner,
                child.BottomLeftCorner,
                child.BottomRightCorner)
            != -1).OrderBy(child => child.BottomRightCorner.y).ToList();
        if (possibleNeighboursInTopStructure.Count == 0)
        {
            topStructure = structure2;
        }
        else
        {
            topStructure = possibleNeighboursInTopStructure[0];
        }
        int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftCorner,
                bottomStructure.TopRightCorner,
                topStructure.BottomLeftCorner,
                topStructure.BottomRightCorner);
        while (x == -1 && sortedBottomStructure.Count > 1)
        {
            sortedBottomStructure = sortedBottomStructure.Where(child => child.TopLeftCorner.x != topStructure.TopLeftCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];
            x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftCorner,
                bottomStructure.TopRightCorner,
                topStructure.BottomLeftCorner,
                topStructure.BottomRightCorner);
        }
        BottomLeftCorner = new Vector2Int(x, bottomStructure.TopLeftCorner.y);
        TopRightCorner = new Vector2Int(x + this.corridorWidth, topStructure.BottomLeftCorner.y);
        connectedRooms[0] = ((RoomNode)bottomStructure).roomObjReference;
        connectedRooms[1] = ((RoomNode)topStructure).roomObjReference;
    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft,
        Vector2Int bottomNodeRight, Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        if (topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if (topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if (bottomNodeLeft.x >= (topNodeLeft.x) && bottomNodeLeft.x <= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)

                ).x;
        }
        if (bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)

                ).x;
        }
        return -1;
    }

    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        Vector2 middlePointStructure1Temp = ((Vector2)structure1.TopRightCorner + structure1.BottomLeftCorner) / 2;
        Vector2 middlePointStructure2Temp = ((Vector2)structure2.TopRightCorner + structure2.BottomLeftCorner) / 2;
        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return RelativePosition.Right;
        }
        else if (angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
    {
        return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y,
            middlePointStructure2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }
}