using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering; // 45w 28h
using UnityEngine.UI;
using System;

using Game.StateMachine;
using Game.Data;

namespace Game.Controllers {
    
    public class CharacterController : BaseCharacterController
    {

        public event Action OnGrapple;

        [HideInInspector] public VFXController vfxController;
        [HideInInspector] public CameraController cameraController;
        [HideInInspector] public TimerController timerController;
        [HideInInspector] public AnimationController animationController;
        [HideInInspector] public CollisionController collisionController;
        [HideInInspector] public TargettingController targettingController;
    
        
        // Unorganized member variables

        public Material cloakMat;
        [HideInInspector] public Vector3 preUpdateEnvironmentForces;
        public Text debug;
        protected Vector3 prevPosition;
        [HideInInspector] public bool doubleJump = false;
        [HideInInspector] public GameObject slashObj;
        public GameObject speedBall;
        protected Vector3 toTarget = Vector3.zero;


    
        protected void Awake() {

            OnGrapple += HandleGrapple;
            
            slashObj = transform.GetChild(1).transform.GetChild(1).gameObject;
            
    
            speedBall.transform.localScale = new Vector3(2f, 2f, 2f);

            characterData = GetComponent<CharacterData>();
            characterData.playerData = GetComponent<PlayerData>();
            characterData.moveData = GetComponent<MoveData>();
            characterData.moveConfig = GetComponent<MoveConfig>();

            timerController = GetComponent<TimerController>();
            vfxController = GetComponent<VFXController>();
            animationController = GetComponent<AnimationController>();
            cameraController = GetComponent<CameraController>();
            collisionController = GetComponent<CollisionController>();
            targettingController = GetComponent<TargettingController>();

            characterData.playerControls = new PlayerControls();
    
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
                
    
            characterData.playerData.detectedTargets = new List<Collider>();
    
        }
    
        private void Start() {
            base.Start();

            currentState = new PlayerStateAir(this, new PlayerStateFactory(this));
            // currentState.OnRequestInstantiate += HandleInstantiation;
            currentState.InitializeSubStates();

            characterData.moveData.origin = transform.position;
            prevPosition = transform.position;


        }

        private void HandleGrapple() {

        }

        /*
        The goal is to connect to a world outside of us, to lose the obsessive self-focus of self-exploration and, simply, explore. 
        One quickly notes that when the mind is focused on other, the self often comes into a far more accurate focus.
        */

