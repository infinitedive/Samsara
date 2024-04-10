using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{

    public class PlayerStateGrappleHang : PlayerState {

        public PlayerStateGrappleHang(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "grapplehang";
        }

        public override void EnterState() // action
        {
            
        }

        public override void UpdateState() // duration
        {

            GrappleHang();

            CheckSwitchStates();
        }

        private void GrappleHang()
        {

            if (ctx.characterData.playerData.grounded) {

            } else {

            }


            ctx.characterData.playerData.grappleDir = (ctx.characterData.playerData.grapplePoint - ctx.characterData.moveData.origin).normalized;

            ctx.vfxController.GrappleVisuals();

            CancelVelocityAgainst(ctx.characterData.playerData.grappleDir, 20f);

        }

        public override void ExitState() // completion
        {

        }

        public override void InitializeSubStates()
        {

        }

        public override void CheckSwitchStates()
        {
            // if (ctx.characterData.playerData.wishDashPress && ctx.characterData.playerData.grounded) {
            //     SwitchState(factory.Burst());
            // }

            // if (ctx.playerData.wishFireDown) {
            //     SwitchState(factory.Lunge());
            // }

            // if (ctx.playerData.wishShiftDown) {
            //     // oldMomentum = Vector3.zero;
            //     SwitchState(_factory.Dash());
            // } 
            // else if (!ctx.playerData.grounded) {
            //     SwitchState(factory.Fall());
            // }
            // else if (ctx.playerData.grappling) {
            //     SwitchState(_factory.Grapple());
            // }

        }

    }
}