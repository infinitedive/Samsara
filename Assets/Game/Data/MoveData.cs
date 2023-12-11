using UnityEngine;

[System.Serializable]
public class MoveData : MonoBehaviour {

    ///// Fields /////

    // Core Data
    
    public Vector3 origin;
    public Vector3 velocity;
    public Vector3 flatVelocity;
    public GameObject groundObject;
    public Vector3 groundNormal;
    public Collider collider;
    Vector3 prevPosition;

    // private void CustomStart() {
    //     origin = transform.position;
    //     velocity = Vector3.zero;
    //     flatVelocity = Vector3.zero;

    //     prevPosition = transform.position;

    //     groundNormal = Vector3.up;
    //     collider = transform.GetComponent<Collider>();
    // }

    private void CustomUpdate() {

        CheckGrounded();

        Vector3 positionalMovement = transform.position - prevPosition; // TODO: 
        transform.position = prevPosition;
        origin += positionalMovement;

        ResolveCollisions();

        transform.position = origin;
        prevPosition = transform.position;

        // flatVelocity = Vector3.ProjectOnPlane(velocity, groundNormal);

    }

    private void ResolveCollisions() {

        // if ((velocity.magnitude) == 0f) {

        //     DivePhysics.ResolveCollisions(collider, ref origin, ref velocity, LayerMask.GetMask (new string[] { "Ground" }), ref squash);

        // } else {

        //     DivePhysics.ResolveCollisions(collider, ref origin, ref velocity, LayerMask.GetMask (new string[] { "Ground" }));
        //     origin += velocity * Time.deltaTime; // p = v * dt


        // }

    }

    private void CheckGrounded() {

        RaycastHit hit;
        if (Physics.Raycast (
            origin: origin,
            direction: -groundNormal,
            hitInfo: out hit,
            maxDistance: 1.4f,
            layerMask: LayerMask.GetMask (new string[] { "Focus", "Ground" }),
            queryTriggerInteraction: QueryTriggerInteraction.Ignore)) {
            
        }

        if (hit.collider == null) {

            SetGround(null);
            groundNormal = Vector3.Lerp(groundNormal, Vector3.up, Time.deltaTime / 2f);

        } else {

            groundNormal = hit.normal.normalized;
            SetGround(hit.collider.gameObject);

            // if (Vector3.Distance(origin - groundNormal, lastContact) < .49f) {
            //     origin += groundNormal * Mathf.Min(Time.deltaTime, .01f); // soft collision resolution?
            // }



        }
    }

    private void SetGround (GameObject obj) {

        if (obj != null) {

            groundObject = obj;

        } else
            groundObject = null;

    }
    

    public void Push(Vector3 force) {
        velocity += force;
    }

}
