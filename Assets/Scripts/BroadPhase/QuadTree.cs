using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : BroadPhase
{
    public int Capacity { get; set; } = 4;
    QuadTreeNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        potentialCollisionCount = 0;
        rootNode = new QuadTreeNode(aabb, Capacity);
        bodies.ForEach(body => rootNode.Insert(body));
    }

    public override void Query(AABB aabb, List<Body> bodies)
    {
        rootNode.Query(aabb, bodies);
        potentialCollisionCount += bodies.Count;
    }

    public override void Query(Body body, List<Body> bodies)
    {
        Query(body.shape.aabb, bodies);
    }

    public override void Draw()
    {
        rootNode?.Draw();
    }
}
