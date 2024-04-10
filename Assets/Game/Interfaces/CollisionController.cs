using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class CollisionController : MonoBehaviour
    {

        public static int groundLayerMask;

        private static Collider[] _colliders = new Collider [maxCollisions];
        private const int maxCollisions = 128;

        CharacterData characterData;
        TimerController timerController;

        [SerializeField] LayerMask groundMask;
        [HideInInspector] GameObject groundObject;
        public CapsuleCollider playerCollider;
        public Vector3 frontSide;
        public Vector3 leftSide;
        public Vector3 rightSide;
        public Vector3 backSide;
        public Vector3 groundNormal = Vector3.up;
        public event Action<int> OnCollisionEnterEvent;
        public event Action<int> OnCollisionStayEvent;
        public event Action<GameObject> OnCollisionExitEvent;

        public static void GetCapsulePoints(CapsuleCollider capc, Vector3 origin, out Vector3 p1, out Vector3 p2) {

            var distanceToPoints = capc.height / 2f - capc.radius;
            p1 = origin + capc.center + Vector3.up * distanceToPoints;
            p2 = origin + capc.center - Vector3.up * distanceToPoints;

        }

        private void ResolvePenetration(int numOverlaps) {

            for (int i = 0; i < numOverlaps; i++) {

                Vector3 direction;
                float distance;

                if (Physics.ComputePenetration (playerCollider, characterData.moveData.origin,
                    Quaternion.identity, _colliders [i], _colliders [i].transform.position,
                    _colliders [i].transform.rotation, out direction, out distance)) {

                    if (characterData.moveData.velocity == Vector3.zero) {
                        return;
                    }

                    if (distance < .01f) {
                        return;
                    }

                    // Debug.Log(characterData.moveData.velocity);
                    direction.Normalize();

                    characterData.moveData.velocity += Vector3.Dot(characterData.moveData.velocity, -direction) * direction;

                    float isSide = Mathf.Abs(Vector3.Dot(direction, Vector3.up));

                    // Handle collision
                    // Vector3 penetrationVector = direction * (distance + .5f * isSide);
                    Vector3 penetrationVector = direction * (distance);
                    characterData.moveData.origin += penetrationVector;

                    // characterData.moveData.velocity += planeForward;


                }
            }

        }

        public void PrivateResolveCollisions () {
        
            // manual collision resolving
            int numOverlaps = 0;
            if (playerCollider is CapsuleCollider) {

                var capc = playerCollider as CapsuleCollider;

                Vector3 point1, point2;
                GetCapsulePoints (capc, characterData.moveData.origin, out point1, out point2);

                numOverlaps = Physics.OverlapCapsuleNonAlloc (point1, point2, capc.radius,
                    _colliders, groundLayerMask, QueryTriggerInteraction.Ignore);

            }
            // else if (characterData.playerCollider is BoxCollider) {

            //     numOverlaps = Physics.OverlapBoxNonAlloc (characterData.moveData.origin, characterData.playerCollider.bounds.extents, _colliders,
            //         Quaternion.identity, layerMask, QueryTriggerInteraction.Ignore);

            // } else if (characterData.playerCollider is SphereCollider) {

            //     var caps = characterData.playerCollider as SphereCollider;

            //     numOverlaps = Physics.OverlapSphereNonAlloc (characterData.moveData.origin, caps.radius, _colliders,
            //         layerMask, QueryTriggerInteraction.Ignore);

            // }

            ResolvePenetration(numOverlaps);
        }


        public void Awake() {

            groundLayerMask = LayerMask.GetMask (new string[] { "Ground", "Player clip" }); //(1 << 0);

            OnCollisionStayEvent += ResolvePenetration;


            characterData = GetComponent<CharacterData>();

            playerCollider = transform.GetComponent<CapsuleCollider>();

            timerController = GetComponent<TimerController>();

            leftSide = Vector3.zero;
            rightSide = Vector3.zero;
            backSide = Vector3.zero;
            frontSide = Vector3.zero;

        }

        public void ResolveCollisions() {
    
            PrivateResolveCollisions();
            characterData.moveData.origin += characterData.moveData.velocity * Time.deltaTime; // p = v * dt
    
        }

        public void CollisionCheck() {
    
            CheckGrounded();
            
            if (timerController.jumpTimer > 0f || timerController.wallTouchTimer > 0f) return;
    
            RaycastHit hit;
            if (Physics.Raycast(characterData.moveData.origin, transform.forward, out hit, 1.1f, groundMask)) {
                frontSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, transform.forward + transform.right, out hit, 1.1f, groundMask)) {
                frontSide = hit.normal;
                if (frontSide == Vector3.zero) rightSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, transform.right, out hit, 1.1f, groundMask)) {
                rightSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, transform.right - transform.forward, out hit, 1.1f, groundMask)) {
                rightSide = hit.normal;
                if (rightSide == Vector3.zero) backSide = hit.normal;
            }
            
            if (Physics.Raycast(characterData.moveData.origin, -transform.forward, out hit, 1.1f, groundMask)) {
                backSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -transform.forward - transform.right, out hit, 1.1f, groundMask)) {
                backSide = hit.normal;
                if (backSide == Vector3.zero) leftSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -transform.right, out hit, 1.1f, groundMask)) {
                leftSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -transform.right + transform.forward, out hit, 1.1f, groundMask)) {
                leftSide = hit.normal;
                if (leftSide == Vector3.zero) frontSide = hit.normal;
            }
    
            characterData.playerData.detectWall = leftSide != Vector3.zero || rightSide != Vector3.zero || backSide != Vector3.zero || frontSide != Vector3.zero;
            // characterData.playerData.detectWall = characterData.playerData.detectWall && timerController.wallTouchTimer <= 0f;
    
            if (characterData.playerData.detectWall) {

                if (characterData.playerData.wallNormal == Vector3.zero) {
                    characterData.playerData.bonusTime = true;
                }

                characterData.playerData.wallNormal = (leftSide + rightSide + backSide + frontSide).normalized;
            } else {
                characterData.playerData.wallNormal = Vector3.zero;
            }

            leftSide = Vector3.zero;
            rightSide = Vector3.zero;
            backSide = Vector3.zero;
            frontSide = Vector3.zero;
            
    
            // if (Physics.) 
    
        }

        public void CheckGrounded() {
    
            RaycastHit hit;
            if (Physics.Raycast (
                origin: characterData.moveData.origin,
                direction: -groundNormal,
                hitInfo: out hit,
                maxDistance: 1.4f,
                layerMask: LayerMask.GetMask (new string[] { "Focus", "Ground" }),
                queryTriggerInteraction: QueryTriggerInteraction.Ignore)) {
                
            }
    
            if (hit.collider == null || timerController.jumpTimer > 0f) {
    
                SetGround(null);
                groundNormal = Vector3.Lerp(groundNormal, Vector3.up, Time.deltaTime / 2f);
                characterData.playerData.grounded = false;
    
            } else {
    
                // lastContact = hit.point;
                groundNormal = hit.normal.normalized;
                SetGround(hit.collider.gameObject);
    
                // if (Vector3.Distance(characterData.moveData.origin - groundNormal, lastContact) < .49f) {
                //     characterData.moveData.origin += groundNormal * Mathf.Min(Time.deltaTime, .01f); // soft collision resolution?
                // }
    
                if (!characterData.playerData.grounded) {
    
                    if (Vector3.Dot(characterData.moveData.velocity, groundNormal) <= -7.5f) {
                        // smokeLand.SetVector3("velocity", Vector3.ProjectOnPlane(characterData.moveData.velocity / 2f, groundNormal));
                        // smokeLand.SetVector3("position", characterData.moveData.origin - groundNormal / 2f);
                        // smokeLand.SetVector3("eulerAngles", Quaternion.LookRotation(groundNormal, Vector3.ProjectOnPlane(-velocityForward, groundNormal)).eulerAngles);
                        // smokeLand.Play();
                    }
                    
                }
        
                characterData.playerData.grounded = true;
    
            }
        }

        public void SetGround (GameObject obj) {
    
            if (obj != null) {
    
                groundObject = obj;
    
            } else
                groundObject = null;
    
        }




    }

}