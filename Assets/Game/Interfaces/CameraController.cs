using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {

    public class CameraController : MonoBehaviour {

        public Vector3 viewTransformLookAt;
        private CharacterData characterData;
        public Vector3 cameraShift = Vector3.zero;
        

        private void Awake() {

            characterData = GetComponent<CharacterData>();

        }

        public void CameraSettings() { // TODO:

            characterData.framingCam.m_UnlimitedSoftZone = false;

            // if (characterData.playerData.detectWall && characterData.firstPersonCam.Priority != 1) {
            //     characterData.framingCam.m_ScreenX = Mathf.Lerp(characterData.framingCam.m_ScreenX, 0.5f + Vector3.Dot(characterData.playerData.wallNormal, -characterData.viewRight) / 3f, Time.deltaTime * 2f);
            // } else {
            //     characterData.framingCam.m_ScreenX = Mathf.Lerp(characterData.framingCam.m_ScreenX, 0.5f, Time.deltaTime);
            // }

            // push character from center

            Ray ray = new Ray(characterData.cam.transform.position, characterData.viewForward); // TODO:
            RaycastHit hit;

            // if (Physics.SphereCast(ray, .05f, out hit, characterData.framingCam.m_CameraDistance, LayerMask.GetMask (new string[] { "Player" }))) {
            // if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask (new string[] { "Player" }))) {
                
                
            //     cameraShift = cam.transform.InverseTransformDirection(hit.normal); // procedural off center camera stuff
            //     cameraShift.z = 0f;
            //     float cameraShiftMag = Mathf.Abs(cameraShift.x);
            //     cameraShift.y = 1f - cameraShiftMag;

            //     // cameraShift = cameraShift.normalized;
            //     // Debug.Log(cameraShift);
                
            //     // characterData.framingCam.m_ScreenX = Mathf.Lerp(characterData.framingCam.m_ScreenX, 0.5f + cameraShift.normalized, Time.deltaTime * 2f);
            //     // characterData.framingCam.m_ScreenY = Mathf.Lerp(characterData.framingCam.m_ScreenY, 0.5f + Vector3.Project(cameraShift.normalized, -viewUp).magnitude / 3f, Time.deltaTime);
                

            // }

            // characterData.framingCam.m_TrackedObjectOffset = Vector3.Lerp(characterData.framingCam.m_TrackedObjectOffset, cameraShift * 1.5f, Time.deltaTime * 4f); // jitter

            // fov changes with speed

            float fov = Mathf.Lerp(90f, 60f, Mathf.Clamp01(characterData.moveData.velocity.magnitude / 15f));
            // virtualFramingCam.m_Lens.FieldOfView = Mathf.Lerp(virtualFramingCam.m_Lens.FieldOfView, 90f, Time.deltaTime * 2f);

            // if (characterData.playerData.mainTarget.position != Vector3.zero) {

            //     float cameraSideDamp = 2f;
            //     float toTargetCS = cam.WorldToViewportPoint(characterData.playerData.mainTarget.position).x - followCam.CameraSide;
            //     followCam.CameraSide += toTargetCS * Time.deltaTime * cameraSideDamp;

            // } else {

            //     float targetCS = 1f;

            //     // if (Vector3.Dot(velocityForward, viewRight) > .5f) targetCS = 0f;
            //     // if (Vector3.Dot(velocityForward, -viewRight) > .5f) targetCS = 1f;


            //     float cameraSideDamp = 1f;
            //     float toTargetCS = targetCS - followCam.CameraSide;
            //     followCam.CameraSide += toTargetCS * Time.deltaTime * cameraSideDamp;


            // }


            // no jerking camera motions
            
            characterData.framingCam.m_DeadZoneDepth = 0f;
            characterData.framingCam.m_DeadZoneHeight = 0f;
            characterData.framingCam.m_DeadZoneWidth = 0f;
            characterData.framingCam.m_LookaheadTime = 0f;

            // toggling between first and third person

            if (characterData.firstPersonCam.Priority == 1) {
                // characterData.framingCam.m_CameraDistance = Mathf.Lerp(characterData.framingCam.m_CameraDistance, 0f, Time.deltaTime*2f);
            } else {
                // characterData.framingCam.m_CameraDistance = Mathf.Lerp(characterData.framingCam.m_CameraDistance, Mathf.Max(Vector3.Dot(characterData.moveData.velocity, characterData.viewForward) / 4f, 2.5f), Time.deltaTime * 2f);
                // characterData.followCam.CameraDistance = Mathf.Lerp(characterData.followCam.CameraDistance, Mathf.Max(characterData.moveData.velocity.magnitude / 4f, 2.5f), Time.deltaTime * 2f);
                // characterData.followCam.ShoulderOffset.x = characterData.followCam.CameraDistance;
            }

            // rule of thirds camera framing, clamp at .666

            characterData.framingCam.m_SoftZoneHeight = Mathf.Lerp(characterData.framingCam.m_SoftZoneHeight, .5f, Time.deltaTime * 4f);
            characterData.framingCam.m_SoftZoneWidth = Mathf.Lerp(characterData.framingCam.m_SoftZoneWidth, .5f, Time.deltaTime * 4f);
            // characterData.framingCam.m_DeadZoneWidth = Mathf.Lerp(characterData.framingCam.m_DeadZoneWidth, .333f, Time.deltaTime * 4f);

            // no z damping, some xy damping
            
            characterData.framingCam.m_XDamping = Mathf.Lerp(characterData.framingCam.m_XDamping, 1f, Time.deltaTime * 4f);
            characterData.framingCam.m_YDamping = Mathf.Lerp(characterData.framingCam.m_YDamping, 1f, Time.deltaTime * 4f);
            characterData.framingCam.m_ZDamping = Mathf.Lerp(characterData.framingCam.m_ZDamping, 0f, Time.deltaTime * 4f);
            
            
            //focusAimBlend = Mathf.Lerp(focusAimBlend, .5f, Time.deltaTime * 8f);

            //aimCam.m_Damping = Mathf.Lerp(aimCam.m_Damping, 0f, Time.deltaTime * 2f);
            // aimCam.m_Damping = Mathf.Lerp(aimCam.m_Damping, Mathf.Clamp01(moveData.velocity.magnitude / moveConfig.runSpeed) * stability, Time.deltaTime * 2f);

            // characterData.framingCam.m_DeadZoneDepth = Mathf.Lerp(characterData.framingCam.m_DeadZoneDepth, 0f, Time.deltaTime * 4f);

        }


    }

    

}