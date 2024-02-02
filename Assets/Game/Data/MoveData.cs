using UnityEngine;

[System.Serializable]
public class MoveData : MonoBehaviour {

    ///// Fields /////

    // Core Data
    
    [SerializeField] public Vector3 origin;
    public Vector3 velocity;
    public Vector3 flatVelocity;
    public GameObject groundObject;
    public Vector3 groundNormal;
    // public Collider collider;
    Vector3 prevPosition;

}
