using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine 

{
    public class PlayerStateGrounded : PlayerState
    {
        public PlayerStateGrounded(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            _isRootState = true;
            name = "grounded";
        }

        public override void EnterState()
        {
            Debug.Log("ENTER GROUNDED");
            // Debug.Log(ctx.moveData.velocity);
            clutchFlag = true;
            ctx.characterData.playerData.falling = false;

            ctx.animationController.animator.Play("Base Layer.Locomotion.LocomotionTree");

            InitializeSubStates();
        }

        public override void UpdateState()
        {

            // if (ctx.playerData.wishJumpUp && ctx.energySlider.value > .25f) {

            //     Jump();

            // }

            ctx.animationController.animator.SetFloat("xVel", Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.avatarLookRight));
            ctx.animationController.animator.SetFloat("zVel", Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.avatarLookForward));
            ctx.animationController.animator.SetFloat("velocityMagnitude", ctx.characterData.moveData.velocity.magnitude);

            if (ctx.characterData.moveData.velocity.magnitude > ctx.characterData.moveConfig.runSpeed && currentSubState.name != "burst") {
                ctx.characterData.moveData.velocity -= ctx.characterData.moveData.velocity * Time.deltaTime;
            }

            if (ctx.characterData.playerData.wishJumpUp && ctx.timerController.jumpTimer <= 0f) {
                Jump();
                ctx.animationController.animator.Play("Base Layer.JumpPlatformerStart");
            }

            ctx.collisionController.CollisionCheck();
            HandleGrapplePress();
            ctx.collisionController.GrappleCheck();

            CheckSwitchStates();
        }

        // Life must be understood backwards, but it must be lived forwards. - Søren Kierkegaard

        public override void ExitState()
        {
            // Debug.Log(currentSubState);
            // ctx.smokeLand.SetVector3("direction", Vector3.ProjectOnPlane(ctx.moveData.velocity, ctx.groundNormal));
            // ctx.smokeLand.SetVector3("position", ctx.moveData.origin);
            // Debug.Log("EXIT GROUNDED");
            // Debug.Log(ctx.moveData.velocity);

            
        }

        public override void InitializeSubStates()
        {
            if (currentSubState == null || currentSubState.name == "neutral") {
                SetSubState(factory.Neutral());

            } else if (currentSubState.name == "burst") {
                SetSubState(factory.Burst());
            } else if (currentSubState.name == "clutch") {
                SetSubState(factory.Clutch());
            }

        }

        public override void CheckSwitchStates()
        {
            if (ctx.characterData.playerData.grappling) {
                SwitchState(factory.Grapple());
            } else if (!ctx.characterData.playerData.grounded) {
                SwitchState(factory.Air());
            }
        }

        protected void Jump() {

            ctx.timerController.jumpTimer = .1f;
            // ctx.timerController.ignoreGravityTimer = Time.deltaTime;

            float forceJump = ctx.characterData.moveConfig.jumpForce; // 10 force vs 20 gravity = 3 units high, 1 second long
            ctx.characterData.moveData.velocity += Vector3.up * forceJump;

        }

        protected void Dash() {

            SetVelocity(ctx.characterData.playerData.wishMove * 15f);
            
        }

    }
}