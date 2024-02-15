using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{

    public class PlayerStateBurst : PlayerState {

        public PlayerStateBurst(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "burst";
        }

        public override void EnterState() // action
        {
            // oldMomentum = Vector3.Scale(ctx.playerData.velocity, new Vector3(1f, 0f, 1f));
            // Debug.Log("ENTER NEUTRAL");
            // Debug.Log(ctx.moveData.velocity);
            time = 0f;
            ctx.characterData.moveData.velocity *= 1.5f;
        }

        public override void UpdateState() // duration
        {

            if (true) {

                FlyMovementUpdate();

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
            if (!ctx.characterData.playerData.wishJumpDown) {
                SwitchState(factory.Neutral());
            }

            // if (ctx.playerData.wishDashPress && ctx.playerData.grounded) {
            //     SwitchState(factory.Dash());
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