using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.VFX;
using Cinemachine;


public interface IPlayer
{
    void HandleKnockback(PlayerCharacter target, Vector3 direction, float knockbackAmount);
    void HandleDamage(PlayerCharacter target, float damageAmount);
    void HandleStagger(PlayerCharacter target, float duration);

    public PlayerData playerData { get; }
    MoveData moveData { get; }
    MoveConfig moveConfig { get; }
    Camera cam { get; set; }
    CapsuleCollider playerCollider { get; }
    GameObject groundObject { get; set; }
    Vector3 bodyForward { get; }
    Vector3 bodyRight { get; }
    Vector3 bodyUp { get; }
    Vector3 avatarLookForward { get; set; }
    Vector3 avatarLookRight { get; }
    Vector3 avatarLookUp { get; }
    Vector3 viewForward { get; set; }
    Vector3 viewRight { get; }
    Vector3 viewUp { get; }
    Vector3 leftSide { get; }
    Vector3 rightSide { get; }
    Vector3 backSide { get; }
    Vector3 frontSide { get; }
    Vector3 velocityForward { get; }

    Quaternion FlatLookRotation(Vector3 forward, Vector3 normal);

    // void StopGrapple();

    Vector3 groundNormal { get; }
    VisualEffect _grappleArc { get; }

}

public abstract class PlayerCharacter : MonoBehaviour, IPlayer
{
    public virtual void HandleDamage(PlayerCharacter target, float damageAmount){}
    public virtual void HandleKnockback(PlayerCharacter target, Vector3 direction, float knockbackAmount){}
    public virtual void HandleStagger(PlayerCharacter target, float duration){}

    public Transform rightHandEffector;

    public Material cloakMat;
    public Vector3 preUpdateEnvironmentForces;
    public Vector3 squash;

    [SerializeField] protected GameObject grappleGun;
    [SerializeField] protected GameObject smokeObj;
    [SerializeField] protected GameObject smokeLandObj;
    [SerializeField] public GameObject sonicBoomObj;
    [SerializeField] protected GameObject airHikeObj;
    [SerializeField] protected GameObject sphereLinesObj;
    [SerializeField] public GameObject ballObj;
    public Volume globalVolume;
    [HideInInspector] public VisualEffect grappleArc;
    [HideInInspector] public VisualEffect slash;
    [HideInInspector] public VisualEffect smoke;
    [HideInInspector] public VisualEffect smokeLand;
    [HideInInspector] public VisualEffect sonicBoom;
    [HideInInspector] public VisualEffect airHike;
    [HideInInspector] public VisualEffect sphereLines;
    public GameObject _virtualFramingCam;
    public GameObject _firstPersonCam;
    public GameObject _thirdPersonCam;
    public ParticleSystem speedTrails;
    [HideInInspector] public CinemachineVirtualCamera virtualFramingCam;
    [HideInInspector] public CinemachineVirtualCamera firstPersonCam;
    [HideInInspector] public CinemachineVirtualCamera thirdPersonCam;
    [HideInInspector] public CinemachineFramingTransposer framingCam;
    [HideInInspector] public Cinemachine3rdPersonFollow followCam;
    [HideInInspector] public CinemachineSameAsFollowTarget aimCam;
    [HideInInspector] public CinemachineFramingTransposer groupcam;
    [HideInInspector] public CinemachineTargetGroup targetGroup;
    [HideInInspector] public CinemachineBrain brain;
    
    [SerializeField] LayerMask _groundMask;

    public Text debug;

    public BezierCurve bezierCurve;

    public Camera _cam;
    GameObject _groundObject;
    public Transform avatarLookTransform;
    protected CapsuleCollider _playerCollider;
    protected Vector3 _frontSide;
    protected Vector3 _leftSide;
    protected Vector3 _rightSide;
    protected Vector3 _backSide;

    PlayerBaseState _currentState;
    public PlayerControls PlayerControls;

    public PlayerData _playerData;
    public MoveData _moveData;
    public MoveConfig _moveConfig;
    protected float zVel;
    protected float xVel;
    protected float yVel;

    public PlayerBaseState currentState { get {return _currentState; } set { _currentState = value; } }
    public MoveConfig moveConfig { get { return _moveConfig; } }
    public PlayerData playerData { get { return _playerData; } }
    public MoveData moveData { get { return _moveData; } }

