using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering; // 45w 28h
using UnityEngine.UI;
using System;

using Game.StateMachine;
using Game.Data;

namespace Game.Controllers {
    
    public class SkateCharacterController : BaseCharacterController
    {

        

        

        // public virtual void HandleDamage(PlayerCharacter target, float damageAmount){}
        // public virtual void HandleKnockback(PlayerCharacter target, Vector3 direction, float knockbackAmount){}
        // public virtual void HandleStagger(PlayerCharacter target, float duration){}

        [HideInInspector] public InputController inputController;
        [HideInInspector] public VFXController vfxController;
        [HideInInspector] public CameraController cameraController;
        [HideInInspector] public TimerController timerController;
        [HideInInspector] public AnimationController animationController;
        [HideInInspector] public CollisionHandler collisionHandler;
        [HideInInspector] public TargettingHandler targettingHandler;
    
        // public Transform rightHandEffector;
        
    
        public Material cloakMat;
        [HideInInspector] public Vector3 preUpdateEnvironmentForces;
        [HideInInspector] public Vector3 squash;
        public Text debug;
        [HideInInspector] public BezierCurve bezierCurve;
        // [HideInInspector] public Quaternion bodyRotation { get { return transform.rotation; } set { transform.rotation = value; } }
        protected Vector3 prevPosition;
        [HideInInspector] public bool doubleJump = false;
        [HideInInspector] public GameObject slashObj;
        [HideInInspector] public float focusAimBlend;
        public GameObject speedBall;
        public Slider energySlider;
        protected AnimationCurve curve;
        protected Vector3 toTarget = Vector3.zero;
        public Material speedBallMaterial;
        public bool isTrailActive;
        [HideInInspector] public float meshRefreshRate = 0.1f;
        [HideInInspector] public float activeTime = 2f;


    
        protected void Awake() {

    
            
            // animationController.animator = transform.GetChild(0).GetComponent<Animator>();
            slashObj = transform.GetChild(1).transform.GetChild(1).gameObject;
            
    
            // energySlider = energyObj.GetComponent<Slider>();
            energySlider.value = .25f;
    
            // characterData.moveConfig.grappleColor = characterData.moveConfig.normalColor;
    
            // characterData.moveData.origin = transform.position;
            // prevPosition = transform.position;
    
            speedBall.transform.localScale = new Vector3(2f, 2f, 2f);

            characterData = GetComponent<CharacterData>();
            characterData.playerData = GetComponent<PlayerData>();
            characterData.moveData = GetComponent<MoveData>();
            characterData.moveConfig = GetComponent<MoveConfig>();

            timerController = GetComponent<TimerController>();
            vfxController = GetComponent<VFXController>();
            animationController = GetComponent<AnimationController>();
            cameraController = GetComponent<CameraController>();
            collisionHandler = GetComponent<CollisionHandler>();
            targettingHandler = GetComponent<TargettingHandler>();

    
            characterData.playerControls = new PlayerControls();
    
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
    
            // viewTransformLookAt = cameraController.camera.transform.forward;
            
            focusAimBlend = .5f;
    
            characterData.playerData.detectedTargets = new List<Collider>();
    
        }
    
        

