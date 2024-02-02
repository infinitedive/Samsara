using UnityEngine;
using UnityEngine.Events;

using Game.StateMachine;
using Game.Data;


namespace Game {
    public class BaseCharacterController : MonoBehaviour
    {

        [SerializeField] Ability[] abilities;

        [HideInInspector] public CharacterData characterData;
        [HideInInspector] public PlayerState _currentState;
        [HideInInspector] public PlayerState currentState { get {return _currentState; } set { _currentState = value; } }
    
        public void Start() {

            // characterData = GetComponent<CharacterData>();

    
            // EntityManager entityManager = World.Active.EntityManager;
        
            characterData.playerControls.Player.Dash.started += context => {
                characterData.playerData.wishDashDown = true;
                characterData.playerData.wishDashPress = true;
            };
    
            characterData.playerControls.Player.Dash.canceled += context => {
                characterData.playerData.wishDashDown = false;
                characterData.playerData.wishDashUp = true;
            };
    
            characterData.playerControls.Player.Grapple.started += context => {
                characterData.playerData.wishGrappleDown = true;
            };
    
            characterData.playerControls.Player.Grapple.canceled += context => {
                characterData.playerData.wishGrappleDown = false;
            };
    
            characterData.playerControls.Player.Tumble.started += context => {
                characterData.playerData.wishTumbleDown = true;
            };
    
            characterData.playerControls.Player.Tumble.canceled += context => {
                characterData.playerData.wishTumbleDown = false;
            };
    
            characterData.playerControls.Player.Escape.started += context => {
                characterData.playerData.wishEscapeDown = true;
            };
    
            characterData.playerControls.Player.Escape.canceled += context => {
                characterData.playerData.wishEscapeDown = false;
            };
    
            characterData.playerControls.Player.Fire.started += context => {
                characterData.playerData.wishFireDown = true;
                characterData.playerData.wishFirePress = true;
            };
    
            characterData.playerControls.Player.Fire.canceled += context => {
                characterData.playerData.wishFireDown = false;
                characterData.playerData.wishFireUp = true;
            };
    
            characterData.playerControls.Player.Aim.started += context => {
                characterData.playerData.wishAimDown = true;
                characterData.playerData.wishAimPress = true;
            };
    
            characterData.playerControls.Player.Aim.canceled += context => {
                characterData.playerData.wishAimDown = false;
                characterData.playerData.wishAimUp = true;
            };
    
            characterData.playerControls.Player.Grapple.started += context => {
                characterData.playerData.wishGrappleDown = true;
                characterData.playerData.wishGrapplePress = true;
            };
    
            characterData.playerControls.Player.Grapple.canceled += context => {
                characterData.playerData.wishGrappleDown = false;
                characterData.playerData.wishGrappleUp = true;
            };
    
            characterData.playerControls.Player.Move.started += context => {
                characterData.playerData.horizontalAxis = context.ReadValue<Vector2>().x;
                characterData.playerData.verticalAxis = context.ReadValue<Vector2>().y;
                characterData.playerData.input = context.ReadValue<Vector2>();
                characterData.playerData.input.z = characterData.playerData.input.y;
                characterData.playerData.input.y = 0f;
            };
    
            characterData.playerControls.Player.Move.performed += context => {
                characterData.playerData.horizontalAxis = context.ReadValue<Vector2>().x;
                characterData.playerData.verticalAxis = context.ReadValue<Vector2>().y;
                characterData.playerData.input = context.ReadValue<Vector2>();
                characterData.playerData.input.z = characterData.playerData.input.y;
                characterData.playerData.input.y = 0f;
            };
    
            characterData.playerControls.Player.Move.canceled += context => {
                characterData.playerData.horizontalAxis = context.ReadValue<Vector2>().x;
                characterData.playerData.verticalAxis = context.ReadValue<Vector2>().y;
                characterData.playerData.input = context.ReadValue<Vector2>();
                characterData.playerData.input.z = characterData.playerData.input.y;
                characterData.playerData.input.y = 0f;
            };
    
            characterData.playerControls.Player.Run.started += context => {
                characterData.playerData.wishRunDown = true;
            };
    
            characterData.playerControls.Player.Run.canceled += context => {
                characterData.playerData.wishRunDown = false;
                characterData.playerData.wishRunUp = true;
            };
    
    
            characterData.playerControls.Player.Jump.started += context => {
                characterData.playerData.wishJumpDown = true;
                characterData.playerData.wishJumpPress = true;
            };
    
            characterData.playerControls.Player.Jump.canceled += context => {
                characterData.playerData.wishJumpDown = false;
                characterData.playerData.wishJumpUp = true;
            };
    
            // 1920 x 1200 res
    
            characterData.playerControls.Player.Look.performed += context => {
                characterData.playerData.mouseDelta = context.ReadValue<Vector2>();
                
            };
    
            characterData.playerControls.Player.AimAcceleration.performed += context => {
                characterData.playerData.aimAcceleration = context.ReadValue<bool>();
                
            };
    
            characterData.playerControls.Player.SuperJump.performed += context => {
                characterData.playerData.superJump = context.ReadValue<bool>();
                
            };
    
            characterData.playerControls.Player.LockOn.performed += context => {
                characterData.playerData.lockOn = context.ReadValue<bool>();
                
            };
    
            characterData.playerControls.Player.FastFall.performed += context => {
                characterData.playerData.fastFall = context.ReadValue<bool>();
                
            };
    
            characterData.playerControls.Player.SwitchWeapon.performed += context => {
                characterData.playerData.switchWeapon = context.ReadValue<bool>();
                
            };
    
            
    
        }
    
        private void OnEnable() {
            characterData.playerControls.Enable();
        }
    
        private void OnDisable() {
            characterData.playerControls.Disable();
        }
        
    }
}
