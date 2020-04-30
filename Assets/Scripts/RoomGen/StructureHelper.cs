using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeaves(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();

        if(parentNode.ChildrenNodes.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach(var child in parentNode.ChildrenNodes)
        {
            nodesToCheck.Enqueue(child);
        }
        while(nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodes.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach(var child in currentNode.ChildrenNodes)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeft, Vector2Int boundaryRight, float pointModifier, int offset)
    {
        int minX = boundaryLeft.x + offset;
        int maxX = boundaryRight.x - offset;
        int minY = boundaryLeft.y + offset;
        int maxY = boundaryRight.y - offset;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));
    }

    public static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeft, Vector2Int boundaryRight, float pointModifier, int offset)
    {
        int minX = boundaryLeft.x + offset;
        int maxX = boundaryRight.x - offset;
        int minY = boundaryLeft.y + offset;
        int maxY = boundaryRight.y - offset;
        return new Vector2Int(
            Random.Range((int)(minX + (maxX-minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY));
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 temp = sum / 2;
        return new Vector2Int((int)temp.x, (int)temp.y);
    }
}

public enum RelativePosition
{
    Up,
    Down,
    Right,
    Left
}
