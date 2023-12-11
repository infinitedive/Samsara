using UnityEngine;

public class PlayerStateGrounded : PlayerBaseState
{
    public PlayerStateGrounded(PlayerCharacter currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        name = "grounded";
    }

    public override void EnterState()
    {
        // Debug.Log("ENTER GROUNDED");
        // Debug.Log(ctx.moveData.velocity);

        InitializeSubStates();
    }

    public override void UpdateState()
    {

        // Debug.Log(ctx.moveData.velocity.magnitude);

        if (ctx.playerData.wishJumpUp && ctx.energySlider.value > .25f) {
            // ctx.framingCam.m_CameraDistance = Mathf.Lerp(ctx.framingCam.m_CameraDistance, 3f, Time.deltaTime * 4f);
            // ctx.sphereLines.SetFloat("Speed", -ctx.playerData.vCharge);
            // ctx.sphereLines.Play();
            
            // SubtractVelocityAgainst(ref ctx.moveData.velocity, -ctx.moveData.velocity.normalized, ctx.moveData.velocity.magnitude * 2f);

            // BrakeCharge(ctx.avatarLookForward);

            Jump();

        }

        // if (ctx.playerData.wishJumpUp && !ctx.playerData.grappling) {
        //     BoostJump(ctx.avatarLookForward, Mathf.Max(ctx.moveData.velocity.magnitude, 30f));
        // }

        ctx.CollisionCheck();


        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // ctx.smokeLand.SetVector3("direction", Vector3.ProjectOnPlane(ctx.moveData.velocity, ctx.groundNormal));
        // ctx.smokeLand.SetVector3("position", ctx.moveData.origin);
        // Debug.Log("EXIT GROUNDED");
        // Debug.Log(ctx.moveData.velocity);

        
    }

    public override void InitializeSubStates()
    {
        // if (ctx.playerData.attacking) {
        //     SetSubState(factory.Lunge());
        // } else if (ctx.playerData.wishShiftDown) {
        //     // SetSubState(factory.Dash());
        // } else {
            SetSubState(factory.Neutral());
        // }

        

    }

    public override void CheckSwitchStates()
    {
        // if (ctx.playerData.wishCrouch) {
        //     SwitchState(factory.Charge());
        // } else 

        if (!ctx.playerData.grounded) {
            SwitchState(factory.Air());
        }
        else if (ctx.playerData.grappling) {
            SwitchState(factory.Grapple());
        }
    }

}