        private void Update () {

            // debug.text = String.Format("Current Parent State: {1}\nCurrent Leaf State: {2}\nSpeed: {3}\nTargets: {4}", currentState?.name, currentState?.currentSubState?.name, Mathf.Floor(characterData.moveData.velocity.magnitude), targettingController.targetLength);

            // Check Ground For State >> Update position >> Resolve Collisions >> Update Next State >> Update Next Rotation

            timerController.DecrementTimers();
            targettingController.FindTargets();
            collisionController.ResolveCollisions();

            Vector3 positionalMovement = transform.position - prevPosition; // TODO: 
            transform.position = prevPosition;
            characterData.moveData.origin += positionalMovement;

            ClampVelocity();
            // animationController.HandleAnimations();
            cameraController.CameraSettings();
            
            currentState.UpdateStates();

            // if (characterData.playerData.wishFireUp) {
            //     vfxController.FireHomingProjectile();
            // }

            if (preUpdateEnvironmentForces != Vector3.zero) {
                characterData.moveData.velocity += preUpdateEnvironmentForces;
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

            targettingController.AimAssist();

            StepTransformRotation();

            characterData.zVel = Vector3.Dot(characterData.moveData.velocity, characterData.bodyForward);
            characterData.yVel = Vector3.Dot(characterData.moveData.velocity, characterData.bodyUp);


            if (characterData.moveData.velocity != Vector3.zero) speedBall.transform.forward = characterData.moveData.velocity.normalized;

            characterData.playerData.wishJumpUp = false;
            characterData.playerData.wishJumpPress = false;
            characterData.playerData.wishSprintUp = false;
            characterData.playerData.wishTumbleUp = false;
            characterData.playerData.wishFireUp = false;
            characterData.playerData.wishFirePress = false;
            characterData.playerData.wishGrappleUp = false;
            characterData.playerData.wishGrapplePress = false;
            characterData.playerData.wishAimPress = false;
            characterData.playerData.wishAimUp = false;
            characterData.playerData.wishSkatePress = false;
            characterData.playerData.wishDashUp = false;
            characterData.playerData.detectWall = false;
            characterData.playerData.bonusTime = false;

            if (characterData.playerData.wishEscapeDown) {
                Application.Quit();
            }



        }

        public void HandleCharacterRotation() {

            Vector3 combinedLookPosition = characterData.lookAtThis.position;
            Quaternion combinedLookRotation = Quaternion.LookRotation((combinedLookPosition - characterData.avatarLookTransform.position).normalized, Vector3.up);

            if (characterData.moveData.velocity.magnitude > characterData.moveConfig.walkSpeed) { // TODO: make bodyTransform not this transform
                characterData.avatarLookRotation = Quaternion.Slerp(characterData.avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
                characterData.bodyRotation = Quaternion.Slerp(characterData.bodyRotation, FlatLookRotation(characterData.viewForward), Time.deltaTime * 10f);
                characterData.velocityRotation = Quaternion.LookRotation(characterData.moveData.velocity);
            } else {
                characterData.avatarLookRotation = Quaternion.Slerp(characterData.avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
                characterData.bodyRotation = Quaternion.Slerp(characterData.bodyRotation, FlatLookRotation(characterData.viewForward), Time.deltaTime * 10f);
                characterData.velocityRotation = characterData.bodyRotation;
            }

            // if (characterData.playerData.wishMove.magnitude > .3f) {
            //     characterData.bodyRotation = Quaternion.Slerp(characterData.bodyRotation, FlatLookRotation(characterData.playerData.wishMove.normalized), Time.deltaTime * 10f);
            // }

        }

        // private void Balls() {

        //     float distance = (characterData.moveData.velocity * Time.deltaTime).magnitude;
        //     float angle1 = distance * (180f / Mathf.PI);
        //     float ballAlignSpeed = 180f;

        //     Quaternion AlignBallRotation (Vector3 rotationAxis, Quaternion rotation) {
        //         Vector3 ballAxis = characterData.bodyUp;
        //         float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
        //         float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //         float maxAngle = ballAlignSpeed * Time.deltaTime;

        //         Quaternion newAlignment =
        //             Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
        //         if (angle <= maxAngle) {
        //             return newAlignment;
        //         }
        //         else {
        //             return Quaternion.SlerpUnclamped(
        //                 rotation, newAlignment, maxAngle / angle
        //             );
        //         }
        //     }

        //     Vector3 rotationAxis = Vector3.Cross(Vector3.up, characterData.moveData.velocity.normalized);

        //     if (characterData.playerData.detectWall) {

        //         if (characterData.playerData.wallNormal != Vector3.zero) {
        //             rotationAxis = Vector3.Cross(characterData.playerData.wallNormal, characterData.moveData.velocity.normalized);
        //         }
        //     }

        //     characterData.bodyRotation = Quaternion.Euler(rotationAxis * angle1) * characterData.bodyRotation;
        //     characterData.bodyRotation = AlignBallRotation(rotationAxis, characterData.bodyRotation);

        // }

        private void HandleLookChanged() {
            if (characterData.playerData.wishFireDown && characterData.playerData.mainTarget.position != Vector3.zero) { // hard lock on

                toTarget = targettingController.reticle.anchoredPosition.normalized;

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
        }

        private void LookDamp() {
            if (characterData.firstPersonCam.Priority == 1) {
                characterData.playerData.xAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .5f, Time.deltaTime * 2f);
                characterData.playerData.yAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .5f, Time.deltaTime * 2f);
            } else {
                characterData.playerData.xAimDamp = Mathf.Lerp(characterData.playerData.xAimDamp, .9f, Time.deltaTime * 2f);
                characterData.playerData.yAimDamp = Mathf.Lerp(characterData.playerData.yAimDamp, .9f, Time.deltaTime * 2f);
            }

            characterData.xMouseMovement = Mathf.Clamp(characterData.playerData.mouseDelta.x * characterData.moveConfig.horizontalSensitivity * characterData.moveConfig.sensitivityMultiplier * characterData.playerData.xAimDamp, -2.5f, 2.5f);
            characterData.yMouseMovement = Mathf.Clamp(-characterData.playerData.mouseDelta.y * characterData.moveConfig.verticalSensitivity  * characterData.moveConfig.sensitivityMultiplier * characterData.playerData.yAimDamp, -2.5f, 2.5f);

        }


        public void StepTransformRotation() { // must handle here because both controllers used
                
                HandleCharacterRotation();

                LookDamp();
                
                HandleFirstPerson();
                HandleLookChanged();
                
        }

        private void HandleFirstPerson() {
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
    
        private void OnDestroy()
        {

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
    
        private void ClampVelocity() {
    
            float tmp = characterData.moveData.velocity.y;
            characterData.moveData.velocity.y = 0f;
    
            characterData.moveData.velocity = Vector3.ClampMagnitude(characterData.moveData.velocity, characterData.moveConfig.maxVelocity);
            characterData.moveData.velocity.y = Mathf.Max(tmp, -characterData.moveConfig.terminalVelocity);
            characterData.moveData.velocity.y = Mathf.Min(tmp, characterData.moveConfig.terminalVelocity);
    
        }
    
        
    
    }

    
}
