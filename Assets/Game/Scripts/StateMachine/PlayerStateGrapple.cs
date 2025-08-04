using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine {
    public class PlayerStateGrapple : PlayerState {

    float initialDistance = 0f;
    Vector3 targetDir = Vector3.zero;
    Vector3 projectedLook = Vector3.zero;

    public PlayerStateGrapple(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = true;
        name = "grapple";
    }

    public override void EnterState()
    {
        // Debug.Log("ENTER GRAPPLE");
        InitializeSubStates();
        initialDistance = ctx.characterData.playerData.distanceFromGrapple;
        projectedLook = Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.grappleDir).normalized;
        targetDir = (ctx.characterData.playerData.grappleDir + projectedLook.normalized).normalized;
        time = 0f;
        ctx.characterData.moveConfig.grappleColor = Color.cyan;
    }

    // Try never to abandon hope for if you do, hope will surely try to abandon you. - Sally Brampton

    public override void UpdateState()
    {
        ctx.characterData.playerData.grappleDir = (ctx.characterData.playerData.grapplePoint - ctx.characterData.moveData.origin).normalized;
        ctx.characterData.playerData.distanceFromGrapple = Vector3.Distance(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint);

        ctx.vfxController._grappleArc.SetVector3("Pos0", ctx.characterData.moveData.origin + ctx.characterData.avatarLookForward);
        ctx.vfxController._grappleArc.SetVector3("Pos1", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .33f));
        ctx.vfxController._grappleArc.SetVector3("Pos2", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .66f));
        ctx.vfxController._grappleArc.SetVector3("Pos3", ctx.characterData.playerData.grapplePoint);
        ctx.vfxController._grappleArc.SetVector4("Color", ctx.characterData.moveConfig.grappleColor);

        DrawRope();

        OnlyAngularVelocityControl(2f);
        CancelVelocityAgainst(ctx.characterData.playerData.grappleDir, 20f);

        if (ctx.characterData.playerData.wishSprintDown) {
                
            // ctx.moveData.velocity = Vector3.Slerp(ctx.moveData.velocity, ctx.characterData.playerData.grappleDir * (ctx.characterData.playerData.distanceFromGrapple), Time.deltaTime * (1f + t));
            

            projectedLook = Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.grappleDir);
            
            Vector3 centerDir = (ctx.characterData.playerData.grappleDir + projectedLook.normalized).normalized;
            targetDir = centerDir;
            // targetDir = ctx.characterData.playerData.distanceFromGrapple < 30f ? centerDir : (centerDir + ctx.characterData.playerData.grappleDir).normalized;

            // float power = Mathf.Clamp((15f - ctx.moveData.velocity.magnitude), 0f, 15f);
            float power = Mathf.Sqrt(ctx.characterData.playerData.distanceFromGrapple * 2f) * Mathf.Clamp((40f - ctx.characterData.moveData.velocity.magnitude) / 5f, 2f, 8f);

            ctx.characterData.moveData.velocity += targetDir * power * Time.deltaTime;
            // ctx.characterData.moveData.velocity = Vector3.ClampMagnitude(ctx.characterData.moveData.velocity, initialDistance * 2f);

            ctx.characterData.moveConfig.grappleColor = Color.Lerp(ctx.characterData.moveConfig.grappleColor, ctx.characterData.moveConfig.accelColor, Time.deltaTime * 2f);
            
            time += Mathf.Min(1f, Time.deltaTime * 1f);

        } else {
            time = 0f;
            // initialDistance = Mathf.Max(initialDistance/2f, 30f);
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // Debug.Log("EXIT GRAPPLE");
        // ctx.jumpTimer = 0f;
        // ctx.timerController.ignoreGravityTimer = 1f;
    }

    public override void InitializeSubStates() // 762416 36987
    {
        SetSubState(factory.Neutral());
    }

    public override void CheckSwitchStates()
    {

        if (!ctx.characterData.playerData.wishGrappleDown && ctx.characterData.playerData.grounded) {
            DisconnectGrapple();
            SwitchState(_factory.Grounded());
            ctx.characterData.playerData.grappling = false;
        }

        else if (!ctx.characterData.playerData.wishGrappleDown && !ctx.characterData.playerData.grounded) {
            DisconnectGrapple();
            SwitchState(_factory.Air());
            ctx.characterData.playerData.grappling = false;
        }
    }

    public void DrawRope() {
    
        // var _lr = ctx.vfxController.grappleGun.GetComponent<LineRenderer>();

        // _lr.positionCount = 2;

        // _lr.useWorldSpace = true;

        // _lr.SetPosition(0, ctx.characterData.moveData.transform.position);
        // _lr.SetPosition(1, ctx.characterData.playerData.grapplePoint);

        // _lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);

        ctx.vfxController._grappleArc.SetVector3("Pos0", ctx.characterData.moveData.origin + ctx.characterData.avatarLookForward);
        ctx.vfxController._grappleArc.SetVector3("Pos1", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .33f));
        ctx.vfxController._grappleArc.SetVector3("Pos2", Vector3.Lerp(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint, .66f));
        ctx.vfxController._grappleArc.SetVector3("Pos3", ctx.characterData.playerData.grapplePoint);
        ctx.vfxController._grappleArc.SetVector4("Color", ctx.characterData.moveConfig.grappleColor);
        
        ctx.vfxController.grappleArc.enabled = true;
    }

    public void EraseRope() {
        ctx.vfxController.grappleArc.SetVector3("Pos0", ctx.vfxController.grappleGun.transform.position);
        ctx.vfxController.grappleArc.SetVector3("Pos1", ctx.vfxController.grappleGun.transform.position);
        ctx.vfxController.grappleArc.SetVector3("Pos2", ctx.vfxController.grappleGun.transform.position);
        ctx.vfxController.grappleArc.SetVector3("Pos3", ctx.vfxController.grappleGun.transform.position);
        ctx.vfxController.grappleArc.enabled = false;
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

    private void GrappleMove() {


            if (!ctx.characterData.playerData.wishJumpDown) OnlyAngularVelocityControl(2f);
            
            CancelVelocityAgainst(ctx.characterData.playerData.grappleDir, 50f);
            
            // ctx.moveData.velocity.y -= (ctx.moveConfig.gravity * Time.deltaTime * .5f);

            // if (ctx.characterData.playerData.wishJumpUp) {
            //    Vector3 wishDir = ctx.characterData.playerData.inputDir.magnitude > .25f ? avatarLookFlat * ctx.characterData.playerData.inputDir : ctx.avatarLookForward;
            //     BoostJump((ctx.avatarLookForward).normalized, Mathf.Max(ctx.moveData.velocity.magnitude, 30f));
            //     ctx.characterData.playerData.grappling = false;
            // }

            // Quaternion grappleRot = Quaternion.LookRotation(ctx.characterData.playerData.grappleDir);

            // Vector3 avatarInputDir = (ctx.avatarLookRotation * ctx.characterData.playerData.inputDir);

            // Vector3 grappleInputDir = avatarInputDir * Vector3.Dot(avatarInputDir, ctx.characterData.playerData.grappleDir);

            // ctx.moveData.velocity += grappleInputDir * ctx.moveConfig.walkSpeed * Time.deltaTime;
            
            if (ctx.characterData.playerData.wishSprintDown) {
                
                // ctx.moveData.velocity = Vector3.Slerp(ctx.moveData.velocity, ctx.characterData.playerData.grappleDir * (ctx.characterData.playerData.distanceFromGrapple), Time.deltaTime * (1f + t));
                

                projectedLook = Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.grappleDir);
                
                targetDir = (ctx.characterData.playerData.grappleDir + projectedLook.normalized).normalized;

                // float power = Mathf.Clamp((15f - ctx.moveData.velocity.magnitude), 0f, 15f);
                float power = Mathf.Sqrt(ctx.characterData.playerData.distanceFromGrapple * 2f) * Mathf.Clamp((40f - ctx.characterData.moveData.velocity.magnitude) / 5f, 2f, 8f);

                Debug.Log(power);

                ctx.characterData.moveData.velocity += targetDir * power * Time.deltaTime;
                // ctx.characterData.moveData.velocity = Vector3.ClampMagnitude(ctx.characterData.moveData.velocity, initialDistance * 2f);

                ctx.characterData.moveConfig.grappleColor = Color.Lerp(ctx.characterData.moveConfig.grappleColor, ctx.characterData.moveConfig.accelColor, Time.deltaTime * 2f);
                
                time += Mathf.Min(1f, Time.deltaTime * 1f);
                // Debug.Log(t);

            }
            

            if (Vector3.Dot(ctx.characterData.playerData.grappleDir, ctx.characterData.viewForward) < -.1f || ctx.characterData.playerData.wishTumbleUp || ctx.characterData.playerData.distanceFromGrapple < ctx.characterData.moveConfig.minDistance) {
                ctx.characterData.playerData.grappling = false;
                // releaseVelocity = ctx.moveData.velocity;
                // ctx.characterData.releaseTimer = 5f;

                // releasedPoints[0] = ctx.vfxController.grappleArc.GetVector3("Pos0");
                // releasedPoints[1] = ctx.vfxController.grappleArc.GetVector3("Pos1");
                // releasedPoints[2] = ctx.vfxController.grappleArc.GetVector3("Pos2");
                // releasedPoints[3] = ctx.vfxController.grappleArc.GetVector3("Pos3");
            }

            // if (ctx.characterData.playerData.wishJumpUp) {
            //     // CancelVelocityAgainst(ctx.avatarLookForward, .5f);
            //     Vector3 wishDir = ctx.characterData.playerData.inputDir.magnitude > .25f ? avatarLookFlat * ctx.characterData.playerData.inputDir : ctx.avatarLookForward;
            //     BoostJump(wishDir, Mathf.Max(30f, ctx.characterData.playerData.distanceFromGrapple * 2f));
            //     ctx.sphereLines.Stop();
            //     ctx.characterData.playerData.grappling = false;
            //     ctx.characterData.playerData.attacking = true;
            //     ctx.lungeCooldownTimer = 1f;
            // }

            // if (ctx.characterData.playerData.wishJumpDown) {
            //     // ctx.framingCam.m_CameraDistance = Mathf.Lerp(ctx.framingCam.m_CameraDistance, 1.5f, Time.deltaTime * 4f);
            //     ctx.sphereLines.SetFloat("Speed", -ctx.characterData.playerData.vCharge);
            //     ctx.sphereLines.Play();

            //     ctx.focusAimBlend = Mathf.Lerp(ctx.focusAimBlend, .8f, Time.deltaTime * 8f);
                
            //     // SubtractVelocityAgainst(ref ctx.moveData.velocity, -ctx.moveData.velocity.normalized, ctx.moveData.velocity.magnitude * 2f);

            //     BrakeCharge(ctx.avatarLookForward);
            // }

    }

}
}