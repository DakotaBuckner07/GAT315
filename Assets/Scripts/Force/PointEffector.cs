using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEffector : Force
{
    public CircleShape shape; 
    public Vector2 position { get => transform.position; set => transform.position = value; }
    public float forceMagnitude { get; set; }
    public ForceEnumData.eType forceMode { get; set; }
    public override void ApplyForce(Body body)
    {
        Circle circleA = new Circle(position, shape.radius); 
        Circle circleB = new Circle(body.position, body.shape.size * 0.5f); 
        if (circleA.Contains(circleB))
        {
            Vector2 direction = position - body.position;
            float distance = direction.magnitude;
            float t = distance / shape.radius;
            Vector2 force = direction.normalized; 
            switch (forceMode) 
            {
                case ForceEnumData.eType.Constant:
                    force = force * forceMagnitude;
                    break; 
                case ForceEnumData.eType.InverseLinear: 
                    force = force * ((1 - t) * forceMagnitude); 
                    break; 
                case ForceEnumData.eType.InverseSquared: 
                    force = force * (((1 - t) * (1 - t)) * forceMagnitude); 
                    break; 
            }
            body.AddForce(force);
        }
    }
}