        private void Update () {

            // debug.text = String.Format("Aim Assist Blend: {0}\nCurrent Super State: {1}\nCurrent Sub State: {2}\nSpeed: {3}\nTargets: {4}", focusAimBlend, currentState?.name, currentState?.currentSubState?.name, Mathf.Floor(characterData.moveData.velocity.magnitude), targettingHandler.targetLength);
            squash = Vector3.zero;

            // Check Ground For State >> Update position >> Resolve Collisions >> Update Next State >> Update Next Rotation

            timerController.DecrementTimers();
            targettingHandler.FindTargets();
            collisionHandler.ResolveCollisions();

            Vector3 positionalMovement = transform.position - prevPosition; // TODO: 
            transform.position = prevPosition;
            characterData.moveData.origin += positionalMovement;

            ClampVelocity();
            // animationController.DoBlendAnimations();
            CheckGrapplePress();
            cameraController.CameraSettings();

            // Dive should be renamed to Skate
            
            currentState.UpdateStates();

            float x, y, z;

            x = Mathf.Abs(characterData.moveData.velocity.normalized.x) * 2f;
            y = Mathf.Abs(characterData.moveData.velocity.normalized.y) * 2f;
            z = 2f + characterData.moveData.velocity.magnitude / 5f;

            if (preUpdateEnvironmentForces != Vector3.zero) {
                characterData.moveData.velocity += preUpdateEnvironmentForces;
                // squash += -preUpdateEnvironmentForces;
                preUpdateEnvironmentForces = Vector3.zero;
            }

            transform.position = characterData.moveData.origin;
            prevPosition = transform.position;

            characterData.moveData.flatVelocity = Vector3.ProjectOnPlane(characterData.moveData.velocity, Vector3.up);

            // if (playerData.wishJumpUp) {
            //     StartCoroutine(ActivateTrail(2f));
            // }

            // rightHandEffector.right = avatarLookForward;

            // if (rightHandEffector.eulerAngles.x < 0f) {
            //     rightHandEffector.eulerAngles = Vector3.Scale(rightHandEffector.eulerAngles, new Vector3(-1f, 1f, 1f));
            // }

            // rightHandEffector.position = transform.position + avatarLookRight * .5f;

            targettingHandler.AimAssist();

            TransformRotation();

            // adjustment.z = playerInput.z * speed - Vector3.Dot(relativeVelocity, zAxis);
            // velocity += xAxis * adjustment.x + zAxis * adjustment.z;

            characterData.zVel = Vector3.Dot(characterData.moveData.velocity, characterData.bodyForward);
            characterData.yVel = Vector3.Dot(characterData.moveData.velocity, characterData.bodyUp);

            Vector3 speedBallSquash = Vector3.zero;

            if (characterData.moveData.velocity != Vector3.zero) speedBall.transform.forward = characterData.moveData.velocity.normalized;
    
            speedBallSquash = speedBall.transform.InverseTransformVector(squash); // squash direction in local

            // if (speedBallSquash != Vector3.zero) Debug.Log(speedBallSquash);

            squash = Vector3.Lerp(squash, Vector3.zero, Time.deltaTime / 8f);

            float squashY = Vector3.Dot(speedBallSquash, Vector3.up) / 2f;

            speedBall.transform.localScale = Vector3.Lerp(speedBall.transform.localScale, new Vector3(2f, 2f - squashY, z), Time.deltaTime * 8f);

            characterData.playerData.wishJumpUp = false;
            characterData.playerData.wishJumpPress = false;
            characterData.playerData.wishRunUp = false;
            characterData.playerData.wishTumbleUp = false;
            characterData.playerData.wishFireUp = false;
            characterData.playerData.wishFirePress = false;
            characterData.playerData.wishGrappleUp = false;
            characterData.playerData.wishGrapplePress = false;
            characterData.playerData.wishAimPress = false;
            characterData.playerData.wishAimUp = false;
            characterData.playerData.wishDashPress = false;
            characterData.playerData.wishDashUp = false;
            characterData.playerData.detectWall = false;
            characterData.playerData.wallNormal = Vector3.zero;

            if (characterData.playerData.wishEscapeDown) {
                Application.Quit();
            }

            focusAimBlend = Mathf.Lerp(focusAimBlend, .5f, Time.deltaTime * 2f);


        }

        private void Start() {
            base.Start();

            currentState = new PlayerStateAir(this, new PlayerStateFactory(this));
            // currentState.OnRequestInstantiate += HandleInstantiation;
            currentState.InitializeSubStates();
            // bezierCurve = new BezierCurve(this);

            characterData.moveData.origin = transform.position;
            prevPosition = transform.position;


        }

        public void HandleInstantiation()
        {
            if (vfxController._fireball != null)
            {
                vfxController.fireball = Instantiate(vfxController._fireball, characterData.avatarLookTransform.position + characterData.avatarLookForward*2f, Quaternion.identity);
                vfxController.fireball.gameObject.SetActive(true);
                vfxController.fireball.transform.forward = characterData.viewForward;
                vfxController.fireball.GetComponent<MoveData>().velocity = characterData.viewForward * 10f;
    
                Debug.Log("fire");
    
                StartCoroutine(FireballRoutine(characterData.playerData.mainTarget));
            }
        }
    
