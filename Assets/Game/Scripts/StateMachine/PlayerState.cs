using UnityEngine;
using System;
using Game.Controllers;

namespace Game.StateMachine {

    /*
        We deny that in order to do something well we must first be willing to do it badly. 
        Instead, we opt for setting our limits at the point where we feel assured of success. 
        Living within these bounds, we may feel stifled, smothered, despairing, bored. But, yes, we do feel safe. 
        And safety is a very expensive illusion.
    */


    public abstract class PlayerState
    {
        protected bool _isRootState = false;
        protected bool _isLeafState = false;
        protected Controllers.CharacterController _ctx;
        protected Controllers.CharacterController ctx { get { return _ctx; } set { _ctx = value; } }
        protected PlayerStateFactory _factory;
        protected PlayerStateFactory factory { get { return _factory; } set { _factory = value; } }
        protected PlayerState _currentSubState;
        public PlayerState currentSubState { get { return _currentSubState; } }
        protected PlayerState _currentSuperState;
        public PlayerState currentSuperState { get { return _currentSuperState; } }
        public string name = "";
        protected Vector3 releaseVelocity = Vector3.zero;
        protected Vector3[] releasedPoints = new Vector3[4];
        public float time = 0f;
        protected Vector3 oldMomentum = Vector3.zero;
        
        protected Quaternion avatarLookFlat;
        protected Quaternion avatarLookFlatSide;
        protected Vector3 flatForward = Vector3.zero;
        protected Vector3 flatForwardSide = Vector3.zero;
        protected bool clutchFlag = true;

