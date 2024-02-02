using UnityEngine;
using System.Collections.Generic;

namespace Game.Data {


    public class InputData {
        public bool down = false;
        public bool up = false;
        public bool press = false;

    }

    [System.Serializable]
    public class PlayerData : MonoBehaviour {

        ///// Fields /////

        // Core Data
        
        public Vector3 inputDir;
        public Vector2 mouseDelta;
        public Vector2 mousePosition;
        public bool aimAcceleration;
        public bool lockOn;
        public bool fastFall;
        public bool aimDown;
        public bool switchWeapon;
        public bool superJump;

        // Input
        
        public float verticalAxis = 0f;
        public float horizontalAxis = 0f;
        public Vector3 wishMove = Vector3.zero;
        public Vector3 input = Vector3.zero;
        public Vector3 xzWishMove = Vector3.zero;
        public bool wishJumpDown = false;
        public bool wishJumpPress = false;
        public bool wishJumpUp = false;
        public bool wishDashUp = false;
        public bool wishDashDown = false;
        public bool wishDashPress = false;

        public bool wishRunDown = false;
        public bool wishRunUp = false;
        public bool wishTumbleDown = false;
        public bool wishTumbleUp = false;
        public bool wishEscapeDown = false;
        public bool wishFireDown = false;
        public bool wishFireUp = false;
        public bool wishFirePress = false;
        public bool wishGrappleDown = false;
        public bool wishGrappleUp = false;
        public bool wishGrapplePress = false;
        public bool wishAimDown = false;
        public bool wishAimPress = false;
        public bool wishAimUp = false;
        
        // Player State
        
        public bool grappling = false; // are we currently
        public bool attacking = false;
        public bool grounded = false;
        public bool detectWall = false;
        public bool hovering = false;

        // Grapple

        public Vector3 grapplePoint;
        public SpringJoint joint;
        public float distanceFromGrapple = 0f;
        public Vector3 grappleNormal;
        public Vector3 grappleDir;
        public float lookingAtPoint;

        // Focus

        public Vector3 focusPoint; // 
        public Vector3 focusNormal;
        public Vector3 focusDir;
        public float distanceFromFocus = 0f;

        // Other
        public Vector3 targetNormal;
        public Vector3 targetMean;
        public Transform mainTarget;
        public Vector3 targetDir;
        public float distanceFromTarget = 0f;
        public Vector3 wallNormal;
        public float vCharge;
        public float xAimDamp;
        public float yAimDamp;

        public List<Collider> detectedTargets;

    }

}