    public Camera cam { get { return _cam; } set { _cam = value; } }
    [HideInInspector] public Vector3 viewForward { get { return cam.transform.forward; } set { cam.transform.forward = value; } }
    [HideInInspector] public Vector3 viewRight { get { return cam.transform.right; } }
    [HideInInspector] public Vector3 viewUp { get { return cam.transform.up; } }
    [HideInInspector] public Vector3 avatarLookForward { get { return avatarLookTransform.forward; } set { avatarLookTransform.forward = value; } }
    [HideInInspector] public Vector3 avatarLookRight { get { return avatarLookTransform.right; } }
    [HideInInspector] public Vector3 avatarLookUp { get { return avatarLookTransform.up; } }
    [HideInInspector] public Vector3 bodyForward { get { return transform.forward; } }
    [HideInInspector] public Vector3 bodyRight { get { return transform.right; } }
    [HideInInspector] public Vector3 bodyUp { get { return transform.up; } }
    [HideInInspector] public Vector3 velocityForward { get { return velocityRotation * Vector3.forward; } }
    [HideInInspector] public Vector3 velocityRight { get { return velocityRotation * Vector3.right; } }
    [HideInInspector] public Vector3 velocityUp { get { return velocityRotation * Vector3.up; } }
    [HideInInspector] public Vector3 leftSide { get { return _leftSide; } set { _leftSide = value; } }
    [HideInInspector] public Vector3 rightSide { get { return _rightSide; } set { _rightSide = value; } }
    [HideInInspector] public Vector3 backSide { get { return _backSide; } set { _backSide = value; } }
    [HideInInspector] public Vector3 frontSide { get { return _frontSide; } set { _frontSide = value; } }
    [HideInInspector] public LayerMask groundMask { get { return _groundMask; } set { _groundMask = value; } }
    [HideInInspector] public Vector3 groundNormal { get { return _groundNormal; } set { _groundNormal = value; } }
    [HideInInspector] public CapsuleCollider playerCollider { get { return _playerCollider; } set { _playerCollider = value; } }
    [HideInInspector] public GameObject groundObject { get { return _groundObject; } set { _groundObject = value; } }
    [HideInInspector] public Quaternion viewRotation { get { return cam.transform.rotation; } set { cam.transform.rotation = value; } }
    [HideInInspector] public Quaternion avatarLookRotation { get { return avatarLookTransform.rotation; } set { avatarLookTransform.rotation = value; } }
    [HideInInspector] public Quaternion bodyRotation { get { return transform.rotation; } set { transform.rotation = value; } }
    [HideInInspector] public Quaternion focusRotation { get { return Quaternion.LookRotation((focusOnThis.transform.position - avatarLookTransform.position).normalized, groundNormal); } }
    public VisualEffect _grappleArc { get { return grappleArc; } }

    protected Vector3 prevPosition;
    protected Animator animator;
    protected Vector3 _groundNormal = Vector3.up;
    protected Vector3 lastContact = Vector3.zero;
    [HideInInspector] public float wallTouchTimer = 0f;
    [HideInInspector] public float jumpTimer = 0f;
    [HideInInspector] public float groundInputTimer = 0f;
    [HideInInspector] public float boostInputTimer = 0f;
    [HideInInspector] public float grappleZipTimer = 0f;
    [HideInInspector] public float reduceGravityTimer = 0f;
    [HideInInspector] public float ignoreGravityTimer = 0f;
    [HideInInspector] public float inputBufferTimer = 0f;
    [HideInInspector] public float runTimer = 2f;
    [HideInInspector] public float lungeCooldownTimer = 0f;
    [HideInInspector] public float releaseTimer = 0f;
    [HideInInspector] public bool doubleJump = false;
    [HideInInspector] public Quaternion velocityRotation;
	[HideInInspector] public float xMovement;
	[HideInInspector] public float yMovement;
	[HideInInspector] protected Vector3 camMask = new Vector3(0, 1, -2);
    [HideInInspector] public Vector3 viewTransformLookAt;
    protected Vector3 cameraShift = Vector3.zero;

    public Transform lookAtThis;
    public Transform focusOnThis;

    [HideInInspector] public GameObject slashObj;

    [HideInInspector] public int targetLength;
    [HideInInspector] public Collider currentTarget = null;
    [HideInInspector] public float focusAimBlend;