        public PlayerState(Controllers.CharacterController currentContext, PlayerStateFactory playerStateFactory) {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubStates();

        public event Action OnRequestInstantiate;

        public void TriggerInstantiate()
        {
            Debug.Log("trigger");
            OnRequestInstantiate?.Invoke();
        }

        public void UpdateStates() {

            flatForward = avatarLookFlat * Vector3.forward;
            flatForwardSide = avatarLookFlatSide * Vector3.forward;

            if (_isRootState) MoveInput();

            UpdateState();
            if (_currentSubState != null) {
                _currentSubState.UpdateStates();
            }
        }

        protected void SwitchState(PlayerState newState) {

            ExitState();

            newState.EnterState();

            if (_isRootState) {
                newState.SetSubState(currentSubState);
                _ctx.currentState = newState;
            } else if (_currentSuperState != null) {
                _currentSuperState.SetSubState(newState);
            }

        }

        protected void SetSuperState(PlayerState newSuperState) {
            
            _currentSuperState = newSuperState;
        }

        protected void SwitchSuperState(PlayerState newSuperState) {
            _currentSuperState.ExitState();
            newSuperState.EnterState();
            _currentSuperState = newSuperState;
        }

        protected void SetSubState(PlayerState newSubState) {

            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }

        private void MoveInput() {
            float forwardMove = ctx.characterData.playerData.verticalAxis;
            float rightMove = ctx.characterData.playerData.horizontalAxis;


            Vector3 wishDir = forwardMove * Vector3.forward + rightMove * Vector3.right;

            // if (ctx.characterData.playerData.hovering) {
            //     wishDir = (forwardMove * Vector3.up + rightMove * Vector3.right).normalized;
            // }

            avatarLookFlat = ctx.FlatLookRotation(ctx.characterData.avatarLookForward);
            // ctx.characterData.playerData.wishMove = ctx.characterData.avatarLookRotation * wishDir;
            ctx.characterData.playerData.wishMove = avatarLookFlat * wishDir;
        }

        protected void HandleGrapplePress() {

            // Debug.Log(ctx.characterData.playerData.wishGrapplePress);

            if (ctx.characterData.playerData.wishGrapplePress && !ctx.characterData.playerData.grappling) {
                ConnectGrapple(ctx.characterData.playerData.focusPoint, ctx.characterData.playerData.focusNormal);
            } else {
                DisconnectGrapple();
            }


        }

        private void ShootGrapple(float distance) {
            Ray ray = new Ray(ctx.characterData.avatarLookTransform.position + ctx.characterData.avatarLookForward * ctx.characterData.moveConfig.castRadius * 2f, ctx.characterData.avatarLookForward);
            RaycastHit hit;

            if (Physics.SphereCast(ray, ctx.characterData.moveConfig.castRadius, out hit, distance, LayerMask.GetMask (new string[] { "Ground" })))
                ConnectGrapple(hit.point, hit.normal); // TODO: Startup animation
                
            ctx.characterData.playerData.grappleNormal = hit.normal;
        }

        protected void DisconnectGrapple() {
            ctx.characterData.playerData.grapplePoint = Vector3.zero;
    
            // bezierCurve.Clear();
            ctx.vfxController.grappleArc.enabled = false;
        }

        protected void ConnectGrapple(Vector3 grapplePosition, Vector3 grappleNormal) {
    
            if (Vector3.Distance(ctx.characterData.moveData.origin, grapplePosition) < ctx.characterData.moveConfig.minDistance) {
                return;
            }
    
            ctx.characterData.playerData.grapplePoint = grapplePosition;
    
            ctx.characterData.playerData.distanceFromGrapple = Vector3.Distance(ctx.characterData.moveData.origin, ctx.characterData.playerData.grapplePoint);
            ctx.characterData.playerData.grappleNormal = grappleNormal;
    
        }


        protected void FloatUp() {

            ctx.characterData.moveData.velocity += Vector3.up * ctx.characterData.moveConfig.jumpForce * Time.deltaTime;

        }

        

        protected void OnlyInfluence() {

            if (ctx.timerController.jumpTimer > 0f) return;

            Vector3 neutralMove = avatarLookFlat * ctx.characterData.playerData.wishMove * ctx.characterData.moveConfig.walkSpeed;
            float power = ctx.characterData.playerData.grounded ? Mathf.Pow(3f, .5f) : Mathf.Pow(3f, .5f) / 2f;

            if (!ctx.characterData.playerData.grounded) neutralMove = avatarLookFlat * ctx.characterData.playerData.wishMove * ctx.characterData.moveConfig.walkSpeed / 2f;

            oldMomentum = Vector3.Lerp(oldMomentum, Vector3.zero, Time.deltaTime);

            if (Vector3.Scale(ctx.characterData.moveData.velocity, new Vector3(1f, 0f, 1f)).magnitude > ctx.characterData.moveConfig.walkSpeed) {
                if (ctx.characterData.playerData.grounded) SubtractVelocityAgainst(ctx.characterData.moveData.velocity.normalized, Mathf.Max(oldMomentum.magnitude / 4f, 5f));
                SkateMovementUpdate(Mathf.Pow(3f, .5f));
                oldMomentum = ctx.characterData.moveData.velocity;
            } else {
                
                var yVel = ctx.characterData.moveData.velocity.y;
                ctx.characterData.moveData.velocity.y = 0f;
                ctx.characterData.moveData.velocity = Vector3.Lerp(ctx.characterData.moveData.velocity, Vector3.ClampMagnitude(neutralMove + oldMomentum, ctx.characterData.moveConfig.walkSpeed), Time.deltaTime * 8f);
                ctx.characterData.moveData.velocity.y = yVel;
            }
            
        }

        protected void WalkMovementUpdate() {

            Vector3 movement = ctx.characterData.playerData.wishMove * ctx.characterData.moveConfig.runSpeed;

            if (ctx.characterData.playerData.grounded) {
                ctx.characterData.playerData.turnResponse = Mathf.Lerp(ctx.characterData.playerData.turnResponse, 8f, Time.deltaTime * 2f);
                LerpMovement(movement, ctx.characterData.playerData.turnResponse);
            } else {
                // ctx.characterData.playerData.turnResponse = Mathf.Lerp(ctx.characterData.playerData.turnResponse, 8f, Time.deltaTime * 2f);
            }


            
            
        }

        protected void SkateMovementUpdate(float power) {

            Vector3 influence = ctx.characterData.playerData.wishMove * ctx.characterData.moveData.velocity.magnitude * power;
            
            float influenceOrthagonalToVelocity = Vector3.Dot(influence, ctx.characterData.velocityRight);
            Vector3 angularAcceleration = influenceOrthagonalToVelocity * ctx.characterData.velocityRight;

            float influenceOppositeToVelocity = Mathf.Clamp(Vector3.Dot(influence, -ctx.characterData.velocityForward), 0f, influence.magnitude / 4f);
            Vector3 deceleration = influenceOppositeToVelocity * -ctx.characterData.velocityForward;

            ctx.characterData.moveData.velocity += angularAcceleration * (Time.deltaTime);
            
        }

        protected void FlyMovementUpdate() {
            
            Vector3 influence = ctx.characterData.playerData.wishMove * ctx.characterData.moveData.velocity.magnitude * Mathf.Pow(3f, .5f);

            float influenceOrthagonalToVelocityRight = Vector3.Dot(influence, ctx.characterData.velocityRight);
            Vector3 angularAccelerationY = influenceOrthagonalToVelocityRight * ctx.characterData.velocityRight;

            float influenceOrthagonalToVelocityUp = Vector3.Dot(influence, ctx.characterData.velocityUp);
            Vector3 angularAccelerationX = influenceOrthagonalToVelocityUp * ctx.characterData.velocityUp;

            float influenceOppositeToVelocity = Mathf.Clamp(Vector3.Dot(influence, -ctx.characterData.velocityForward), 0f, influence.magnitude / 4f);
            Vector3 deceleration = influenceOppositeToVelocity * -ctx.characterData.velocityForward;


            float lookingRightY = Vector3.Dot(influence.normalized, ctx.characterData.velocityRight);
            float lookingBackY = Vector3.Dot(influence.normalized, -ctx.characterData.velocityForward);

            float newSpeed = ctx.characterData.moveConfig.runSpeed - ctx.characterData.moveData.velocity.magnitude;

            ctx.characterData.moveData.velocity += angularAccelerationY * (Time.deltaTime) + angularAccelerationX * (Time.deltaTime);
            
        }

        public void ImpulseCancelVelocityAgainst(Vector3 wishDir) {

            if (Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir.normalized) > 0f) {
                ctx.characterData.moveData.velocity += Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir.normalized) * wishDir;
            }


        }

