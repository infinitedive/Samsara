using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class CollisionHandler : MonoBehaviour
    {

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


        public void Awake() {

            characterData = GetComponent<CharacterData>();

            playerCollider = transform.GetComponent<CapsuleCollider>();

            playerCollider = transform.GetComponent<CapsuleCollider>();

            timerController = GetComponent<TimerController>();

            leftSide = Vector3.zero;
            rightSide = Vector3.zero;
            backSide = Vector3.zero;
            frontSide = Vector3.zero;

        }

        public void ResolveCollisions() {
    
            DivePhysics.ResolveCollisions(playerCollider, ref characterData.moveData.origin, ref characterData.moveData.velocity, LayerMask.GetMask (new string[] { "Ground" }));
            characterData.moveData.origin += characterData.moveData.velocity * Time.deltaTime; // p = v * dt
    
        }

        public void CollisionCheck() {
    
            CheckGrounded();
            
            if (timerController.jumpTimer > 0f) return;
    
            RaycastHit hit;
            if (Physics.Raycast(characterData.moveData.origin, characterData.bodyForward, out hit, 1.1f, groundMask)) {
                frontSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, characterData.bodyForward + characterData.bodyRight, out hit, 1.1f, groundMask)) {
                frontSide = hit.normal;
                if (frontSide == Vector3.zero) rightSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, characterData.bodyRight, out hit, 1.1f, groundMask)) {
                rightSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, characterData.bodyRight - characterData.bodyForward, out hit, 1.1f, groundMask)) {
                rightSide = hit.normal;
                if (rightSide == Vector3.zero) backSide = hit.normal;
            }
            
            if (Physics.Raycast(characterData.moveData.origin, -characterData.bodyForward, out hit, 1.1f, groundMask)) {
                backSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -characterData.bodyForward - characterData.bodyRight, out hit, 1.1f, groundMask)) {
                backSide = hit.normal;
                if (backSide == Vector3.zero) leftSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -characterData.bodyRight, out hit, 1.1f, groundMask)) {
                leftSide = hit.normal;
            }
    
            if (Physics.Raycast(characterData.moveData.origin, -characterData.bodyRight + characterData.bodyForward, out hit, 1.1f, groundMask)) {
                leftSide = hit.normal;
                if (leftSide == Vector3.zero) frontSide = hit.normal;
            }
    
            characterData.playerData.detectWall = leftSide != Vector3.zero || rightSide != Vector3.zero || backSide != Vector3.zero || frontSide != Vector3.zero;
            characterData.playerData.detectWall = characterData.playerData.detectWall && timerController.wallTouchTimer <= 0f;
    
            if (characterData.playerData.detectWall) {
                characterData.playerData.wallNormal = (leftSide + rightSide + backSide + frontSide).normalized;
            } else {
                characterData.playerData.wallNormal = Vector3.zero;
            }
            
    
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