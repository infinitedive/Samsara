using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{

    public class PlayerStateNeutral : PlayerState {

        public PlayerStateNeutral(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "neutral";
        }

        public override void EnterState() // action
        {
            // oldMomentum = Vector3.Scale(ctx.playerData.velocity, new Vector3(1f, 0f, 1f));
            // Debug.Log("ENTER NEUTRAL");
            // Debug.Log(ctx.moveData.velocity);
            time = 0f;
            // gearController.GearOne();
        }

        public override void UpdateState() // duration
        {

            WalkMovementUpdate();

            CheckSwitchStates();
        }

        private void NeutralMovement()
        {

            if (ctx.characterData.playerData.grounded) {

            } else {

            }

        }

        public override void ExitState() // completion
        {

        }

        public override void InitializeSubStates()
        {

        }

        public override void CheckSwitchStates()
        {
            // if (ctx.characterData.playerData.wishSkatePress && ctx.characterData.playerData.grounded) {
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