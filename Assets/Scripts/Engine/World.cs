using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public BoolData simulate;
    public BoolData collision;
    public BoolData wrap;
    public BoolData collisionDebug;
    public FloatData gravity;
    public FloatData gravitation;
    public FloatData fixedFPS;
    public StringData fpsText;
    public StringData collisionText;
    public VectorField vectorField;
    public BroadPhaseTypeData BroadPhaseType;

    public Vector2 Gravity { get { return new Vector2(0, gravity.value); } }
    public List<Body> bodies { get; set; } = new List<Body>();
    public List<Force> forces { get; set; } = new List<Force>();
    public List<Spring> springs { get; set; } = new List<Spring>();

    public Vector2 worldSize { get => size * 2; }
    public AABB AABB { get => aabb; }

    static World instance;
    static public World Instance { get { return instance; } }

    BroadPhase broadPhase;
    BroadPhase[] broadPhases = { new NullBroadPhase(), new QuadTree(), new BVH() };

    AABB aabb;
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
        aabb = new AABB(Vector2.zero, size * 2);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        fps = (1.0f / dt);
        fpsAverage = (fpsAverage * smoothing) + (fps * (1.0f - smoothing));
        fpsText.value = "FPS: " + fps.ToString("F1");

        broadPhase = broadPhases[BroadPhaseType.index];

        springs.ForEach(spring => spring.Draw());

        if (simulate == false) return;

        timeAccumulator += dt;

        GravitationalForce.ApplyForce(bodies, gravitation);
        forces.ForEach(force => bodies.ForEach(body => force.ApplyForce(body)));
        springs.ForEach(spring => spring.ApplyForce());
        bodies.ForEach(body => vectorField.ApplyForce(body));

        while (timeAccumulator >= fixedDeltaTime) 
        {
            bodies.ForEach(body => body.Step(fixedDeltaTime)); 
            bodies.ForEach(body => Integrator.SemiImplicitEuler(body, fixedDeltaTime)); 

            if(collision)
            {
                bodies.ForEach(body => body.shape.color = Color.white);
                broadPhase.Build(aabb, bodies);

                Collision.CreateBroadPhaseContacts(broadPhase, bodies, out List<Contact> contacts);
                Collision.CreateNarrowPhaseContacts(ref contacts);
                contacts.ForEach(contact => Collision.UpdateContactInfo(ref contact));

                //Collision.CreateContacts(bodies, out List<Contact> contacts);
                //contacts.ForEach(contact => { contact.bodyA.shape.color = Color.green; contact.bodyB.shape.color = Color.green; });


                ContactSolver.Resolve(contacts);
                contacts.ForEach(contact => { contact.bodyA.shape.color = Color.red; contact.bodyB.shape.color = Color.red; });
            }

            timeAccumulator = timeAccumulator - fixedDeltaTime; 
        }
        collisionText.value = "Broad Phase: " + BroadPhase.potentialCollisionCount.ToString();
        if(collisionDebug) broadPhase.Draw();

        if (wrap) 
        {
            bodies.ForEach(body => body.position = Utilities.Wrap(body.position, -size, size)); 
        }
        bodies.ForEach(body => body.force = Vector2.zero);
        bodies.ForEach(body => body.acceleration = Vector2.zero);

    }
}
