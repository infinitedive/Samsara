using UnityEngine;

public abstract class PlayerBaseState
{
    protected bool _isRootState = false;
    protected bool _isTransitionState = false;
    protected PlayerCharacter _ctx;
    protected PlayerCharacter ctx { get { return _ctx; } set { _ctx = value; } }
    protected PlayerStateFactory _factory;
    protected PlayerStateFactory factory { get { return _factory; } set { _factory = value; } }
    protected PlayerBaseState _currentSubState;
    public PlayerBaseState currentSubState { get { return _currentSubState; } }
    protected PlayerBaseState _currentSuperState;
    public PlayerBaseState currentSuperState { get { return _currentSuperState; } }
    public string name = "";
    protected Vector3 releaseVelocity = Vector3.zero;
    protected Vector3[] releasedPoints = new Vector3[4];
    public float time = 0f;
    
    protected Quaternion avatarLookFlat;
    protected Quaternion avatarLookFlatSide;
    protected Vector3 flatForward = Vector3.zero;
    protected Vector3 flatForwardSide = Vector3.zero;

    public PlayerBaseState(PlayerCharacter currentContext, PlayerStateFactory playerStateFactory) {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubStates();

    public void UpdateStates() {

        flatForward = avatarLookFlat * Vector3.forward;
        flatForwardSide = avatarLookFlatSide * Vector3.forward;

        if (_isRootState) MoveInput();

        UpdateState();
        if (_currentSubState != null) {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState) {

        ExitState();

        // Debug.Log("switch " + name);
        // Debug.Log(ctx.moveData.velocity);

        newState.EnterState();

        if (_isRootState) {
            _ctx.currentState = newState;
        } else if (_currentSuperState != null) {
            _currentSuperState.SetSubState(newState);
        }

    }

    protected void SetSuperState(PlayerBaseState newSuperState) {
        
        _currentSuperState = newSuperState;
    }

    protected void SwitchSuperState(PlayerBaseState newSuperState) {
        _currentSuperState.ExitState();
        newSuperState.EnterState();
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) {

        // Debug.Log("set " + newSubState.name);
        // Debug.Log(ctx.moveData.velocity);

        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    private void MoveInput() {
        float forwardMove = ctx.playerData.verticalAxis;
        float rightMove = ctx.playerData.horizontalAxis;

        Vector3 wishDir = (forwardMove * Vector3.forward + rightMove * Vector3.right).normalized;

        if (ctx.playerData.hovering) {
            wishDir = (forwardMove * Vector3.up + rightMove * Vector3.right).normalized;
        }

        avatarLookFlat = ctx.FlatLookRotation(ctx.avatarLookForward);
        ctx.playerData.wishMove = ctx.avatarLookRotation * wishDir;
        ctx.playerData.wishMove = avatarLookFlat * wishDir;
    }

    protected void Jump() {

        ctx.jumpTimer = Time.deltaTime;
        ctx.ignoreGravityTimer = Time.deltaTime;

        float forceJump = ctx.moveConfig.jumpForce; // 10 force vs 20 gravity = 3 units high, 1 second long
        ctx.moveData.velocity += Vector3.up * forceJump;

    }

    protected void BoostJump(Vector3 wishDir, float customMagnitude = 0f) {

        if (ctx.boostInputTimer > 0f || ctx.energySlider.value < .25f) return;

        // ctx.boostInputTimer = Time.deltaTime * 2f;
        // ctx.jumpTimer = Time.deltaTime * 2f;
        ctx.ignoreGravityTimer = Time.deltaTime * 2f;

        ctx.airHike.SetVector3("origin", ctx.moveData.origin);
        ctx.airHike.SetVector3("lookAt", ctx.groundNormal);
        ctx.airHike.SetFloat("size", 4f);
        ctx.airHike.Play();

        ctx.sonicBoom.Play();

        float relativePower = Mathf.Clamp((ctx.moveData.velocity.magnitude + 20f)/ctx.moveData.velocity.magnitude, -20f, 20f);

        float forceJump = ctx.moveConfig.jumpForce + relativePower;
        // if (customMagnitude > 0f) {
        //     ImpulseCancelVelocityAgainst(wishDir);
        //     ctx.moveData.velocity = wishDir * customMagnitude + Vector3.Dot(ctx.moveData.velocity.normalized, wishDir) * ctx.moveData.velocity.magnitude * wishDir;
        // } else {
            // ImpulseCancelVelocityAgainst(wishDir);
            // ctx.moveData.velocity = wishDir * forceJump + Mathf.Clamp01(Vector3.Dot(ctx.moveData.velocity.normalized, wishDir)) * Vector3.Dot(ctx.moveData.velocity, wishDir) * wishDir;
        // }

        ctx.playerData.vCharge = 0f;
        ctx.energySlider.value -= .25f;
    }

    protected void Dash() {

        SetVelocity(ctx.playerData.wishMove * 15f);

        
    }

    protected void OnlyInfluence() {

        if (ctx.jumpTimer > 0f) return;

        Vector3 neutralMove = avatarLookFlat * ctx.playerData.input * ctx.moveConfig.walkSpeed;
        float power = ctx.playerData.grounded ? Mathf.Pow(3f, .5f) : Mathf.Pow(3f, .5f) / 2f;

        if (!ctx.playerData.grounded) neutralMove = avatarLookFlat * ctx.playerData.input * ctx.moveConfig.walkSpeed / 2f;

        // oldMomentum = Vector3.Lerp(oldMomentum, Vector3.zero, Time.deltaTime);

        if (Vector3.Scale(ctx.moveData.velocity, new Vector3(1f, 0f, 1f)).magnitude > ctx.moveConfig.walkSpeed) {
            // if (ctx.playerData.grounded) SubtractVelocityAgainst(ctx.moveData.velocity.normalized, Mathf.Max(oldMomentum.magnitude / 4f, 5f));
            // SkateMovementUpdate(Mathf.Pow(3f, .5f));
            // oldMomentum = ctx.moveData.velocity;
        } else {
            
            // var yVel = ctx.moveData.velocity.y;
            // ctx.moveData.velocity.y = 0f;
            // ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, Vector3.ClampMagnitude(neutralMove + oldMomentum, ctx.moveConfig.walkSpeed), Time.deltaTime * 8f);
            // ctx.moveData.velocity.y = yVel;
        }
        
    }

    protected void WalkMovementUpdate() {

        Vector3 movement = ctx.playerData.wishMove * ctx.moveConfig.walkSpeed;

        if (ctx.playerData.wishRunDown) {
            movement = ctx.playerData.wishMove * ctx.moveConfig.runSpeed;
        }

        AccelerateTo(movement, 8f);

    }

    protected void SkateMovementUpdate(float power) {

        Vector3 influence = ctx.playerData.wishMove * ctx.moveData.velocity.magnitude * power;
        
        float influenceOrthagonalToVelocity = Vector3.Dot(influence, ctx.velocityRight);
        Vector3 angularAcceleration = influenceOrthagonalToVelocity * ctx.velocityRight;

        float influenceOppositeToVelocity = Mathf.Clamp(Vector3.Dot(influence, -ctx.velocityForward), 0f, influence.magnitude / 4f);
        Vector3 deceleration = influenceOppositeToVelocity * -ctx.velocityForward;

        ctx.moveData.velocity += angularAcceleration * (Time.deltaTime);
        
    }

    protected void FlyMovementUpdate(ref Vector3 influencedV) {
        
        Vector3 influence = ctx.playerData.wishMove * ctx.moveData.velocity.magnitude * Mathf.Pow(3f, .5f);

        float influenceOrthagonalToVelocityRight = Vector3.Dot(influence, ctx.velocityRight);
        Vector3 angularAccelerationY = influenceOrthagonalToVelocityRight * ctx.velocityRight;

        float influenceOrthagonalToVelocityUp = Vector3.Dot(influence, ctx.velocityUp);
        Vector3 angularAccelerationX = influenceOrthagonalToVelocityUp * ctx.velocityUp;

        float influenceOppositeToVelocity = Mathf.Clamp(Vector3.Dot(influence, -ctx.velocityForward), 0f, influence.magnitude / 4f);
        Vector3 deceleration = influenceOppositeToVelocity * -ctx.velocityForward;


        float lookingRightY = Vector3.Dot(influence.normalized, ctx.velocityRight);
        float lookingBackY = Vector3.Dot(influence.normalized, -ctx.velocityForward);

        influencedV += angularAccelerationY * (Time.deltaTime) + angularAccelerationX * (Time.deltaTime);
        
    }

    public void ImpulseCancelVelocityAgainst(Vector3 wishDir) {

        if (Vector3.Dot(ctx.moveData.velocity, -wishDir.normalized) > 0f) {
            ctx.moveData.velocity += Vector3.Dot(ctx.moveData.velocity, -wishDir.normalized) * wishDir;
        }


    }

    public Vector3 ImpulseCancelVelocityAgainst(Vector3 wishDir, Vector3 influencedV) {

        if (Vector3.Dot(ctx.moveData.velocity, -wishDir.normalized) > 0f) {

            return influencedV + Vector3.Dot(influencedV, -wishDir.normalized) * wishDir;

        }

        return influencedV;

    }

    public void SetVelocity(Vector3 _v){
        // ctx.moveData.velocity = _v;
    }

    public void OnlyAngularVelocity(Vector3 wishDir, float response) {

        Vector3 velocityOrthagonal = ctx.moveData.velocity + Vector3.Dot(ctx.moveData.velocity, -wishDir) * wishDir;

        ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, velocityOrthagonal, Time.deltaTime * response);

    }

    public void CancelVelocityAgainst(Vector3 wishDir, float response) {

        if (Vector3.Dot(ctx.moveData.velocity, -wishDir.normalized) > 0f) {

            ctx.moveData.velocity += Vector3.Dot(ctx.moveData.velocity, -wishDir) * wishDir * Time.deltaTime * response;

        }
        
    }

    protected void AccelerateTo(Vector3 targetVelocity, float delta) {

        Vector3 neutralMove = (avatarLookFlat * targetVelocity);

        float yVel = ctx.moveData.velocity.y;
        ctx.moveData.velocity = Vector3.Lerp(ctx.moveData.velocity, targetVelocity, Time.deltaTime * 8f);
        ctx.moveData.velocity.y = yVel;

    }

    public void OnlyAngularVelocityControl(float response) {

        float speed = ctx.moveData.velocity.magnitude;
        Vector3 result = Vector3.zero;
        Vector3 velocityRadial = Vector3.Dot(ctx.moveData.velocity, ctx.playerData.grappleDir) * ctx.playerData.grappleDir;
        
        Vector3 targetUp = Vector3.Cross(ctx.playerData.grappleDir, ctx.avatarLookForward).normalized;

        if (Vector3.Dot(ctx.moveData.velocity, ctx.playerData.grappleDir) < 0f) {
            velocityRadial = Vector3.Dot(ctx.moveData.velocity, ctx.playerData.grappleDir) * ctx.playerData.grappleDir;
        }

        Vector3 velocityOrthagonal = ctx.moveData.velocity - velocityRadial;

        Vector3 velocityOrthagonalUp = Vector3.Project(velocityOrthagonal, targetUp);
        Vector3 velocityOrthagonalRight = velocityOrthagonal - velocityOrthagonalUp;

        Vector3 velocityOrthagonalUpDampen = Vector3.Dot(velocityOrthagonal, -targetUp) * targetUp;

        result = (velocityOrthagonalRight + velocityOrthagonalUp + velocityOrthagonalUpDampen + velocityRadial).normalized * speed;
                
        ctx.moveData.velocity = Vector3.Slerp(ctx.moveData.velocity, result, Time.deltaTime * response);
        ctx.moveData.velocity = velocityOrthagonal;

    }

    protected void SubtractVelocityAgainst(Vector3 wishDir, float amount) {

        ctx.moveData.velocity += Vector3.Dot(ctx.moveData.velocity.normalized, -wishDir.normalized) * wishDir.normalized * amount * Time.deltaTime; 

    }

    protected void AddVelocityTowards(Vector3 wishDir, float amount) {
        ctx.moveData.velocity += Vector3.Dot(ctx.moveData.velocity.normalized, wishDir.normalized) * wishDir.normalized * amount * Time.deltaTime;  
    }

}
