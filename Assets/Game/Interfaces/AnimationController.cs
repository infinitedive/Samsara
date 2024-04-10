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
            animator = transform.GetChild(2).GetComponent<Animator>();

        }

        public void HandleAnimations() {

            /*
            InputAngle
            WalkStartAngle
            Horizontal
            Vertical
            IsStopRU
            IsStopLU
            IsRU
            InputMagnitude
            HorAimAngle
            VerAimAngle
            WalkStopAngle
            IsDead
            IsFalling
            IsJump
            Crouch_notUsed
            IsCrouch
            IsControl
            X
            Z
            RawInputAngle
            InteractAngle
            Running
            SprintFactor
            FloorAngle
            Vault
            Vault2m
            HitWall
            Vault05m
            Vault1m
            IsAvoidMode
            Slide
            FallingTime
            */

            // animator.SetFloat(parameterName, value);

            float inputAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(characterData.moveData.velocity.normalized, Vector3.up), characterData.playerData.wishMove, Vector3.up);
            animator.SetFloat("InputAngle", inputAngle); // -35 - 35
            animator.SetFloat("WalkStartAngle", Vector3.SignedAngle(Vector3.ProjectOnPlane(characterData.moveData.velocity.normalized, Vector3.up), characterData.playerData.wishMove, Vector3.up));
            // animator.SetFloat("InteractAngle", 0f);
            // animator.SetFloat("SprintFactor", 1f - (characterData.moveConfig.walkSpeed - characterData.moveData.velocity.magnitude)/characterData.moveConfig.walkSpeed);
            animator.SetFloat("FloorAngle", 0f);

            animator.SetFloat("InputMagnitude", characterData.moveData.velocity.magnitude / characterData.moveConfig.walkSpeed);

            // animator.SetBool("Running", characterData.playerData.running);
            bool isStopRU = false;
            animator.SetBool("IsStopRU", isStopRU); // talking about which leg is up
            bool isStopLU = false;
            animator.SetBool("IsStopLU", isStopLU);

            if (false) {
                animator.SetFloat("IsRU", 0f);
            }

            // animator.SetTrigger("Slide");

            // if (characterData.playerData.detectWall) {
            //     animator.SetTrigger("HitWall");
            // }
            
            animator.SetBool("IsAvoidMode", false); //isDodgeDown

            if (false) {
                animator.SetTrigger("Vault");
                animator.SetTrigger("Vault2m");
                animator.SetTrigger("Vault05m");
                animator.SetTrigger("Vault1m");
            }

            animator.SetBool("IsJump", characterData.playerData.wishJumpPress);
            // animator.SetBool("IsFalling", characterData.playerData.falling);
            // animator.SetBool("IsDead", characterData.playerData.dead);
            // animator.SetBool("IsCrouch", characterData.playerData.crouching);

            // animator.SetFloat("Horizontal", 0f);
            // animator.SetFloat("Vertical", 0f);
            // animator.SetFloat("HorAimAngle", 0f);
            // animator.SetFloat("VerAimAngle", 0f);
            // animator.SetFloat("WalkStopAngle", 0f);
            // animator.SetFloat("Crouch_notUsed", 0f);
            // animator.SetFloat("IsControl", 0f);
            // animator.SetFloat("X", 0f);
            // animator.SetFloat("Z", 0f);
            // animator.SetFloat("RawInputAngle", 0f);
            // animator.SetFloat("FallingTime", 0f);

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