    public GameObject energyObj;
    public GameObject speedBall;
    public Slider energySlider;
    protected AnimationCurve curve;
    public RectTransform reticle;
    protected Vector3 toTarget = Vector3.zero;
    protected Camera avatarCamera;
    public Material[] characterMaterials;
    public Material speedBallMaterial;
    public float dither;
    public bool isTrailActive;
    public float meshRefreshRate = 0.1f;
    public float activeTime = 2f;
    protected SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Material mat;

    

    protected void Awake() {

        playerCollider = transform.GetComponent<CapsuleCollider>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        slashObj = transform.GetChild(1).transform.GetChild(1).gameObject;

        energySlider = energyObj.GetComponent<Slider>();
        energySlider.value = .25f;

        moveConfig.grappleColor = moveConfig.normalColor;

        virtualFramingCam =  _virtualFramingCam.GetComponent<CinemachineVirtualCamera>();
        firstPersonCam =  _firstPersonCam.GetComponent<CinemachineVirtualCamera>();
        thirdPersonCam =  _thirdPersonCam.GetComponent<CinemachineVirtualCamera>();
        framingCam = _virtualFramingCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        followCam = _thirdPersonCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        // groupcam = _groupCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        // targetGroup = _targetGroup.GetComponent<CinemachineTargetGroup>();
        brain = cam.GetComponent<CinemachineBrain>();
        aimCam = _virtualFramingCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineSameAsFollowTarget>();
        
        moveData.origin = transform.position;
        prevPosition = transform.position;

        speedBall.transform.localScale = new Vector3(2f, 2f, 2f);

        PlayerControls = new PlayerControls();

        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        grappleArc = grappleGun.GetComponent<VisualEffect>();
        smoke = smokeObj.GetComponent<VisualEffect>();
        smokeLand = smokeLandObj.GetComponent<VisualEffect>();
        sonicBoom = sonicBoomObj.GetComponent<VisualEffect>();
        airHike = airHikeObj.GetComponent<VisualEffect>();
        sphereLines = sphereLinesObj.GetComponent<VisualEffect>();
        avatarCamera = avatarLookTransform.gameObject.GetComponent<Camera>();

        dither = 0f;


        sonicBoomObj.SetActive(true);

        sonicBoom.Stop();
        sphereLines.Stop();

        viewTransformLookAt = cam.transform.forward;
        lookAtThis.position = Vector3.zero;
        lookAtThis.localPosition = Vector3.zero;
        lookAtThis.localScale = new Vector3(moveConfig.castRadius/2f, moveConfig.castRadius/2f, moveConfig.castRadius/2f);

        focusOnThis.position = Vector3.zero;
        focusOnThis.localPosition = Vector3.zero;
        focusOnThis.localScale = new Vector3(2f, 2f, 2f);

        avatarLookForward = bodyForward;
        focusAimBlend = .5f;

        playerData.detectedTargets = new List<Collider>();

    }

    protected void OnEnable() {
        PlayerControls.Enable();

        EventManager.OnKnockbackReceived += HandleKnockback;
        EventManager.OnDamageReceived += HandleDamage;
        EventManager.OnStaggerReceived += HandleStagger;
    }

    protected void OnDisable() {
        PlayerControls.Disable();

        EventManager.OnKnockbackReceived -= HandleKnockback;
        EventManager.OnDamageReceived -= HandleDamage;
        EventManager.OnStaggerReceived -= HandleStagger;
    }


