using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public BoolData collision;
    public BoolData wrap;
    public FloatData gravity;
    public FloatData gravitation;
    public FloatData fixedFPS;
    public StringData fpsText;

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();

    static World instance;
    static public World Instance { get { return instance; } }

    float timeAccumulator = 0;
    float fixedDeltaTime { get { return 1.0f / fixedFPS.value; } }
    float fps = 0;
    float fpsAverage = 0;
    float smoothing = 0.975f;

    private Vector2 size;

    private void Awake()
    {
        instance = this;
        size = Camera.main.ViewportToWorldPoint(Vector2.one);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        fps = (1.0f / dt);
        fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        fpsText.value = "FPS: " + fps.ToString("F1");

        if (simulate == false) return;

        timeAccumulator += dt;
        //bodies.ForEach(body => body.shape.color = Color.red);//new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
        GravitationalForce.ApplyForce(bodies, gravitation);

        while (timeAccumulator >= fixedDeltaTime) 
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime)); 
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime)); 

            bodies.ForEach(body => body.shape.color = Color.white);
            if(collision)
            {
                Collision.CreateContacts(bodies, out List<Contact> contacts);
                contacts.ForEach(contact => { contact.bodyA.shape.color = Color.green; contact.bodyB.shape.color = Color.green; });
                ContactSolver.Resolve(contacts);
            }

            timeAccumulator = timeAccumulator - fixedDeltaTime; 
        }
        if (wrap) 
        {
            bodies.ForEach(body => body.position = Utilities.Wrap(body.position, -size, size)); 
        }
        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

    }
}