        public void TriggerThing() {
            if (vfxController._fireball != null)
            {
                vfxController.fireball = Instantiate(vfxController._fireball, characterData.avatarLookTransform.position + characterData.avatarLookForward*2f, Quaternion.identity);
                vfxController.fireball.gameObject.SetActive(true);
                vfxController.fireball.transform.forward = characterData.viewForward;
    
                Debug.Log("fire");
    
                StartCoroutine(FireballRoutine(characterData.playerData.mainTarget));
            }
        }


        public void TransformRotation() { // must handle here because both controllers used

                // AimAssist();

                // characterData.focusOnThis.position = Vector3.Lerp(characterData.focusOnThis.position, characterData.playerData.focusPoint, Time.deltaTime * 10f);
                
                Vector3 combinedLookPosition = characterData.lookAtThis.position;
                Quaternion combinedLookRotation = Quaternion.LookRotation((combinedLookPosition - characterData.avatarLookTransform.position).normalized, Vector3.up);

                if (characterData.moveData.velocity.magnitude > characterData.moveConfig.walkSpeed) { // TODO: make bodyTransform not this transform
                    characterData.avatarLookRotation = Quaternion.Slerp(characterData.avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
                    // characterData.avatarLookRotation = combinedLookRotation;
                    // characterData.bodyRotation = Quaternion.Slerp(characterData.bodyRotation, FlatLookRotation(characterData.viewForward), Time.deltaTime * 5f);
                    characterData.velocityRotation = Quaternion.LookRotation(characterData.moveData.velocity);
                } else {
                    characterData.avatarLookRotation = Quaternion.Slerp(characterData.avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
                    // characterData.avatarLookRotation = combinedLookRotation;
                    if (characterData.firstPersonCam.Priority == 1) {
                        // characterData.avatarLookRotation = combinedLookRotation;
                        // characterData.bodyRotation = FlatLookRotation(characterData.avatarLookForward);
                    } else {

                        // characterData.bodyRotation = Quaternion.Slerp(characterData.bodyRotation, FlatLookRotation(characterData.viewForward), Time.deltaTime * 5f);
                        
                    }
                    characterData.velocityRotation = characterData.bodyRotation;
                }

                float distance = (characterData.moveData.velocity * Time.deltaTime).magnitude;
                float angle1 = distance * (180f / Mathf.PI);
                float ballAlignSpeed = 180f;

                Quaternion AlignBallRotation (Vector3 rotationAxis, Quaternion rotation) {
                    Vector3 ballAxis = characterData.bodyUp;
                    float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
                    float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                    float maxAngle = ballAlignSpeed * Time.deltaTime;

                    Quaternion newAlignment =
                        Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
                    if (angle <= maxAngle) {
                        return newAlignment;
                    }
                    else {
                        return Quaternion.SlerpUnclamped(
                            rotation, newAlignment, maxAngle / angle
                        );
                    }
                }

                Vector3 rotationAxis = Vector3.Cross(Vector3.up, characterData.moveData.velocity.normalized);

                if (characterData.playerData.detectWall) {

                    if (characterData.playerData.wallNormal != Vector3.zero) {
                        rotationAxis = Vector3.Cross(characterData.playerData.wallNormal, characterData.moveData.velocity.normalized);
                    }
                }

                // characterData.transform.localRotation = 
                // if (Vector3.Dot(characterData.moveData.velocity.normalized, characterData.avatarLookForward) > .5f) {
                    characterData.bodyRotation = Quaternion.Euler(rotationAxis * angle1) * characterData.bodyRotation;
                    characterData.bodyRotation = AlignBallRotation(rotationAxis, characterData.bodyRotation);

                // } else if (Vector3.Dot(characterData.moveData.velocity.normalized, characterData.avatarLookForward) < -.5f) {
                //     characterData.bodyRotation = Quaternion.Euler(-rotationAxis * angle1) * characterData.bodyRotation;
                //     characterData.bodyRotation = AlignBallRotation(rotationAxis, characterData.bodyRotation);

                // }

                // TODO: think about inverting velocity movement

                // 28 46

                // Vector2 differenceInPixels = cameraController.camera.WorldToScreenPoint(characterData.lookAtThis.position) - cameraController.camera.WorldToScreenPoint(characterData.playerData.focusPoint);

                if (characterData.firstPersonCam.Priority == 1) {
                    characterData.playerData.xAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .5f, Time.deltaTime * 2f);
                    characterData.playerData.yAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .5f, Time.deltaTime * 2f);

                    

                } else {
                    characterData.playerData.xAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .9f, Time.deltaTime * 2f);
                    characterData.playerData.yAimDamp = Mathf.Lerp(characterData.playerData.yAimDamp, .9f, Time.deltaTime * 2f);

                }

