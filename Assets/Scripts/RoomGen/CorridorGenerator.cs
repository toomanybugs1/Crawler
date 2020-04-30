using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorGenerator
{
    public List<Node> CreateCorridor(List<RoomNode> allSpaceNodes, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(allSpaceNodes.OrderByDescending(node => node.TreeLayerIndex).ToList());
        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            if (node.ChildrenNodes.Count == 0)
                continue;

            CorridorNode corridor = new CorridorNode(node.ChildrenNodes[0], node.ChildrenNodes[1], corridorWidth);
            corridorList.Add(corridor);
        }
        return corridorList;
    }
}