        public Vector3 ImpulseCancelVelocityAgainst(Vector3 wishDir, Vector3 influencedV) {

            if (Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir.normalized) > 0f) {

                return influencedV + Vector3.Dot(influencedV, -wishDir.normalized) * wishDir;

            }

            return influencedV;

        }

        public void SetVelocity(Vector3 _v){
            // ctx.characterData.moveData.velocity = _v;
        }

        public void OnlyAngularVelocity(Vector3 wishDir, float response) {

            Vector3 velocityOrthagonal = ctx.characterData.moveData.velocity + Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir) * wishDir;

            ctx.characterData.moveData.velocity = Vector3.Lerp(ctx.characterData.moveData.velocity, velocityOrthagonal, Time.deltaTime * response);

        }

        public void CancelVelocityAgainst(Vector3 wishDir, float response) {

            if (Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir.normalized) > 0f) {

                ctx.characterData.moveData.velocity += Vector3.Dot(ctx.characterData.moveData.velocity, -wishDir) * wishDir * Time.deltaTime * response;

            }
            
        }

        // We’ve all heard that the unexamined life is not worth living, but consider too that the unlived life is not worth examining.

        protected void SlerpMovement(Vector3 targetVelocity, float delta) {

            // if (ctx.characterData.moveData.velocity.magnitude > targetVelocity.magnitude) {

            //     float yVel = ctx.characterData.moveData.velocity.y;
            //     ctx.characterData.moveData.velocity = Vector3.Slerp(ctx.characterData.moveData.velocity, targetVelocity.normalized * ctx.characterData.moveData.velocity.magnitude, Time.deltaTime * delta);
            //     ctx.characterData.moveData.velocity.y = yVel;

            // } else {

            //     float yVel = ctx.characterData.moveData.velocity.y;
            //     ctx.characterData.moveData.velocity = Vector3.Slerp(ctx.characterData.moveData.velocity, targetVelocity, Time.deltaTime * delta);
            //     ctx.characterData.moveData.velocity.y = yVel;
            // }

            float yVel = ctx.characterData.moveData.velocity.y;
            ctx.characterData.moveData.velocity = Vector3.Slerp(ctx.characterData.moveData.velocity, targetVelocity, Time.deltaTime * delta);
            ctx.characterData.moveData.velocity.y = yVel;

        }

        protected void LerpMovement(Vector3 targetVelocity, float delta) {

            if (ctx.characterData.moveData.velocity.magnitude > targetVelocity.magnitude) {

                float yVel = ctx.characterData.moveData.velocity.y;
                ctx.characterData.moveData.velocity = Vector3.Lerp(ctx.characterData.moveData.velocity, targetVelocity.normalized * ctx.characterData.moveData.velocity.magnitude, Time.deltaTime * delta);
                ctx.characterData.moveData.velocity.y = yVel;

            } else {

                float yVel = ctx.characterData.moveData.velocity.y;
                ctx.characterData.moveData.velocity = Vector3.Lerp(ctx.characterData.moveData.velocity, targetVelocity, Time.deltaTime * delta);
                ctx.characterData.moveData.velocity.y = yVel;
            }

        }

        public void OnlyAngularVelocityControl(float response) {

            float speed = ctx.characterData.moveData.velocity.magnitude;
            Vector3 result = Vector3.zero;
            Vector3 velocityRadial = Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.playerData.grappleDir) * ctx.characterData.playerData.grappleDir;
            
            Vector3 targetUp = Vector3.Cross(ctx.characterData.playerData.grappleDir, ctx.characterData.avatarLookForward).normalized;

            if (Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.playerData.grappleDir) < 0f) {
                velocityRadial = Vector3.Dot(ctx.characterData.moveData.velocity, ctx.characterData.playerData.grappleDir) * ctx.characterData.playerData.grappleDir;
            }

            Vector3 velocityOrthagonal = ctx.characterData.moveData.velocity - velocityRadial;

            Vector3 velocityOrthagonalUp = Vector3.Project(velocityOrthagonal, targetUp);
            Vector3 velocityOrthagonalRight = velocityOrthagonal - velocityOrthagonalUp;

            Vector3 velocityOrthagonalUpDampen = Vector3.Dot(velocityOrthagonal, -targetUp) * targetUp;

            result = (velocityOrthagonalRight + velocityOrthagonalUp + velocityOrthagonalUpDampen + velocityRadial).normalized * speed;
                    
            ctx.characterData.moveData.velocity = Vector3.Slerp(ctx.characterData.moveData.velocity, result, Time.deltaTime * response);
            ctx.characterData.moveData.velocity = velocityOrthagonal;

        }

        protected void SubtractVelocityAgainst(Vector3 wishDir, float amount) {

            ctx.characterData.moveData.velocity += Vector3.Dot(ctx.characterData.moveData.velocity.normalized, -wishDir.normalized) * wishDir.normalized * amount * Time.deltaTime; 

        }

        protected void AddVelocityTowards(Vector3 wishDir, float amount) {
            ctx.characterData.moveData.velocity += Vector3.Dot(ctx.characterData.moveData.velocity.normalized, wishDir.normalized) * wishDir.normalized * amount * Time.deltaTime;  
        }

    }
}