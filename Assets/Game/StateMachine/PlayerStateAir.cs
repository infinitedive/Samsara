using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine 

{
    public class PlayerStateAir : PlayerState {

    public PlayerStateAir(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = true;
        name = "air";
    }

    public override void EnterState()
    {
        // Debug.Log("ENTER AIR");
        // oldMomentum = Vector3.zero;
        // Debug.Log(ctx.moveData.velocity);

        InitializeSubStates();
    }

    public override void UpdateState()
    {

        if (ctx.timerController.ignoreGravityTimer > 0f || ctx.characterData.playerData.detectWall || currentSubState.name == "burst" || currentSubState.name == "clutch") {  } 
        else  {
            ctx.characterData.moveData.velocity.y -= (ctx.characterData.moveConfig.gravity * Time.deltaTime * 1f);
        }


        // AirMovement();
        // OnlyAngularVelocity(ctx.characterData.playerData.wishMove, 1f);

        ctx.collisionHandler.CollisionCheck();

        if (ctx.characterData.playerData.detectWall) {
            ctx.characterData.moveData.velocity.y = Mathf.Lerp(ctx.characterData.moveData.velocity.y, 0f, Time.deltaTime * 10f);
            // ctx.characterData.moveData.velocity.y = 0f;

            ctx.characterData.moveData.velocity -= ctx.characterData.playerData.wallNormal;

            if (ctx.characterData.playerData.wishJumpUp && ctx.timerController.wallTouchTimer <= 0f) {
                Jump((ctx.characterData.playerData.wallNormal * 2f + Vector3.up).normalized * 1.5f);
                ctx.timerController.wallTouchTimer = .2f;
            }
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // Debug.Log("EXIT AIR");
        // Debug.Log(ctx.moveData.velocity);
    }

    public override void InitializeSubStates()
    {

        Debug.Log(currentSubState);

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

        if (ctx.characterData.playerData.grounded) {
            SwitchState(factory.Grounded());
        } else if (ctx.characterData.playerData.grappling) {
            SwitchState(factory.Grapple());
        }

    }

    

    private void AirMovement() {
            
        if (ctx.characterData.playerData.detectWall && !ctx.characterData.playerData.grappling) { // wall run state

            if (ctx.doubleJump) {
                ctx.vfxController.airHike.SetVector3("origin", ctx.characterData.moveData.origin);
                ctx.vfxController.airHike.SetVector3("lookAt", ctx.characterData.playerData.wallNormal / 2f);
                ctx.vfxController.airHike.SetFloat("size", 4f);
                ctx.vfxController.airHike.Play();
            }

            ctx.doubleJump = false;

            if (Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.playerData.wallNormal) <= -7.5f) {
                
                ctx.vfxController.smokeLand.SetVector3("velocity", Vector3.ProjectOnPlane(ctx.characterData.moveData.velocity / 2f, ctx.characterData.playerData.wallNormal));
                ctx.vfxController.smokeLand.SetVector3("position", ctx.characterData.moveData.origin - ctx.characterData.playerData.wallNormal / 2f);
                ctx.vfxController.smokeLand.SetVector3("eulerAngles", Quaternion.LookRotation(ctx.characterData.playerData.wallNormal, Vector3.ProjectOnPlane(-ctx.characterData.velocityForward, ctx.characterData.playerData.wallNormal)).eulerAngles);
                ctx.vfxController.smokeLand.Play();
            }
            
            // ctx.characterData.moveData.velocity.y = Mathf.Lerp(ctx.characterData.moveData.velocity.y, 0f, Time.deltaTime / 4f);

            // if (ctx.characterData.moveData.velocity.magnitude > ctx.moveConfig.runSpeed + 10f) {
            //     SubtractVelocityAgainst(Vector3.ProjectOnPlane(ctx.characterData.moveData.velocity.normalized, ctx.characterData.playerData.wallNormal), ctx.characterData.moveData.velocity.magnitude / 2f);
            // } 
            // else 
            // if (ctx.characterData.moveData.velocity.magnitude < ctx.moveConfig.walkSpeed && ctx.characterData.playerData.wishShiftDown) {
            //     AddVelocityTo(Vector3.ProjectOnPlane(ctx.characterData.moveData.velocity.normalized, ctx.characterData.playerData.wallNormal), ctx.moveConfig.walkSpeed + 5f);
            // }


            // if (ctx.jumpTimer <= 0f) ctx.characterData.moveData.velocity += -ctx.characterData.playerData.wallNormal;

            ctx.vfxController.smoke.SetVector3("position", ctx.characterData.moveData.origin - ctx.characterData.playerData.wallNormal / 2f);
            ctx.vfxController.smoke.SetVector3("direction", -ctx.characterData.moveData.velocity.normalized);

            // if (ctx.jumpTimer <= 0f) {

            //     ctx.characterData.moveData.velocity += -ctx.characterData.playerData.wallNormal;
            //     ctx.characterData.moveData.velocity.y = Mathf.Lerp(ctx.characterData.moveData.velocity.y, 0f, Time.deltaTime / 2f);

            // }


            // if (ctx.characterData.playerData.wishJumpDown) {
            //     // ctx.framingCam.m_CameraDistance = Mathf.Lerp(ctx.framingCam.m_CameraDistance, 3f, Time.deltaTime * 4f);
            //     ctx.sphereLines.SetFloat("Speed", -ctx.characterData.playerData.vCharge);
            //     ctx.sphereLines.Play();
                
            //     // SubtractVelocityAgainst(ref ctx.characterData.moveData.velocity, -ctx.characterData.moveData.velocity.normalized, ctx.characterData.moveData.velocity.magnitude / 4f);

            //     BrakeCharge(ctx.avatarLookForward);
            // }

            // if (ctx.characterData.playerData.wishJumpUp) {
            //     BoostJump((ctx.characterData.playerData.wallNormal + ctx.avatarLookForward).normalized, Mathf.Max(ctx.characterData.moveData.velocity.magnitude, 20f));
            //     ctx.sphereLines.Stop();
            // }

        } else { // falling state

            // ctx.bezierCurve.PredictGravityArc(ctx.characterData.moveData.origin, ctx.moveConfig.gravity, ctx.characterData.moveData.velocity);
            // ctx.bezierCurve.DrawProjection();

            // if (ctx.characterData.playerData.wishJumpDown && !ctx.doubleJump) {
            //     SubtractVelocityAgainst(ref ctx.characterData.moveData.velocity, -ctx.characterData.moveData.velocity.normalized, ctx.characterData.moveData.velocity.magnitude / 2f);
            //     ctx.vcam.m_CameraDistance = Mathf.Lerp(ctx.vcam.m_CameraDistance, 3f, Time.deltaTime * 4f);

            //     BrakeCharge();

            //     ctx.sphereLines.SetFloat("Speed", -ctx.characterData.playerData.vCharge);

            //     ctx.sphereLines.Play();
            // }

            // if (ctx.characterData.playerData.wishJumpUp) {
            //     AirJump(Vector3.up);
            //     ctx.sphereLines.Stop();
            // }

        }
    }

}
}