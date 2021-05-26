﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum eType
    {
        Static,
        Kinematic,
        Dynamic
    }

    public enum eForceMode
    {
        Force,
        Acceleration,
        Velocity
    }

    public Shape shape;

    public eType type { get; set; }
    public Vector2 force { get; set; } = Vector2.zero;
    public Vector2 acceleration { get; set; } = Vector2.zero;
    public Vector2 velocity { get; set; } = Vector2.zero;
    public Vector2 position { get { return transform.position; } set { transform.position = value; } }

    public float mass { get => shape.mass; }
    public float inverseMass { get => (mass == 0 || type == eType.Static) ? 0 : 1 / mass; }
    public float damping { get; set; } = 0;
    public float restitution { get; set; } = 0.5f;


    public void AddForce(Vector2 force, eForceMode forceMode = eForceMode.Force)
    {
        if (type == eType.Static) return;
        switch (forceMode)
        {
            case eForceMode.Force:
                this.force += force;
                break;
            case eForceMode.Acceleration:
                acceleration = force;
                break;
            case eForceMode.Velocity:
                velocity = force;
                break;
            default:
                break;
        }

    }

    public void Step(float dt)
    {
        if (type != eType.Dynamic) return;
        acceleration += World.Instance.Gravity + (force * inverseMass);
    }
}
