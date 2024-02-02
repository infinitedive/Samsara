using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class TargettingHandler : MonoBehaviour
    {

        CharacterData characterData;
        [HideInInspector] public int targetLength;
        public RectTransform reticle;

        public void Awake() {

            characterData = GetComponent<CharacterData>();

        }


        public void AimAssist() {

            string[] mask = new string[] { "Ground", "Ball", "Enemy" };

            Vector3 castPos = characterData.avatarLookTransform.position;

            RaycastHit hit;

            if (!characterData.playerData.attacking && !characterData.playerData.grappling) { // aim assist stuff

                for (float i = 1f; i <= characterData.moveConfig.maxDistance; i++) {

                    castPos = characterData.avatarLookTransform.position + characterData.avatarLookForward * i / 2f;

                    Ray r = new Ray(castPos, characterData.avatarLookForward);

                    if (Physics.SphereCast (
                        ray: r,
                        radius: i / 2f,
                        hitInfo: out hit,
                        maxDistance: characterData.moveConfig.maxDistance - i,
                        layerMask: LayerMask.GetMask (mask),
                        queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                    {
                        
                        characterData.playerData.focusPoint = hit.point;
                        characterData.playerData.focusDir = (characterData.playerData.focusPoint - characterData.moveData.origin).normalized;
                        characterData.playerData.distanceFromFocus = (characterData.playerData.focusPoint - characterData.moveData.origin).magnitude;
                        characterData.playerData.focusNormal = hit.normal;

                        if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {
                            characterData.playerData.mainTarget.position = hit.collider.transform.position;
                            characterData.playerData.targetDir = (characterData.playerData.mainTarget.position - characterData.moveData.origin).normalized;
                            characterData.playerData.distanceFromTarget = (characterData.playerData.mainTarget.position - characterData.moveData.origin).magnitude;
                            characterData.currentTarget = hit.collider;
                        } else {
                            characterData.currentTarget = null;
                            characterData.playerData.mainTarget.position = Vector3.zero;
                            characterData.playerData.targetDir = Vector3.zero;
                            characterData.playerData.distanceFromTarget = 0f;
                        }

                        break;
                    } else {
                        characterData.playerData.focusPoint = characterData.avatarLookTransform.position + characterData.avatarLookForward * characterData.moveConfig.maxDistance / 2f;
                    }

                }

            }

        }

        public void FindTargets() {

            characterData.playerData.targetMean = Vector3.zero;
            characterData.playerData.mainTarget.position = Vector3.zero;

            string[] mask = new string[] { "Ground", "Ball", "Enemy" };

            Vector3 castPos = characterData.avatarLookTransform.position;
            characterData.playerData.detectedTargets = new List<Collider>();

            for (float i = 1f; i <= characterData.moveConfig.maxDistance; i++) {

                castPos = characterData.avatarLookTransform.position + characterData.avatarLookForward * i / 2f;

                Ray r = new Ray(castPos, characterData.avatarLookForward);
                RaycastHit[] hits = Physics.SphereCastAll (
                    ray: r,
                    radius: i / 2f,
                    maxDistance: characterData.moveConfig.maxDistance - i,
                    layerMask: LayerMask.GetMask (mask),
                    queryTriggerInteraction: QueryTriggerInteraction.Ignore);

                foreach (RaycastHit hit in hits)
                {

                    if (!characterData.playerData.detectedTargets.Contains(hit.collider)) {


                        // characterData.playerData.focusPoint = hit.point;
                        // characterData.playerData.focusDir = (characterData.playerData.focusPoint - moveData.origin).normalized;
                        // characterData.playerData.distanceFromFocus = (characterData.playerData.focusPoint - moveData.origin).magnitude;
                        // characterData.playerData.focusNormal = hit.normal;

                        if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {

                            characterData.playerData.detectedTargets.Add(hit.collider);

                            bool closest = (characterData.playerData.mainTarget.position - characterData.avatarLookTransform.position).magnitude > (hit.collider.transform.position - characterData.avatarLookTransform.position).magnitude;

                            bool fromCenter = Vector3.Dot(characterData.playerData.mainTarget.position, characterData.viewForward) < Vector3.Dot(hit.collider.transform.position, characterData.viewForward);

                            characterData.playerData.mainTarget.position = hit.collider.transform.position;
                            
                            // if (characterData.playerData.mainTarget.position != Vector3.zero) {
                                // characterData.playerData.mainTarget.position = hit.collider.transform.position;
                            // } else {
                            //     characterData.playerData.mainTarget.position = characterData.lookAtThis.position;
                            // }
                            // else if (closest && fromCenter) {
                            //     characterData.playerData.mainTarget.position = hit.collider.transform.position;
                            // }

                        }

                        // characterData.playerData.targetDir = (characterData.playerData.mainTargetPoint - characterData.avatarLookTransform.position).normalized;
                        // characterData.playerData.distanceFromTarget = (characterData.playerData.mainTargetPoint - characterData.avatarLookTransform.position).magnitude;
                        // currentTarget = hit.collider;
                    }

                    // if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {
                    //     characterData.playerData.targetPoint = hit.collider.transform.position;
                    //     characterData.playerData.targetDir = (characterData.playerData.targetPoint - moveData.origin).normalized;
                    //     characterData.playerData.distanceFromTarget = (characterData.playerData.targetPoint - moveData.origin).magnitude;
                    //     currentTarget = hit.collider;
                    // } else {
                    //     currentTarget = null;
                    //     characterData.playerData.targetPoint = Vector3.zero;
                    //     characterData.playerData.targetDir = Vector3.zero;
                    //     characterData.playerData.distanceFromTarget = 0f;
                    // }

                    // break;
                } 
                // else {
                //     characterData.playerData.focusPoint = characterData.avatarLookTransform.position + characterData.avatarLookForward * characterData.moveConfig.maxDistance / 2f;
                // }

            }

            targetLength = characterData.playerData.detectedTargets.Count;


            if (characterData.playerData.mainTarget.position != Vector3.zero) {
                reticle.position = characterData.cam.WorldToScreenPoint(characterData.playerData.mainTarget.position);
            } else {
                reticle.anchoredPosition = new Vector2(0f, 0f);
                // characterData.playerData.mainTarget = characterData.lookAtThis;
                // reticle.position = cameraController.camera.WorldToScreenPoint(characterData.playerData.mainTarget.position);
            }



        }

    }

}