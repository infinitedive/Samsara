using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using System;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class GearController : MonoBehaviour
    {

        private CharacterData characterData;
        private MoveConfig moveConfig;
        private CameraController cameraController;
        private int maxGear = 2;

        private void Awake() {

            characterData = GetComponent<CharacterData>();

        }

        public void UpGear() {

            if (characterData.gear < maxGear) characterData.gear += 1;
        }

        public void DownGear() {

            if (characterData.gear < maxGear) characterData.gear -= 1;
        }

        // public void ProcessControls() {

        //     switch (characterData.gear) {
        //         case 0:
        //             GearZero();
        //             break;

        //         case 1:
        //             GearOne();
        //             break;

        //         case 2:
        //             GearTwo();
        //             break;
                

        //     }

        // }

        public void GearZero() { // airplane
            characterData.playerData.wishMove = characterData.viewForward;

        }

        public void GearOne() { // strafe
            float forwardMove = characterData.playerData.verticalAxis;
            float rightMove = characterData.playerData.horizontalAxis;
            float upMove = characterData.playerData.wishJumpDown ? 1f : -1f;

            Vector3 wishDir = forwardMove * Vector3.forward + rightMove * Vector3.right;

            Quaternion avatarLookFlat = Quaternion.LookRotation(Vector3.ProjectOnPlane(characterData.avatarLookForward, Vector3.up).normalized, Vector3.up);
            characterData.playerData.wishMove = avatarLookFlat * wishDir;
        }

        public void GearTwo() { // radial

            characterData.playerData.wishMove = Vector3.ProjectOnPlane(characterData.viewForward, characterData.playerData.targetNormal);
            
            float upMove = characterData.playerData.verticalAxis;
            float rightMove = characterData.playerData.horizontalAxis;
            float forwardMove = characterData.playerData.wishJumpDown ? 1f : -1f;

            Vector3 wishDir = upMove * Vector3.up + rightMove * Vector3.right;


        }

    }

}