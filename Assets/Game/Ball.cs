using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour, IHittable
{

    public MoveData moveData;
    private Vector3 origin;
    private Vector3 velocity;
    private Vector3 prevPosition;
    private bool grounded;
    private Rigidbody rb;
    private Vector3 targetPos;
    public Collider[] targets;

    public event UnityAction OnHit;

    private SphereCollider collider { get { return gameObject.GetComponent<SphereCollider>(); } }

    public void GetHit(Vector3 hitVel) 
    {
        moveData.velocity = hitVel;
    }

    public void Stop() {
        moveData.velocity = Vector3.Lerp(moveData.velocity, Vector3.zero, Time.deltaTime * 2f);
        moveData.velocity.y = 0f;
    }

    private void Awake() 
    {
        moveData = GetComponent<MoveData>();

        moveData.origin = transform.position;
        prevPosition = transform.position;

    }

    void Start()
    {
        // moveData.targets = new Collider[5];
    }

    // Update is called once per frame
    void Update()
    {

        // ChasePlayer();

        Vector3 positionalMovement = transform.position - prevPosition; // TODO: Update
        transform.position = prevPosition;
        moveData.origin += positionalMovement;

        ResolveCollisions();

        // currentState.UpdateStates();

        transform.position = moveData.origin;
        prevPosition = transform.position;
        
    }

    // private void ChasePlayer() {
    //     int numTargets = Physics.OverlapSphereNonAlloc(moveData.origin, 50f, moveData.targets, LayerMask.GetMask (new string[] { "Player" }), QueryTriggerInteraction.Ignore);
                
    //     for (int i = 0; i < numTargets; i++) {

    //         targetPos = moveData.targets[i].transform.position;

    //     }

    //     if (numTargets == 0) targetPos = Vector3.zero;

    //     if (targetPos != Vector3.zero) {

    //         Vector3 toPlayer = (targetPos - moveData.origin);

    //         moveData.velocity = Vector3.Slerp(moveData.velocity, toPlayer, Time.deltaTime / 2f);
    //         moveData.velocity.y = 0f;

    //     } else {
    //         moveData.velocity = Vector3.zero;
    //     }


    // }

    private void ResolveCollisions() {
        var layerMask = LayerMask.GetMask (new string[] { "Ground", "Player" });

        if ((moveData.velocity.sqrMagnitude) == 0f) {


            // Do collisions while standing still
            DivePhysics.ResolveBallCollisions(collider, ref moveData.origin, ref moveData.velocity, layerMask, 2f);

        } else {

            float maxDistPerFrame = 0.2f;
            Vector3 velocityThisFrame = moveData.velocity * Time.deltaTime;
            moveData.origin += velocityThisFrame;
            float velocityDistLeft = velocityThisFrame.magnitude;
            float initialVel = velocityDistLeft;
            
            // while (velocityDistLeft > 0f) {

            //     float amountThisLoop = Mathf.Min (maxDistPerFrame, velocityDistLeft);
            //     velocityDistLeft -= amountThisLoop;

            //     // increment origin
            //     Vector3 velThisLoop = velocityThisFrame * (amountThisLoop / initialVel);
                
            //     origin += velThisLoop;

            //     // don't penetrate walls
            //     DivePhysics.ResolveBallCollisions(collider, ref origin, ref velocity, LayerMask.GetMask (new string[] { "Ground" }), 2f);

            // }

            DivePhysics.ResolveBallCollisions(collider, ref moveData.origin, ref moveData.velocity, layerMask, 2f);

        }

    }
    
}
