using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Coordinate;

public class InverseKinematicSegment : KinematicSegment
{
    private void Update()
    {
        transform.localScale = Vector2.one * size;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width)
    {
        this.parent = parent;
        this.size = width;

        this.angle = angle;
        this.length = length;

        start = position;
    }

    public void Follow(Vector2 target)
    {
        Vector2 direction = target - start;
        Polar polar = CartesianToPolar(direction);
        this.angle = polar.angle;

        start = target - (direction.normalized * length);
    }
}
