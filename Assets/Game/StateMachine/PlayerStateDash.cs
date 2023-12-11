using UnityEngine;

public class PlayerStateDash : PlayerBaseState {

    public PlayerStateDash(PlayerCharacter currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = false;
        name = "dash";
    }

    public override void EnterState()
    {
        time = 0f;

        // SetVelocity(ctx.playerData.wishMove.normalized * 25f);

    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // SkateMovementUpdate(Mathf.Pow(3f, .5f));
        // SubtractVelocityAgainst(ctx.moveData.velocity.normalized, ctx.moveData.velocity.magnitude * 2f);

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // +=1
    }

    public override void InitializeSubStates()
    {

    }

    public override void CheckSwitchStates()
    {
        if (time > .25f) {
            SwitchState(factory.Neutral());
        } else if (time > 0f && ctx.playerData.wishDashPress && ctx.playerData.grounded) {
            SwitchState(factory.Dash());
        }
    }


}