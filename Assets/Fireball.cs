using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public MoveData moveData;
    private Vector3 prevPosition;
    private SphereCollider collider { get { return gameObject.GetComponent<SphereCollider>(); } }
    private void Awake() 
    {
        moveData = GetComponent<MoveData>();

        moveData.origin = transform.position;
        prevPosition = transform.position;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 positionalMovement = transform.position - prevPosition; // TODO: Update
        transform.position = prevPosition;
        moveData.origin += positionalMovement;

        ResolveCollisions();

        transform.position = moveData.origin;
        prevPosition = transform.position;
        

    }

    private void ResolveCollisions() {
        var layerMask = LayerMask.GetMask (new string[] { "Ground", "Player" });

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

            DivePhysics.ResolveCollisions(collider, ref moveData.origin, ref moveData.velocity, layerMask);


    }
}