                characterData.xMouseMovement = Mathf.Clamp(characterData.playerData.mouseDelta.x * characterData.moveConfig.horizontalSensitivity * characterData.moveConfig.sensitivityMultiplier * characterData.playerData.xAimDamp, -2.5f, 2.5f);
                characterData.yMouseMovement = Mathf.Clamp(-characterData.playerData.mouseDelta.y * characterData.moveConfig.verticalSensitivity  * characterData.moveConfig.sensitivityMultiplier * characterData.playerData.yAimDamp, -2.5f, 2.5f);

                // aim influence / virtual mouse

                // if (!(cameraController.camera.WorldToViewportPoint(playerData.mainTarget.position).z < 0f)) {

                //     Vector3 toReticle = reticle.anchoredPosition.normalized;

                // }
                
                if (characterData.playerData.wishFireDown && characterData.playerData.mainTarget.position != Vector3.zero) { // hard lock on

                    toTarget = targettingHandler.reticle.anchoredPosition.normalized;

                    characterData.xMouseMovement += toTarget.x;
                    characterData.yMouseMovement += toTarget.y;

                    characterData.xMouseMovement = Mathf.Clamp(characterData.xMouseMovement, -2f, 2f);
                    characterData.yMouseMovement = Mathf.Clamp(characterData.yMouseMovement, -2f, 2f);
                    
                    characterData.viewTransformLook.x = Mathf.Clamp(characterData.viewTransformLook.x + characterData.yMouseMovement, characterData.moveConfig.minYRotation, characterData.moveConfig.maxYRotation);
                    characterData.viewTransformLook.y = characterData.viewTransformLook.y + characterData.xMouseMovement;

                    Vector3 influence = characterData.avatarLookRotation * new Vector3(Mathf.Clamp(characterData.xMouseMovement, -5f, 5f), Mathf.Clamp(-characterData.yMouseMovement, -5f, 5f), 0f);

                    
                    characterData.viewRotation = 
                        Quaternion.AngleAxis(characterData.viewTransformLook.y, Vector3.up) *
                        Quaternion.AngleAxis(characterData.viewTransformLook.z, Vector3.forward) *
                        Quaternion.AngleAxis(characterData.viewTransformLook.x, Vector3.right);

                    Vector3 vanishingPoint = characterData.cam.transform.position + characterData.cam.transform.forward * 20f;

                    characterData.lookAtThis.position += (characterData.playerData.mainTarget.position - characterData.lookAtThis.position + influence) * Time.deltaTime * 5f;

                } else {

                    characterData.viewTransformLook.x = Mathf.Clamp(characterData.viewTransformLook.x + characterData.yMouseMovement, characterData.moveConfig.minYRotation, characterData.moveConfig.maxYRotation);
                    characterData.viewTransformLook.y = characterData.viewTransformLook.y + characterData.xMouseMovement;

                    characterData.viewRotation = 
                        Quaternion.AngleAxis(characterData.viewTransformLook.y, Vector3.up) *
                        Quaternion.AngleAxis(characterData.viewTransformLook.z, Vector3.forward) *
                        Quaternion.AngleAxis(characterData.viewTransformLook.x, Vector3.right);

                    Vector3 vanishingPoint = characterData.avatarLookTransform.position + characterData.viewForward* 20f;

                    characterData.lookAtThis.position = vanishingPoint;

                }

                // _vcam.SetActive(true);

