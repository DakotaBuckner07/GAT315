﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalForce : MonoBehaviour
{
    public static void ApplyForce(List<Body> bodies, float G)
    {
        for (int i = 0; i < bodies.Count - 1; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                Body bodyA = bodies[i]; 
                Body bodyB = bodies[j];

                Vector2 direction = bodyB.transform.position - bodyA.transform.position;
                float distanceSqr = Mathf.Max(direction.sqrMagnitude, 1);

                float force = G * (bodyA.mass * bodyB.mass) / distanceSqr;

                bodyA.AddForce((direction.normalized * force), Body.eForceMode.Force);
                bodyB.AddForce(-(direction.normalized * force), Body.eForceMode.Force);
            }
        }
    }
}
