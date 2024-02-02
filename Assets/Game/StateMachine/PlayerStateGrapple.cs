using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine {
    public class PlayerStateGrapple : PlayerState {

    Vector3 grapplePointDir = Vector3.zero;
    float initialDistance = 0f;
    Vector3 targetDir = Vector3.zero;
    Vector3 projectedLook = Vector3.zero;

    public PlayerStateGrapple(SkateCharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = true;
        name = "grapple";
    }

    public override void EnterState()
    {
        // Debug.Log("ENTER GRAPPLE");
        InitializeSubStates();
        initialDistance = ctx.characterData.playerData.distanceFromGrapple;
        targetDir = (ctx.characterData.playerData.grappleDir + projectedLook.normalized).normalized;
        projectedLook = Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.grappleDir).normalized;
        time = 0f;
    }

    public override void UpdateState()
    {
        GrappleUpdate();

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // Debug.Log("EXIT GRAPPLE");
        // ctx.jumpTimer = 0f;
        ctx.timerController.ignoreGravityTimer = 1f;
    }

    public override void InitializeSubStates() // 762416 36987
    {
        SetSubState(factory.Noop());
    }

    public override void CheckSwitchStates()
    {

        if (!ctx.characterData.playerData.wishGrappleDown && ctx.characterData.playerData.grounded ) {
            ctx.StopGrapple();
            SwitchState(_factory.Grounded());
            ctx.characterData.playerData.grappling = false;
        } 

        else if (!ctx.characterData.playerData.wishGrappleDown && !ctx.characterData.playerData.grounded) {
            ctx.StopGrapple();
            SwitchState(_factory.Air());
            ctx.characterData.playerData.grappling = false;
        }
    }

    private void GrappleMoveTargeting() {

        Vector3 grappleDir = (ctx.characterData.playerData.grapplePoint - ctx.characterData.moveData.origin).normalized;

        float lookCurve = Mathf.Clamp(Vector3.Dot(ctx.characterData.avatarLookForward, grappleDir), 0f, 1f);

        // ctx.bezierCurve.GetSpiralCurve(0.7f, t);
        // ctx.bezierCurve.SpiralCurvePoint();
        // ctx.bezierCurve.CreateSpiralFromVelocity();
        // ctx.bezierCurve.CreateArcFromVelocity();
        // ctx.bezierCurve.GrappleArc(ctx.playerData.grappleNormal, ctx.playerData.grapplePoint);
        

    }

    private void GrappleUpdate() {

        ctx.characterData.playerData.grappleDir = (ctx.characterData.playerData.grapplePoint - ctx.characterData.moveData.origin).normalized;

        ctx.vfxController._grappleArc.SetVector3("Pos0", ctx.characterData.moveData.origin + ctx.characterData.avatarLookForward);
        ctx.vfxController._grappleArc.SetVector3("Pos1", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .33f));
        ctx.vfxController._grappleArc.SetVector3("Pos2", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .66f));
        ctx.vfxController._grappleArc.SetVector3("Pos3", ctx.characterData.playerData.grapplePoint);
        ctx.vfxController._grappleArc.SetVector4("Color", ctx.characterData.moveConfig.grappleColor);

        ctx.DrawRope();

        Debug.Log(ctx.characterData.playerData.distanceFromGrapple);

        CancelVelocityAgainst(ctx.characterData.playerData.grappleDir, 20f);



    }

    // private void GrappleMove() {


    //         if (!ctx.playerData.wishJumpDown) OnlyAngularVelocityControl(2f);
            
    //         CancelVelocityAgainst(ctx.playerData.grappleDir, 50f);
            
    //         // ctx.moveData.velocity.y -= (ctx.moveConfig.gravity * Time.deltaTime * .5f);

    //         // if (ctx.playerData.wishJumpUp) {
    //         //    Vector3 wishDir = ctx.playerData.inputDir.magnitude > .25f ? avatarLookFlat * ctx.playerData.inputDir : ctx.avatarLookForward;
    //         //     BoostJump((ctx.avatarLookForward).normalized, Mathf.Max(ctx.moveData.velocity.magnitude, 30f));
    //         //     ctx.playerData.grappling = false;
    //         // }

    //         // Quaternion grappleRot = Quaternion.LookRotation(ctx.playerData.grappleDir);

    //         // Vector3 avatarInputDir = (ctx.avatarLookRotation * ctx.playerData.inputDir);

    //         // Vector3 grappleInputDir = avatarInputDir * Vector3.Dot(avatarInputDir, ctx.playerData.grappleDir);

    //         // ctx.moveData.velocity += grappleInputDir * ctx.moveConfig.walkSpeed * Time.deltaTime;
            
    //         if (ctx.playerData.wishRunDown) {
                
    //             // ctx.moveData.velocity = Vector3.Slerp(ctx.moveData.velocity, ctx.playerData.grappleDir * (ctx.playerData.distanceFromGrapple), Time.deltaTime * (1f + t));
                

    //             projectedLook = Vector3.ProjectOnPlane(ctx.avatarLookForward, -ctx.playerData.grappleDir);
                
    //             targetDir = (ctx.playerData.grappleDir + projectedLook.normalized).normalized;

    //             // float power = Mathf.Clamp((15f - ctx.moveData.velocity.magnitude), 0f, 15f);
    //             float power = Mathf.Sqrt(ctx.playerData.distanceFromGrapple * 2f) * Mathf.Clamp((40f - ctx.moveData.velocity.magnitude) / 5f, 2f, 8f);

    //             Debug.Log(power);

    //             ctx.moveData.velocity += targetDir * power * Time.deltaTime;
    //             // ctx.moveData.velocity = Vector3.ClampMagnitude(ctx.moveData.velocity, initialDistance * 2f);

    //             ctx.moveConfig.grappleColor = Color.Lerp(ctx.moveConfig.grappleColor, ctx.moveConfig.accelColor, Time.deltaTime * 2f);
                
    //             time += Mathf.Min(1f, Time.deltaTime * 1f);
    //             // Debug.Log(t);

    //         }
    //         else {
    //             time = 0f;
    //             // initialDistance = Mathf.Max(initialDistance/2f, 30f);
    //         }

    //         if (Vector3.Dot(ctx.playerData.grappleDir, ctx.viewForward) < -.1f || ctx.playerData.wishTumbleUp || ctx.playerData.distanceFromGrapple < ctx.moveConfig.minDistance) {
    //             ctx.playerData.grappling = false;
    //             // releaseVelocity = ctx.moveData.velocity;
    //             ctx.releaseTimer = 5f;

    //             releasedPoints[0] = ctx._grappleArc.GetVector3("Pos0");
    //             releasedPoints[1] = ctx._grappleArc.GetVector3("Pos1");
    //             releasedPoints[2] = ctx._grappleArc.GetVector3("Pos2");
    //             releasedPoints[3] = ctx._grappleArc.GetVector3("Pos3");
    //         }

    //         // if (ctx.playerData.wishJumpUp) {
    //         //     // CancelVelocityAgainst(ctx.avatarLookForward, .5f);
    //         //     Vector3 wishDir = ctx.playerData.inputDir.magnitude > .25f ? avatarLookFlat * ctx.playerData.inputDir : ctx.avatarLookForward;
    //         //     BoostJump(wishDir, Mathf.Max(30f, ctx.playerData.distanceFromGrapple * 2f));
    //         //     ctx.sphereLines.Stop();
    //         //     ctx.playerData.grappling = false;
    //         //     ctx.playerData.attacking = true;
    //         //     ctx.lungeCooldownTimer = 1f;
    //         // }

    //         // if (ctx.playerData.wishJumpDown) {
    //         //     // ctx.framingCam.m_CameraDistance = Mathf.Lerp(ctx.framingCam.m_CameraDistance, 1.5f, Time.deltaTime * 4f);
    //         //     ctx.sphereLines.SetFloat("Speed", -ctx.playerData.vCharge);
    //         //     ctx.sphereLines.Play();

    //         //     ctx.focusAimBlend = Mathf.Lerp(ctx.focusAimBlend, .8f, Time.deltaTime * 8f);
                
    //         //     // SubtractVelocityAgainst(ref ctx.moveData.velocity, -ctx.moveData.velocity.normalized, ctx.moveData.velocity.magnitude * 2f);

    //         //     BrakeCharge(ctx.avatarLookForward);
    //         // }

    // }

}
}