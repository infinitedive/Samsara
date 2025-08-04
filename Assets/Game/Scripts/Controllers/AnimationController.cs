using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

/*
We speak often about ideas as brainchildren. What we do not realize is that brainchildren, like all babies, should not be dragged from the creative womb prematurely. 
Ideas, like stalactites and stalagmites, form in the dark inner cave of consciousness. They form in drips and drops, not by squared-off building blocks. 
We must learn to wait for an idea to hatch. Or, to use a gardening image, we must learn to not pull our ideas up by the roots to see if they are growing.
*/

namespace Game.Controllers {
    public class AnimationController : MonoBehaviour
    {

        private CharacterData characterData;
        public Animator animator;

        private void Awake() {

            characterData = GetComponent<CharacterData>();
            // animator = transform.GetChild(2).GetComponent<Animator>();

        }

        public void Locomotion() {
            
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
            float walkStartAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(characterData.bodyForward, Vector3.up), characterData.playerData.wishMove, Vector3.up);
            Debug.Log(walkStartAngle);
            animator.SetFloat("InputAngle", inputAngle); // -35 - 35
            animator.SetFloat("WalkStartAngle", walkStartAngle);
            // animator.SetFloat("InteractAngle", 0f);
            // animator.SetFloat("SprintFactor", 1f - (characterData.moveConfig.walkSpeed - characterData.moveData.velocity.magnitude)/characterData.moveConfig.walkSpeed);
            animator.SetFloat("FloorAngle", 0f);

            float inputMagnitude = 0f;

            if (characterData.playerData.wishMove != Vector3.zero) {
                inputMagnitude = .3f;

                if (characterData.playerData.wishSkateDown) {
                    inputMagnitude = .75f;
                } else if (characterData.playerData.wishSprintDown) {
                    inputMagnitude = 1f;
                }


            }

            animator.SetFloat("InputMagnitude", inputMagnitude);
            // animator.Play();
            // animator.SetBool("Running", characterData.playerData.running);
            bool isStopRU = characterData.playerData.wishMove == Vector3.zero && characterData.moveData.velocity.magnitude > 1f;
            animator.SetBool("IsStopRU", isStopRU); // talking about which leg is up
            bool isStopLU = false;
            animator.SetBool("IsStopLU", isStopLU);

            if (true) {
                animator.SetFloat("IsRU", 1f);
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

            animator.SetBool("IsJump", !characterData.playerData.grounded);
            animator.SetBool("IsFalling", characterData.playerData.falling);
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