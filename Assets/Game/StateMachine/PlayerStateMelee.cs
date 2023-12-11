using UnityEngine;


public class PlayerStateMelee : PlayerBaseState {

    float initialDistance = 0f;
    Vector3 angleDir = Vector3.zero;
    Vector3 angleForward = Vector3.zero;
    Vector3 angleTarget = Vector3.zero;


    public PlayerStateMelee(PlayerCharacter currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = false;
        name = "melee";
    }

    public override void EnterState()
    {
        Debug.Log("ENTER MELEE");
        // FindLookingAtTarget();
        time = 0f;
        // ctx.playerData.velocity = Vector3.zero;
        // ctx.playerData.attacking = false;
        
        
    }

    public override void UpdateState()
    {


        // IHittable ball = GameObject.Instantiate(ctx.ballObj, ctx.avatarLookTransform.position + ctx.avatarLookForward * 5f, ctx.avatarLookRotation).GetComponent<IHittable>();
        // ball.GetHit(ctx.avatarLookForward * 10f);


        if (ctx.currentTarget) {
            // if (ctx.currentTarget.TryGetComponent(out IHittable target)) {

                    
            //     string[] mask = new string[] { "Ball", "Enemy" };

            //     Collider[] hits = Physics.OverlapSphere(ctx.playerData.origin, 3f);

            //     foreach (Collider hit in hits) {
                    
            //         if (hit.TryGetComponent(out IHittable _ball)) {
            //             _ball.GetHit(angleForward * 15f);
            //             ctx.playerData.velocity = Vector3.zero;
            //         }

            //     }

            // Vector3 toTarget = (ctx.playerData.targetPoint - ctx.playerData.origin);

            // ctx.playerData.velocity = Vector3.Slerp(ctx.playerData.velocity, toTarget, Time.deltaTime * 2f);


                

            // }


                time += Mathf.Min(3f, Time.deltaTime);
            // }
        } else {
            ctx.playerData.attacking = false;
        }

        if (ctx.playerData.wishFireUp) {
            ctx.playerData.attacking = false;
            ctx.slashObj.SetActive(true);
            ctx.slash.Play();
        }

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        ctx.reduceGravityTimer = 0f;
 
    }

    public override void InitializeSubStates()
    {

    }

    public override void CheckSwitchStates()
    {

        if (ctx.playerData.grounded && !ctx.playerData.attacking) {
            SwitchState(_factory.Neutral());
        } else if (!ctx.playerData.grounded  && !ctx.playerData.attacking) {
            SwitchState(_factory.Fall());
        }
    }

    // private void LungeTargeting() {

    //     if (Vector3.Dot(ctx.avatarLookForward, ctx.playerData.targetDir) < .5f) {
    //         ctx.currentTarget = null;
    //         t = 1f;
    //         return;
    //     }

    //     ctx.bezierCurve.AttackArc(Vector3.ProjectOnPlane(ctx.playerData.targetNormal, Vector3.up).normalized, ctx.currentTarget.transform.position);

    // }

    // private void FindLookingAtTarget() {

    //     if (ctx.targetLength > 0) {
            
    //         Vector3 _targetPos = Vector3.zero;
    //         Vector3 _targetDir = Vector3.zero;

    //         foreach (Collider target in ctx.playerData.targets) {

    //             if (target == null) continue;

    //             _targetPos = target.transform.position;
    //             _targetDir = (target.transform.position - ctx.avatarLookTransform.position).normalized;

    //             if (Vector3.Dot(ctx.avatarLookForward, _targetDir) >= .5f) {
    //                 ctx.playerData.targetPoint = _targetPos;
    //                 ctx.playerData.targetDir = _targetDir;
    //                 ctx.playerData.distanceFromTarget = (ctx.playerData.targetPoint - ctx.avatarLookTransform.position).magnitude;
    //                 ctx.currentTarget = target;
    //             }
    //         }
            
    //     }

    //     if (ctx.playerData.targetDir == Vector3.zero) {
    //         ctx.currentTarget = null;
    //         ctx.playerData.targetPoint = Vector3.zero;
    //         ctx.playerData.targetDir = Vector3.zero;
    //         ctx.playerData.distanceFromTarget = 0f;
    //         ctx.playerData.targetNormal = Vector3.zero;
    //         return;
    //     }

    //     // Ray ray = new Ray(ctx.avatarLookTransform.position, ctx.playerData.targetDir);
    //     // RaycastHit hit;

    //     // if (Physics.SphereCast(ray, 1f, out hit, 300f, LayerMask.GetMask (new string[] { "Enemy" }))) {
    //     //     ctx.playerData.targetNormal = hit.normal;
    //     //     // ctx.bezierCurve = new BezierCurve(ctx);
    //     //     // ctx.bezierCurve.AttackArc(ctx.playerData.targetNormal, ctx.playerData.targetPoint);
    //     // } else {
    //     //     ctx.currentTarget = null;
    //     //     ctx.playerData.targetPoint = Vector3.zero;
    //     //     ctx.playerData.targetDir = Vector3.zero;
    //     //     ctx.playerData.distanceFromTarget = 0f;
    //     //     ctx.playerData.targetNormal = Vector3.zero;
    //     //     Debug.Log("miss");
    //     // }


    //     // Collision closestTarget = null;

    //     // foreach (Collision target in playerData.targets) {

    //     //     if (closestTarget == null) {
    //     //         closestTarget = target;
    //     //     } else {

    //     //         if (Vector3.Distance(target.transform.position, playerData.origin) > Vector3.Distance(closestTarget.transform.position, playerData.origin)) {
    //     //             closestTarget = target;
    //     //         }

    //     //     }

    //     // }

    // }


    

}