    public Quaternion FlatLookRotation(Vector3 forward) {
        return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, groundNormal).normalized, groundNormal);
    }

    public Quaternion FlatLookRotation(Vector3 forward, Vector3 normal) {
        return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, normal).normalized, normal);
    }

    protected void Start() {

        // EntityManager entityManager = World.Active.EntityManager;
        

        PlayerControls.Player.Dash.started += context => {
            playerData.wishDashDown = true;
            playerData.wishDashPress = true;
        };

        PlayerControls.Player.Dash.canceled += context => {
            playerData.wishDashDown = false;
            playerData.wishDashUp = true;
        };

        PlayerControls.Player.Grapple.started += context => {
            playerData.wishGrappleDown = true;
        };

        PlayerControls.Player.Grapple.canceled += context => {
            playerData.wishGrappleDown = false;
        };

        PlayerControls.Player.Tumble.started += context => {
            playerData.wishTumbleDown = true;
        };

        PlayerControls.Player.Tumble.canceled += context => {
            playerData.wishTumbleDown = false;
        };

        PlayerControls.Player.Escape.started += context => {
            playerData.wishEscapeDown = true;
        };

        PlayerControls.Player.Escape.canceled += context => {
            playerData.wishEscapeDown = false;
        };

        PlayerControls.Player.Fire.started += context => {
            playerData.wishFireDown = true;
            playerData.wishFirePress = true;
        };

        PlayerControls.Player.Fire.canceled += context => {
            playerData.wishFireDown = false;
            playerData.wishFireUp = true;
        };

        PlayerControls.Player.Aim.started += context => {
            playerData.wishAimDown = true;
            playerData.wishAimPress = true;
        };

        PlayerControls.Player.Aim.canceled += context => {
            playerData.wishAimDown = false;
            playerData.wishAimUp = true;
        };

        PlayerControls.Player.Grapple.started += context => {
            playerData.wishGrappleDown = true;
            playerData.wishGrapplePress = true;
        };

        PlayerControls.Player.Grapple.canceled += context => {
            playerData.wishGrappleDown = false;
            playerData.wishGrappleUp = true;
        };

        PlayerControls.Player.Move.started += context => {
            playerData.horizontalAxis = context.ReadValue<Vector2>().x;
            playerData.verticalAxis = context.ReadValue<Vector2>().y;
            playerData.input = context.ReadValue<Vector2>();
            playerData.input.z = playerData.input.y;
            playerData.input.y = 0f;
        };

        PlayerControls.Player.Move.performed += context => {
            playerData.horizontalAxis = context.ReadValue<Vector2>().x;
            playerData.verticalAxis = context.ReadValue<Vector2>().y;
            playerData.input = context.ReadValue<Vector2>();
            playerData.input.z = playerData.input.y;
            playerData.input.y = 0f;
        };

        PlayerControls.Player.Move.canceled += context => {
            playerData.horizontalAxis = context.ReadValue<Vector2>().x;
            playerData.verticalAxis = context.ReadValue<Vector2>().y;
            playerData.input = context.ReadValue<Vector2>();
            playerData.input.z = playerData.input.y;
            playerData.input.y = 0f;
        };

        PlayerControls.Player.Run.started += context => {
            playerData.wishRunDown = true;
        };

        PlayerControls.Player.Run.canceled += context => {
            playerData.wishRunDown = false;
            playerData.wishRunUp = true;
        };


        PlayerControls.Player.Jump.started += context => {
            playerData.wishJumpDown = true;
        };

        PlayerControls.Player.Jump.canceled += context => {
            playerData.wishJumpDown = false;
            playerData.wishJumpUp = true;
        };

        // 1920 x 1200 res

        PlayerControls.Player.Look.performed += context => {
            playerData.mouseDelta = context.ReadValue<Vector2>();
            
        };

        PlayerControls.Player.AimAcceleration.performed += context => {
            playerData.aimAcceleration = context.ReadValue<bool>();
            
        };

        PlayerControls.Player.SuperJump.performed += context => {
            playerData.superJump = context.ReadValue<bool>();
            
        };

        PlayerControls.Player.LockOn.performed += context => {
            playerData.lockOn = context.ReadValue<bool>();
            
        };

        PlayerControls.Player.FastFall.performed += context => {
            playerData.fastFall = context.ReadValue<bool>();
            
        };

        PlayerControls.Player.SwitchWeapon.performed += context => {
            playerData.switchWeapon = context.ReadValue<bool>();
            
        };

        currentState = new PlayerStateAir(this, new PlayerStateFactory((PlayerCharacter)this));
        currentState.InitializeSubStates();
        // bezierCurve = new BezierCurve(this);

    }

    protected void ConnectGrapple(Vector3 grapplePosition) {

        if (Vector3.Distance(moveData.origin, grapplePosition) < moveConfig.minDistance && energySlider.value < .1f) {
            return;
        }

        playerData.grapplePoint = grapplePosition;
        playerData.joint = gameObject.AddComponent<SpringJoint>();
        playerData.joint.autoConfigureConnectedAnchor = false;
        playerData.joint.connectedAnchor = playerData.grapplePoint;

        playerData.distanceFromGrapple = Vector3.Distance(moveData.origin, playerData.grapplePoint);
        playerData.grappling = true;

    }

    public void StopGrapple() {
        playerData.grapplePoint = focusOnThis.position;
        Destroy(playerData.joint);

        // bezierCurve.Clear();
        grappleArc.enabled = false;
        
    }

    protected void CheckGrounded() {

        RaycastHit hit;
        if (Physics.Raycast (
            origin: moveData.origin,
            direction: -groundNormal,
            hitInfo: out hit,
            maxDistance: 1.4f,
            layerMask: LayerMask.GetMask (new string[] { "Focus", "Ground" }),
            queryTriggerInteraction: QueryTriggerInteraction.Ignore)) {
            
        }

        if (hit.collider == null || jumpTimer > 0f) {

            SetGround(null);
            groundNormal = Vector3.Lerp(groundNormal, Vector3.up, Time.deltaTime / 2f);
            playerData.grounded = false;

        } else {

            lastContact = hit.point;
            groundNormal = hit.normal.normalized;
            SetGround(hit.collider.gameObject);

            // if (Vector3.Distance(moveData.origin - groundNormal, lastContact) < .49f) {
            //     moveData.origin += groundNormal * Mathf.Min(Time.deltaTime, .01f); // soft collision resolution?
            // }

            if (!playerData.grounded) {

                if (Vector3.Dot(moveData.velocity, groundNormal) <= -7.5f) {
                    // smokeLand.SetVector3("velocity", Vector3.ProjectOnPlane(moveData.velocity / 2f, groundNormal));
                    // smokeLand.SetVector3("position", moveData.origin - groundNormal / 2f);
                    // smokeLand.SetVector3("eulerAngles", Quaternion.LookRotation(groundNormal, Vector3.ProjectOnPlane(-velocityForward, groundNormal)).eulerAngles);
                    // smokeLand.Play();
                }
                
            }

            doubleJump = true;

            playerData.grounded = true;

        }
    }

    protected void SetGround (GameObject obj) {

        if (obj != null) {

            groundObject = obj;

        } else
            groundObject = null;

    }
    

    protected void ClampVelocity() {

        float tmp = moveData.velocity.y;
        moveData.velocity.y = 0f;

        moveData.velocity = Vector3.ClampMagnitude(moveData.velocity, moveConfig.maxVelocity);
        moveData.velocity.y = Mathf.Max(tmp, -moveConfig.terminalVelocity);
        moveData.velocity.y = Mathf.Min(tmp, moveConfig.terminalVelocity);

    }

    protected void ResolveCollisions() {

        DivePhysics.ResolveCollisions(playerCollider, ref moveData.origin, ref moveData.velocity, LayerMask.GetMask (new string[] { "Ground" }), ref squash);
        moveData.origin += moveData.velocity * Time.deltaTime; // p = v * dt

    }

    public void CollisionCheck() {

        CheckGrounded();
        
        if (jumpTimer > 0f) return;

        RaycastHit hit;
        if (Physics.Raycast(moveData.origin, bodyForward, out hit, 1.1f, groundMask)) {
            frontSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, bodyForward + bodyRight, out hit, 1.1f, groundMask)) {
            frontSide = hit.normal;
            if (frontSide == Vector3.zero) rightSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, bodyRight, out hit, 1.1f, groundMask)) {
            rightSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, bodyRight - bodyForward, out hit, 1.1f, groundMask)) {
            rightSide = hit.normal;
            if (rightSide == Vector3.zero) backSide = hit.normal;
        }
        
        if (Physics.Raycast(moveData.origin, -bodyForward, out hit, 1.1f, groundMask)) {
            backSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, -bodyForward - bodyRight, out hit, 1.1f, groundMask)) {
            backSide = hit.normal;
            if (backSide == Vector3.zero) leftSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, -bodyRight, out hit, 1.1f, groundMask)) {
            leftSide = hit.normal;
        }

        if (Physics.Raycast(moveData.origin, -bodyRight + bodyForward, out hit, 1.1f, groundMask)) {
            leftSide = hit.normal;
            if (leftSide == Vector3.zero) frontSide = hit.normal;
        }

        playerData.detectWall = leftSide != Vector3.zero || rightSide != Vector3.zero || backSide != Vector3.zero || frontSide != Vector3.zero;
        playerData.detectWall = playerData.detectWall && wallTouchTimer <= 0f;

        if (playerData.detectWall) {
            playerData.wallNormal = (leftSide + rightSide + backSide + frontSide).normalized;
        } else {
            playerData.wallNormal = Vector3.zero;
        }
        

        // if (Physics.) 

    }

}
