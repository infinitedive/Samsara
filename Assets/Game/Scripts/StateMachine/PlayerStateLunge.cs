using UnityEngine;
using Game.Controllers;

namespace Game.StateMachine {
    public class PlayerStateLunge : PlayerState {
    
        Vector3 toTarget = Vector3.zero;
        Vector3 startPos = Vector3.zero;
        Vector3 di = Vector3.zero;
        Vector3 centerPivot = Vector3.zero;
    
        public PlayerStateLunge(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
            _isRootState = false;
            name = "lunge";
        }
    
        public override void EnterState()
        {
            // Debug.Log("ENTER LUNGE");
            time = 0f;
    
        }
    
        public override void UpdateState()
        {
    
            if (ctx.characterData.playerData.wishFireDown && !ctx.characterData.playerData.attacking) {
    
                di = (Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.focusDir).normalized * 5f);
    
                toTarget = (ctx.characterData.playerData.focusPoint - ctx.characterData.moveData.origin);
    
                startPos = ctx.characterData.moveData.origin;
    
                centerPivot = startPos + toTarget * .5f - di;
    
                // ctx.bezierCurve.DrawCircle(centerPivot);
    
    
    
            }
    
            else {
                ctx.characterData.playerData.attacking = true;
    
                // if (ctx.characterData.playerData.wishFireUp) {
                //     ctx.moveData.velocity /= 2f;
                // }
    
                // ctx.moveData.origin = Vector3.Slerp(startPos, ctx.characterData.playerData.focusPoint, t * 2f);
                // ctx.moveData.origin = ctx.CenteredSlerp(startPos, ctx.characterData.playerData.focusPoint, centerPivot, t);
    
                if (ctx.characterData.playerData.grappling) {
                    di = (Vector3.ProjectOnPlane(ctx.characterData.avatarLookForward, -ctx.characterData.playerData.grappleDir).normalized * 2f);
                    toTarget = (ctx.characterData.playerData.grapplePoint - ctx.characterData.moveData.origin);
                    ctx.characterData.moveData.velocity = Vector3.Slerp(ctx.characterData.moveData.velocity, toTarget, Time.deltaTime * (2f));
                }
                else if (ctx.characterData.playerData.wishFireUp) {
                    // BoostJump((toTarget + di).normalized, toTarget.magnitude * 2f);
                    ctx.timerController.ignoreGravityTimer = .9f;
                }
    
                // ctx.bezierCurve.InterpolateAcrossCircleC1(startPos, ctx.characterData.playerData.focusPoint, centerPivot, t);
    
                // float length = (end - start).magnitude * Mathf.PI / 2f;
                // float estimatedTime =  length / Mathf.Max(ctx.moveConfig.runSpeed, ctx.moveData.velocity.magnitude);
    
    
                time += Time.deltaTime;
            }
    
            CheckSwitchStates();
        }
    
        public override void ExitState()
        {
            ctx.timerController.ignoreGravityTimer = 1f;
            // ctx.slashObj.SetActive(true);
            // ctx.slash.Play();
        }
    
        public override void InitializeSubStates()
        {
    
        }
    
        public override void CheckSwitchStates()
        {
    
            // if (t > 2f || ctx.targetLength > 0) {
            //     SwitchState(factory.LungeCooldown());
            // }
        }
    
    }
}