                if (characterData.playerData.wishAimDown) {
                    characterData.firstPersonCam.Priority = 1;
                    // virtualFramingCam.Priority = 0;
                    characterData.thirdPersonCam.Priority = 0;

                    vfxController.dither = Mathf.Lerp(vfxController.dither, 1f, Time.deltaTime * 4f);

                    for (int i = 0; i < characterData.characterMaterials.Length; i++) 
                    {
                        characterData.characterMaterials[i].SetFloat("_dither", vfxController.dither);
                    }

                    Color color = cloakMat.color;
                    color.a = 0f;
                    cloakMat.color = Color.Lerp(cloakMat.color, color, Time.deltaTime * 2f);

                    // cameraController.camera.cullingMask &= ~(1 << LayerMask.NameToLayer("FirstPersonCull"));


                } else {
                    characterData.firstPersonCam.Priority = 0;
                    // virtualFramingCam.Priority = 1;
                    characterData.thirdPersonCam.Priority = 1;

                    vfxController.dither = Mathf.Lerp(vfxController.dither, 0f, Time.deltaTime * 4f);

                    for (int i = 0; i < characterData.characterMaterials.Length; i++) 
                    {
                        characterData.characterMaterials[i].SetFloat("_dither", vfxController.dither);
                    }

                    Color color = cloakMat.color;
                    color.a = 1f;
                    cloakMat.color = Color.Lerp(cloakMat.color, color, Time.deltaTime * 2f);

                    // cameraController.camera.cullingMask |= (1 << LayerMask.NameToLayer("FirstPersonCull"));
                }
                
            }
    
