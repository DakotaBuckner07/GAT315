using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

public struct Circle
{
    public Vector2 center { get; set; }
    public float radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public bool Contains(Circle circle)
    {
        Vector2 direction = circle.center - this.center;
        float sqrDistance = direction.sqrMagnitude;
        float sqrRadius = (radius + circle.radius) * ( circle.radius + radius);

        return (sqrDistance <= sqrRadius);
    }
}