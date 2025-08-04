using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine

{


    public class PlayerStateBurst : PlayerState {



        public PlayerStateBurst(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "burst";
        }

        public override void EnterState() // action
        {
            // oldMomentum = Vector3.Scale(ctx.playerData.velocity, new Vector3(1f, 0f, 1f));
            Debug.Log("ENTER Burst");
            // Debug.Log(ctx.moveData.velocity);
            time = 2f;

            
            
        }

        public override void UpdateState() // duration
        {

            ctx.characterData.moveData.velocity += (2f * (ctx.characterData.moveConfig.maxVelocity - ctx.characterData.moveData.velocity.magnitude)) * ctx.characterData.velocityForward * Time.deltaTime;

            if (true) {

                FlyMovementUpdate();

                // if (ctx.playerData.wishDashUp) {
                //     Dash();
                // }

            } else {
                SkateMovementUpdate(Mathf.Pow(3f, .5f));
            }

            if (ctx.characterData.playerData.bonusTime) {
                time += 1f;
            }

            // if (ctx.playerData.wishFirePress) {
            //     ctx.TriggerThing();
            // }

            Debug.Log(time);

            time -= Time.deltaTime;

            CheckSwitchStates();
        }

        public override void ExitState() // completion
        {
            Debug.Log("EXIT BURST");
        }

        public override void InitializeSubStates()
        {

        }

        public override void CheckSwitchStates()
        {
            if (time <= 0f) {
                SwitchState(factory.Neutral());
            }

            // if (ctx.characterData.playerData.wishSkatePress) {
            //     SwitchState(factory.Clutch());
            // }

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