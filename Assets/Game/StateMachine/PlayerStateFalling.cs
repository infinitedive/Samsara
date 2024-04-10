using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{

    public class PlayerStateFalling : PlayerState {

        public PlayerStateFalling(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "neutral";
        }

        public override void EnterState() // action
        {
            // oldMomentum = Vector3.Scale(ctx.playerData.velocity, new Vector3(1f, 0f, 1f));
            // Debug.Log("ENTER NEUTRAL");
            // Debug.Log(ctx.moveData.velocity);
            time = 0f;
        }

        public override void UpdateState() // duration
        {

            if (ctx.characterData.playerData.grounded && ctx.characterData.moveData.velocity.magnitude <= ctx.characterData.moveConfig.runSpeed) {

                WalkMovementUpdate();

                // if (ctx.playerData.wishDashUp) {
                //     Dash();
                // }

            } else {
                SkateMovementUpdate(Mathf.Pow(3f, .5f));
            }

            if (ctx.characterData.playerData.wishJumpDown) {

                

            }

            // if (ctx.playerData.wishFirePress) {
            //     ctx.TriggerThing();
            // }

            CheckSwitchStates();
        }

        public override void ExitState() // completion
        {

        }

        public override void InitializeSubStates()
        {

        }

        public override void CheckSwitchStates()
        {
            if (ctx.characterData.playerData.wishDashPress && ctx.characterData.playerData.grounded) {
                SwitchState(factory.Burst());
            }

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