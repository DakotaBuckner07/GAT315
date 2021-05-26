using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    public AABB aabb; 
    public List<Body> bodies = null; 
    public BVHNode left = null; 
    public BVHNode right = null; 
    
    public BVHNode(List<Body> bodies) 
    {
        this.bodies = bodies; 
        ComputeBoundary(); 
        Split(); 
    }

    public void Query(AABB aabb, List<Body> bodies) 
    {
        if (!this.aabb.Contains(aabb)) return; // <if the aabb is not contained in the bvh node aabb>) return; 
        if (this.bodies.Count > 0) 
        {
            bodies.AddRange(this.bodies); 
        }
        //< check if the left / right node below is not null, use ? operator>
        (left)?.Query(aabb, bodies);//< query the left node passing in aabb and bodies >
        (right)?.Query(aabb, bodies);//< query the right node passing in aabb and bodies >
    }
    public void ComputeBoundary()
    {
        if (bodies.Count > 0)// < bodies list has more than 0 elements >)
        {
            aabb.center = bodies[0].position; 
            aabb.size = Vector3.zero; 
            foreach (Body body in bodies)
            {
                aabb.Expand(body.shape.aabb);//< expand this aabb by the body shape aabb>
            }
        }
    }

    public void Split() 
    {
        int length = bodies.Count;//< number of elements in bodies list>;
        int half = length / 2;//< half the length>; 
        if (half >= 1) 
        {
            left = new BVHNode(bodies.GetRange(0, half)); 
            right = new BVHNode(bodies.GetRange(half, half + (length % 2))); bodies.Clear(); 
        } 
    }

    public void Draw() 
    {
        aabb.Draw(Color.white);
        //<check if the left / right node is not null below, use ? operator>
        (left)?.Draw();//<draw left node>
        (right)?.Draw();//< draw right node>
    }
}