        IEnumerator<SkateCharacterController> FireballRoutine(Transform target) {
    
            Vector3 v = vfxController.fireball.GetComponent<MoveData>().velocity;
            vfxController.fireball.GetComponent<MoveData>().velocity = characterData.avatarLookForward * 10f;
    
            float timer = 0f;
            while (timer < 2f)
            {
                // transform.position += movementPerSecond * Time.deltaTime;
                Vector3 toTarget = target.position - vfxController.fireball.transform.position;
                vfxController.fireball.GetComponent<MoveData>().velocity = Vector3.Slerp(vfxController.fireball.GetComponent<MoveData>().velocity, toTarget, Time.deltaTime * 2f);
                // vfxController.fireball.transform.position += v * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }
    
            Destroy(vfxController.fireball);
    
        }

        private void ShootGrapple(float distance) {
            Ray ray = new Ray(characterData.avatarLookTransform.position + characterData.avatarLookForward * characterData.moveConfig.castRadius * 2f, characterData.avatarLookForward);
            RaycastHit hit;

            if (Physics.SphereCast(ray, characterData.moveConfig.castRadius, out hit, distance, LayerMask.GetMask (new string[] { "Ground" })))
                ConnectGrapple(hit.point); // TODO: Startup animation
                
            characterData.playerData.grappleNormal = hit.normal;
        }

        private void CheckGrapplePress() {

            if (characterData.playerData.wishGrapplePress && !characterData.playerData.grappling) {
                ConnectGrapple(characterData.playerData.focusPoint);
                
            }


        }

        

        
    
        IEnumerator<SkateCharacterController> GrappleRoutine(Transform target) {
    
            Vector3 v = vfxController.fireball.GetComponent<MoveData>().velocity;
            vfxController.fireball.GetComponent<MoveData>().velocity = characterData.avatarLookForward * 10f;
    
            float timer = 0f;
            while (timer < 2f)
            {
                // transform.position += movementPerSecond * Time.deltaTime;
                Vector3 toTarget = target.position - vfxController.fireball.transform.position;
                vfxController.fireball.GetComponent<MoveData>().velocity = Vector3.Slerp(vfxController.fireball.GetComponent<MoveData>().velocity, toTarget, Time.deltaTime * 2f);
                // vfxController.fireball.transform.position += v * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }
    
            Destroy(vfxController.fireball);
    
        }

        public void CastFireball() {


            vfxController.fireball = Instantiate(vfxController._fireball, characterData.avatarLookTransform.position + characterData.avatarLookForward*2f, Quaternion.identity);

        //     }

        //     grapple.playerCollider = innerCollider;
        //     Physics.IgnoreCollision(grapple.gameObject.GetComponent<SphereCollider>(), grapple.playerCollider, true);
                
            vfxController.fireball.gameObject.SetActive(true);

            float launchSpeed = 10f;

        //     // Debug.Log(Vector3.Dot(moveData.velocity, viewForward) * viewForward);
            
            vfxController.fireball.transform.forward = characterData.viewForward;

            // StartCoroutine(FireballRoutine(characterData.playerData.mainTarget));

        }
    
        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            if (currentState != null)
            {
                currentState.OnRequestInstantiate -= HandleInstantiation;
            }
        }
    
        // protected void OnEnable() {
        //     PlayerControls.Enable();
    
        //     EventManager.OnKnockbackReceived += HandleKnockback;
        //     EventManager.OnDamageReceived += HandleDamage;
        //     EventManager.OnStaggerReceived += HandleStagger;
        // }
    
        // protected void OnDisable() {
        //     PlayerControls.Disable();
    
        //     EventManager.OnKnockbackReceived -= HandleKnockback;
        //     EventManager.OnDamageReceived -= HandleDamage;
        //     EventManager.OnStaggerReceived -= HandleStagger;
        // }
    
    
        public Quaternion FlatLookRotation(Vector3 forward) {
            return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, Vector3.up).normalized, Vector3.up);
        }
    
        public Quaternion FlatLookRotation(Vector3 forward, Vector3 normal) {
            return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, normal).normalized, normal);
        }
    

        protected void ConnectGrapple(Vector3 grapplePosition) {
    
            if (Vector3.Distance(characterData.moveData.origin, grapplePosition) < characterData.moveConfig.minDistance && energySlider.value < .1f) {
                return;
            }
    
            characterData.playerData.grapplePoint = grapplePosition;
            characterData.playerData.joint = gameObject.AddComponent<SpringJoint>();
            characterData.playerData.joint.autoConfigureConnectedAnchor = false;
            characterData.playerData.joint.connectedAnchor = characterData.playerData.grapplePoint;
    
            characterData.playerData.distanceFromGrapple = Vector3.Distance(characterData.moveData.origin, characterData.playerData.grapplePoint);
            characterData.playerData.grappling = true;
    
        }
    
        public void StopGrapple() {
            characterData.playerData.grapplePoint = characterData.focusOnThis.position;
            Destroy(characterData.playerData.joint);
    
            // bezierCurve.Clear();
            vfxController.grappleArc.enabled = false;
            
        }
    
        public void DrawRope() {
    
            if (!characterData.playerData.grappling) return;
    
            var _lr = vfxController.grappleGun.GetComponent<LineRenderer>();
    
            _lr.positionCount = 2;
    
            _lr.useWorldSpace = true;
    
            _lr.SetPosition(0, vfxController.grappleGun.transform.position);
            _lr.SetPosition(1, characterData.playerData.focusPoint);
    
            _lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);
            
            vfxController.grappleArc.enabled = true;
        }
    
        public void EraseRope() {
            vfxController.grappleArc.SetVector3("Pos0", vfxController.grappleGun.transform.position);
            vfxController.grappleArc.SetVector3("Pos1", vfxController.grappleGun.transform.position);
            vfxController.grappleArc.SetVector3("Pos2", vfxController.grappleGun.transform.position);
            vfxController.grappleArc.SetVector3("Pos3", vfxController.grappleGun.transform.position);
            vfxController.grappleArc.enabled = false;
    
        }
        
    
        protected void ClampVelocity() {
    
            float tmp = characterData.moveData.velocity.y;
            characterData.moveData.velocity.y = 0f;
    
            characterData.moveData.velocity = Vector3.ClampMagnitude(characterData.moveData.velocity, characterData.moveConfig.maxVelocity);
            characterData.moveData.velocity.y = Mathf.Max(tmp, -characterData.moveConfig.terminalVelocity);
            characterData.moveData.velocity.y = Mathf.Min(tmp, characterData.moveConfig.terminalVelocity);
    
        }
    
        
    
    }

    
}
