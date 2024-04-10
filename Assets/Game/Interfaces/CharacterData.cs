using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Cinemachine;

using Game.StateMachine;
using System;

namespace Game.Data {
    [Serializable]
    public class CharacterData : MonoBehaviour
    {
        // void HandleKnockback(PlayerCharacter target, Vector3 direction, float knockbackAmount);
        // void HandleDamage(PlayerCharacter target, float damageAmount);
        // void HandleStagger(PlayerCharacter target, float duration);

        [HideInInspector] public Vector3 lastContactRight = Vector3.up;
        [HideInInspector] public Vector3 lastSteepRight = Vector3.up;
        
        [HideInInspector] public int gear;
        [HideInInspector] public float zVel;
        [HideInInspector] public float xVel;
        [HideInInspector] public float yVel;

        [HideInInspector] public float xMouseMovement;
    	[HideInInspector] public float yMouseMovement;
    
        [HideInInspector] public PlayerData playerData;
        [HideInInspector] public MoveData moveData;
        [HideInInspector] public MoveConfig moveConfig;
        [HideInInspector] public Material[] characterMaterials;
        PlayerState _currentState;
        [HideInInspector] public CapsuleCollider playerCollider;
        [HideInInspector] public GameObject groundObject;
    
        // Quaternion FlatLookRotation(Vector3 forward, Vector3 normal);
    
        // void StopGrapple();
    
        [HideInInspector] public Vector3 groundNormal;
        public PlayerControls playerControls;

        public GameObject _virtualFramingCam;
        public GameObject _firstPersonCam;
        public GameObject _thirdPersonCam;
        [HideInInspector] public CinemachineVirtualCamera virtualFramingCam;
        [HideInInspector] public CinemachineVirtualCamera firstPersonCam;
        [HideInInspector] public CinemachineVirtualCamera thirdPersonCam;
        [HideInInspector] public CinemachineFramingTransposer framingCam;
        [HideInInspector] public Cinemachine3rdPersonFollow followCam;
        [HideInInspector] public CinemachineSameAsFollowTarget aimCam;
        [HideInInspector] public CinemachineFramingTransposer groupcam;
        [HideInInspector] public CinemachineTargetGroup targetGroup;
        [HideInInspector] public CinemachineBrain brain;

        public Camera cam;
        [HideInInspector] public Quaternion viewRotation { get { return cam.transform.rotation; } set { cam.transform.rotation = value; } }
        [HideInInspector] public Vector3 viewForward { get { return cam.transform.forward; } set { cam.transform.forward = value; } }
        [HideInInspector] public Vector3 viewRight { get { return cam.transform.right; } }
        [HideInInspector] public Vector3 viewUp { get { return cam.transform.up; } }
        public Transform avatarLookTransform;
        public Transform bodyTransform;
        [HideInInspector] public Quaternion avatarLookRotation { get { return avatarLookTransform.rotation; } set { avatarLookTransform.rotation = value; } }
        [HideInInspector] public Vector3 avatarLookForward { get { return avatarLookTransform.forward; } set { avatarLookTransform.forward = value; } }
        [HideInInspector] public Vector3 avatarLookForwardFlat { get { return Vector3.ProjectOnPlane(avatarLookTransform.forward, Vector3.up); } }
        [HideInInspector] public Vector3 avatarLookRight { get { return avatarLookTransform.right; } }
        [HideInInspector] public Vector3 avatarLookUp { get { return avatarLookTransform.up; } }
        [HideInInspector] public Quaternion bodyRotation { get { return bodyTransform.rotation; } set { bodyTransform.rotation = value; } }
        [HideInInspector] public Vector3 bodyForward { get { return bodyTransform.forward; } }
        [HideInInspector] public Vector3 bodyRight { get { return bodyTransform.right; } }
        [HideInInspector] public Vector3 bodyUp { get { return bodyTransform.up; } }
        [HideInInspector] public Quaternion velocityRotation;
        [HideInInspector] public Vector3 velocityForward { get { return velocityRotation * Vector3.forward; } }
        [HideInInspector] public Vector3 velocityRight { get { return velocityRotation * Vector3.right; } }
        [HideInInspector] public Vector3 velocityUp { get { return velocityRotation * Vector3.up; } }
        public Volume globalVolume;
        public Transform lookAtThis;
        public Transform focusOnThis;
        [HideInInspector] public Quaternion focusRotation { get { return Quaternion.LookRotation((focusOnThis.transform.position - avatarLookTransform.position).normalized, groundNormal); } }
        protected Camera avatarCamera;
        public Vector3 viewTransformLook;

        public Collider currentTarget = null;

        private void Awake() {

            virtualFramingCam =  _virtualFramingCam.GetComponent<CinemachineVirtualCamera>();
            firstPersonCam =  _firstPersonCam.GetComponent<CinemachineVirtualCamera>();
            thirdPersonCam =  _thirdPersonCam.GetComponent<CinemachineVirtualCamera>();
            framingCam = _virtualFramingCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            followCam = _thirdPersonCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            // groupcam = _groupCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            // targetGroup = _targetGroup.GetComponent<CinemachineTargetGroup>();
            brain = cam.GetComponent<CinemachineBrain>();
            aimCam = _virtualFramingCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineSameAsFollowTarget>(); // frog

            playerControls = new PlayerControls();

            // avatarCamera = avatarLookTransform.gameObject.GetComponent<Camera>();

            lookAtThis.position = Vector3.zero;
            lookAtThis.localPosition = Vector3.zero;
            // lookAtThis.localScale = new Vector3(moveConfig.castRadius/2f, moveConfig.castRadius/2f, moveConfig.castRadius/2f);
    
            focusOnThis.position = Vector3.zero;
            focusOnThis.localPosition = Vector3.zero;
            focusOnThis.localScale = new Vector3(2f, 2f, 2f);
    
            avatarLookForward = bodyForward;

            playerData = GetComponent<PlayerData>();
            moveData = GetComponent<MoveData>();
            moveConfig = GetComponent<MoveConfig>();

        }
    
    }
}