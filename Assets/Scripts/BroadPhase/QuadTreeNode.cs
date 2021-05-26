using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadTreeNode
{
    AABB aabb;
    int capacity;
    List<Body> bodies;
    bool subdivided = false;

    QuadTreeNode northeast;
    QuadTreeNode northwest;
    QuadTreeNode southeast;
    QuadTreeNode southwest;

    public QuadTreeNode(AABB aabb, int capacity)
    {
        this.aabb = aabb;
        this.capacity = capacity;

        bodies = new List<Body>();
    }

    public void Insert(Body body)
    {
        if (!aabb.Contains(body.shape.aabb)) return;

        if (bodies.Count < capacity) bodies.Add(body);
        else
        {
            if (!subdivided)
            {
                Subdivide();
            }
            northeast.Insert(body);
            northwest.Insert(body);
            southeast.Insert(body);
            southwest.Insert(body);
        }
    }

    public void Query(AABB aabb, List<Body> bodies)
    {
        if (!this.aabb.Contains(aabb)) return;

        bodies.AddRange(this.bodies.Where(body => body.shape.aabb.Contains(aabb)));

        foreach(Body b in this.bodies)
        {
            if (b.shape.aabb.Contains(aabb)) bodies.Add(b);
        }

        if(subdivided)
        {
            northeast.Query(aabb, bodies);
            southeast.Query(aabb, bodies);
            southwest.Query(aabb, bodies);
            northwest.Query(aabb, bodies);
        }
    }

    void Subdivide()
    {
        float xc = aabb.extents.x * 0.5f;
        float yc = aabb.extents.y * 0.5f;

        northeast = new QuadTreeNode(new AABB(new Vector2(aabb.center.x - xc, aabb.center.y + yc), aabb.extents), capacity);
        northwest = new QuadTreeNode(new AABB(new Vector2(aabb.center.x + xc, aabb.center.y + yc), aabb.extents), capacity);
        southeast = new QuadTreeNode(new AABB(new Vector2(aabb.center.x - xc, aabb.center.y - yc), aabb.extents), capacity);
        southwest = new QuadTreeNode(new AABB(new Vector2(aabb.center.x + xc, aabb.center.y - yc), aabb.extents), capacity);

        subdivided = true;
    }

    public void Draw()
    {
        aabb.Draw(Color.white);

        northeast?.Draw();
        southeast?.Draw();
        southwest?.Draw();
        northwest?.Draw();
    }
}
