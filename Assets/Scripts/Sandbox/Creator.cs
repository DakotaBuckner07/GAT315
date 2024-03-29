﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : Action
{
    public GameObject original;
    public FloatData speed;
    public FloatData damping;
    public FloatData size;
    public FloatData density;
    public FloatData restitution;
    public BodyEnumData bodyType;

    bool action { get; set; } = false;
    bool oneTime { get; set; } = false;
    public override eActionType actionType => eActionType.Creator;

    public override void StartAction()
    {
        action = true;
        oneTime = true;
    }

    public override void StopAction()
    {
        action = false;
    }

    void Update()
    {
        if (action && (oneTime || Input.GetKey(KeyCode.LeftControl)))
        {
            oneTime = false;
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject gameObject = Instantiate(original, position, Quaternion.identity);
            if(gameObject.TryGetComponent<Body>(out Body body))
            {
                body.damping = damping;
                body.shape.size = size;
                body.shape.density = density;
                body.restitution = restitution;
                body.type = (Body.eType)bodyType.value;
                Vector2 force = Random.insideUnitSphere.normalized * speed.value;
                body.AddForce(force, Body.eForceMode.Velocity);

                World.Instance.bodies.Add(body);
            }

        }
    }
}
