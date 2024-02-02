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

            InitializeSubStates();
        }

        public override void UpdateState()
        {

            // if (ctx.playerData.wishJumpUp && ctx.energySlider.value > .25f) {

            //     Jump();

            // }

            if (ctx.characterData.playerData.wishJumpPress && ctx.timerController.jumpTimer <= 0f) {
                ctx.timerController.jumpTimer = .5f;
                Jump();
            }

            ctx.collisionHandler.CollisionCheck();

            


            CheckSwitchStates();
        }

        public override void ExitState()
        {
            // ctx.smokeLand.SetVector3("direction", Vector3.ProjectOnPlane(ctx.moveData.velocity, ctx.groundNormal));
            // ctx.smokeLand.SetVector3("position", ctx.moveData.origin);
            // Debug.Log("EXIT GROUNDED");
            // Debug.Log(ctx.moveData.velocity);

            
        }

        public override void InitializeSubStates()
        {
            // if (ctx.playerData.attacking) {
            //     SetSubState(factory.Lunge());
            // } else if (ctx.playerData.wishShiftDown) {
            //     // SetSubState(factory.Dash());
            // } else {
                SetSubState(factory.Neutral());
            // }

            

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