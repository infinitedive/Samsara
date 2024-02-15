using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{


    public class PlayerStateClutch : PlayerState {



        public PlayerStateClutch(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "clutch";
        }

        public override void EnterState() // action
        {
            // oldMomentum = Vector3.Scale(ctx.playerData.velocity, new Vector3(1f, 0f, 1f));
            // Debug.Log("ENTER NEUTRAL");
            // Debug.Log(ctx.moveData.velocity);
            time = 1f;
            clutchFlag = false;
            
        }

        public override void UpdateState() // duration
        {

            ctx.characterData.moveData.velocity = Vector3.Lerp(ctx.characterData.moveData.velocity, Vector3.zero, Time.deltaTime * 4f);

            time -= Time.deltaTime;

            CheckSwitchStates();
        }

        public override void ExitState() // completion
        {
            ctx.characterData.moveData.velocity = ctx.characterData.moveConfig.runSpeed * ctx.characterData.avatarLookForwardFlat;
        }

        public override void InitializeSubStates()
        {

        }

        public override void CheckSwitchStates()
        {
            if (time <= 0f) {
                SwitchState(factory.Burst());
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