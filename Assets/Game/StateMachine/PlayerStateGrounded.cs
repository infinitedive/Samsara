using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine 

{
    public class PlayerStateGrounded : PlayerState
    {
        public PlayerStateGrounded(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            _isRootState = true;
            name = "grounded";
        }

        public override void EnterState()
        {
            // Debug.Log("ENTER GROUNDED");
            // Debug.Log(ctx.moveData.velocity);
            clutchFlag = true;

            InitializeSubStates();
        }

        public override void UpdateState()
        {

            // if (ctx.playerData.wishJumpUp && ctx.energySlider.value > .25f) {

            //     Jump();

            // }

            if (ctx.characterData.moveData.velocity.magnitude > ctx.characterData.moveConfig.runSpeed && currentSubState.name != "burst") {
                ctx.characterData.moveData.velocity -= ctx.characterData.moveData.velocity * Time.deltaTime;
            }

            if (ctx.characterData.playerData.wishJumpUp && ctx.timerController.jumpTimer <= 0f) {
                ctx.timerController.jumpTimer = .5f;
                Jump();
            }

            ctx.collisionHandler.CollisionCheck();

            


            CheckSwitchStates();
        }

        public override void ExitState()
        {
            Debug.Log(currentSubState);
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

    }
}