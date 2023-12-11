using UnityEngine;

public class PlayerStateNeutral : PlayerBaseState {

    public PlayerStateNeutral(PlayerCharacter currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
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

        if (ctx.playerData.grounded) {

            WalkMovementUpdate();

            // if (ctx.playerData.wishDashUp) {
            //     Dash();
            // }

        } else {
            
            SkateMovementUpdate(1f);

        }

        

        

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
        if (ctx.playerData.wishDashPress && ctx.playerData.grounded) {
            SwitchState(factory.Dash());
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