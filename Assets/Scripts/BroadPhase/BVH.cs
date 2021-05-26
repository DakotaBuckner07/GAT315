using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode; 
    
    public override void Build(AABB aabb, List<Body> bodies)
    {
        potentialCollisionCount = 0; 
        List<Body> sorted = new List<Body>(bodies);
        // sort bodies along x-axis (position.x)
        //collection.OrderBy(v => v.Value)
        //BroadPhase broadPhase = new BVH();
        sorted.Sort((body, body2) => body.position.x.CompareTo(body2.position.x));
        // set sorted bodies to root bvh node
        rootNode = new BVHNode(sorted);
    }
    
    public override void Query(AABB aabb, List<Body> bodies)
    {
        rootNode.Query(aabb, bodies);
        // update the number of potential collisions
        potentialCollisionCount += bodies.Count;
    }
    
    public override void Query(Body body, List<Body> bodies)
    {
        Query(body.shape.aabb, bodies);
    }
    public override void Draw()
    {
        //<check if the root node is not null below, use ? operator>
        (rootNode)?.Draw();//<call Draw() on root node>
    }
}