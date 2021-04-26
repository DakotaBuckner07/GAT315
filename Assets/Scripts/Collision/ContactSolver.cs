using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContactSolver
{
    public static void Resolve(List<Contact> contacts)
    {
        foreach(Contact c in contacts)
        {
            //Seperation
            float totalInverseMass = c.bodyA.inverseMass + c.bodyB.inverseMass;
            Vector2 seperation = c.normal * c.depth / totalInverseMass;
            c.bodyA.position += (seperation * c.bodyA.inverseMass);
            c.bodyB.position -= (seperation * c.bodyB.inverseMass);

            // Collision Impulse
            Vector2 relativeVelocity = c.bodyA.velocity - c.bodyB.velocity;
            float normalVelocity = Vector2.Dot(relativeVelocity, c.normal);

            if (normalVelocity > 0) continue;

            float restitution = (c.bodyA.restitution + c.bodyB.restitution) * 0.5f;
            float impulseMagnitude = -(1.0f + restitution) * normalVelocity / totalInverseMass;

            Vector2 impulse = c.normal * impulseMagnitude;
            c.bodyA.AddForce(c.bodyA.velocity + (impulse * c.bodyA.inverseMass), Body.eForceMode.Velocity);
            c.bodyB.AddForce(c.bodyB.velocity - (impulse * c.bodyB.inverseMass), Body.eForceMode.Velocity);
        }
    }
}
