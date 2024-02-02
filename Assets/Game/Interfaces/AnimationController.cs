using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class AnimationController : MonoBehaviour
    {

        private CharacterData characterData;
        public Animator animator;

        private void Awake() {

            characterData = GetComponent<CharacterData>();

        }



        public void DoBlendAnimations() {

            characterData.zVel = Vector3.Dot(Vector3.ProjectOnPlane(characterData.moveData.velocity, characterData.groundNormal), characterData.bodyForward);

            characterData.xVel = Vector3.Dot(Vector3.ProjectOnPlane(characterData.moveData.velocity, characterData.groundNormal), characterData.bodyRight);

            

            var turningDelta = characterData.xMouseMovement * 30f;

            if (!characterData.playerData.grounded) {
                // smoke.SetVector3("position", characterData.moveData.origin - groundNormal);
                // smoke.SetFloat("force", characterData.moveData.velocity.magnitude / 10f);
                // smoke.SetFloat("spawnRate", 32f + characterData.moveData.velocity.magnitude);


                if (characterData.moveData.velocity.magnitude > characterData.moveConfig.walkSpeed) {

                    if (characterData.playerData.grounded) {
                        // smoke.SetVector3("direction", -characterData.playerData.xzWishMove);
                    } else {
                        // smoke.SetVector3("direction", -characterData.playerData.wishMove);
                    }

                } else {
                    // smoke.SetVector3("direction", Vector3.zero);
                    // smoke.SetFloat("force", 0f);
                }

                if (characterData.playerData.wishTumbleDown && characterData.playerData.grounded && characterData.moveData.velocity.magnitude > characterData.moveConfig.walkSpeed) {
                    // smoke.SetVector3("direction", moveData.velocity.normalized);
                    // smoke.SetFloat("force", moveData.velocity.magnitude / 10f);

                    characterData.xVel = Mathf.Lerp(characterData.xVel, -characterData.xVel, .3f);
                    characterData.zVel = Mathf.Lerp(characterData.zVel, -characterData.zVel, .3f);
                }

            } else {

            }

            animator.SetFloat("xVel", characterData.xVel);
            animator.SetFloat("zVel", characterData.zVel);
            animator.SetFloat("turningDelta", turningDelta);

            // if (characterData.playerData.)

            // animator.SetBool("ChargePress", characterData.playerData.wishCrouchDown);
            // animator.SetBool("onGround", characterData.playerData.grounded);
            
        }
    }

}