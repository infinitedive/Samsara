using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {

    // public class SkateCharacter {
        
        // public void HandleKnockback(PlayerCharacter target, Vector3 direction, float knockbackAmount) {

        //     target.moveData.velocity += direction * knockbackAmount;

        // }

        // public void HandleDamage(PlayerCharacter target, float damageAmount) {

        //     // moveData.velocity += direction * knockbackAmount;

        // }

        // public void HandleStagger(PlayerCharacter target, float duration) {

        //     // moveData.velocity += direction * knockbackAmount;

        // }

        // IEnumerator ActivateTrail(float timeActive) {

        //     while (timeActive > 0) {
        //         timeActive -= meshRefreshRate;

        //         if (skinnedMeshRenderers == null) {
        //             skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        //         }

        //         for( int i=0; i<skinnedMeshRenderers.Length; i++) {
        //             GameObject gObj = new GameObject();
        //             gObj.transform.position = moveData.origin - Vector3.up;
        //             gObj.transform.rotation = avatarLookRotation;

        //             MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
        //             MeshFilter mf = gObj.AddComponent<MeshFilter>();

        //             Mesh mesh = new Mesh();
        //             skinnedMeshRenderers[i].BakeMesh(mesh);

        //             mf.mesh = mesh;
        //             mr.material = mat;

        //             Destroy(gObj, 2f);
        //         }

        //         yield return new WaitForSeconds(meshRefreshRate);
        //     }

        // }

        // IEnumerator FireballRoutine(Transform target) {

        //     float timer = 0f;
        //     while (timer < 2f)
        //     {
        //         // transform.position += movementPerSecond * Time.deltaTime;
        //         Vector3 toTarget = target.position - fireball.transform.position;
        //         fireball.transform.position += toTarget * Time.deltaTime;
        //         timer += Time.deltaTime;
        //         yield return null;
        //     }

        //     Destroy(fireball);

        // }

        // private void Update () {

        //     debug.text = String.Format("Aim Assist Blend: {0}\nCurrent Super State: {1}\nCurrent Sub State: {2}\nSpeed: {3}\nTargets: {4}", focusAimBlend, currentState?.name, currentState?.currentSubState?.name, Mathf.Floor(moveData.velocity.magnitude), targetLength);
        //     squash = Vector3.zero;

        //     // Check Ground For State >> Update position >> Resolve Collisions >> Update Next State >> Update Next Rotation

        //     DecrementTimers();
        //     FindTargets();

        //     Vector3 positionalMovement = transform.position - prevPosition; // TODO: 
        //     transform.position = prevPosition;
        //     moveData.origin += positionalMovement;

        //     ClampVelocity();
        //     ResolveCollisions();
        //     DoBlendAnimations();
        //     CheckGrapplePress(); 
        //     CameraSettings();

        //     // Dive should be renamed to Skate
            
        //     currentState.UpdateStates();

        //     float x, y, z;

        //     x = Mathf.Abs(moveData.velocity.normalized.x) * 2f;
        //     y = Mathf.Abs(moveData.velocity.normalized.y) * 2f;
        //     z = 2f + moveData.velocity.magnitude / 5f;

        //     if (preUpdateEnvironmentForces != Vector3.zero) {
        //         moveData.velocity += preUpdateEnvironmentForces;
        //         // squash += -preUpdateEnvironmentForces;
        //         preUpdateEnvironmentForces = Vector3.zero;
        //     }


        //     transform.position = moveData.origin;
        //     prevPosition = transform.position;

        //     moveData.flatVelocity = Vector3.ProjectOnPlane(moveData.velocity, groundNormal);

        //     // if (playerData.wishJumpUp) {
        //     //     StartCoroutine(ActivateTrail(2f));
        //     // }

        //     // rightHandEffector.right = avatarLookForward;

        //     // if (rightHandEffector.eulerAngles.x < 0f) {
        //     //     rightHandEffector.eulerAngles = Vector3.Scale(rightHandEffector.eulerAngles, new Vector3(-1f, 1f, 1f));
        //     // }

        //     // rightHandEffector.position = transform.position + avatarLookRight * .5f;

        //     AimAssist();

        //     TransformRotation();

        //     zVel = Vector3.Dot(moveData.velocity, bodyForward);
        //     yVel = Vector3.Dot(moveData.velocity, bodyUp);

        //     Vector3 speedBallSquash = Vector3.zero;

        //     if (moveData.velocity != Vector3.zero) speedBall.transform.forward = moveData.velocity.normalized;
    
        //     speedBallSquash = speedBall.transform.InverseTransformVector(squash); // squash direction in local

        //     // if (speedBallSquash != Vector3.zero) Debug.Log(speedBallSquash);

        //     squash = Vector3.Lerp(squash, Vector3.zero, Time.deltaTime / 8f);

        //     float squashY = Vector3.Dot(speedBallSquash, Vector3.up) / 2f;

        //     speedBall.transform.localScale = Vector3.Lerp(speedBall.transform.localScale, new Vector3(2f, 2f - squashY, z), Time.deltaTime * 8f);

        //     leftSide = Vector3.zero;
        //     rightSide = Vector3.zero;
        //     backSide = Vector3.zero;
        //     frontSide = Vector3.zero;

        //     playerData.wishJumpUp = false;
        //     playerData.wishRunUp = false;
        //     playerData.wishTumbleUp = false;
        //     playerData.wishFireUp = false;
        //     playerData.wishFirePress = false;
        //     playerData.wishGrappleUp = false;
        //     playerData.wishGrapplePress = false;
        //     playerData.wishAimPress = false;
        //     playerData.wishAimUp = false;
        //     playerData.wishDashPress = false;
        //     playerData.wishDashUp = false;

        //     if (playerData.wishEscapeDown) {
        //         Application.Quit();
        //     }

        //     focusAimBlend = Mathf.Lerp(focusAimBlend, .5f, Time.deltaTime * 2f);


        // }

        // public void Fireball() {


        //     fireball = Instantiate(_fireball, avatarLookTransform.position + avatarLookForward*2f, Quaternion.identity);

        // //     }

        // //     grapple.playerCollider = innerCollider;
        // //     Physics.IgnoreCollision(grapple.gameObject.GetComponent<SphereCollider>(), grapple.playerCollider, true);
                
        //     fireball.gameObject.SetActive(true);

        //     float launchSpeed = 10f;

        // //     // Debug.Log(Vector3.Dot(moveData.velocity, viewForward) * viewForward);
            
        //     fireball.transform.forward = viewForward;

        //     // StartCoroutine(FireballRoutine(playerData.mainTarget));

        // }

        // private void FindTargets() {

        //     playerData.targetMean = Vector3.zero;
        //     playerData.mainTarget.position = Vector3.zero;

        //     string[] mask = new string[] { "Ground", "Ball", "Enemy" };

        //     Vector3 castPos = avatarLookTransform.position;
        //     playerData.detectedTargets = new List<Collider>();

        //     for (float i = 1f; i <= moveConfig.maxDistance; i++) {

        //         castPos = avatarLookTransform.position + avatarLookForward * i / 2f;

        //         Ray r = new Ray(castPos, avatarLookForward);
        //         RaycastHit[] hits = Physics.SphereCastAll (
        //             ray: r,
        //             radius: i / 2f,
        //             maxDistance: moveConfig.maxDistance - i,
        //             layerMask: LayerMask.GetMask (mask),
        //             queryTriggerInteraction: QueryTriggerInteraction.Ignore);

        //         foreach (RaycastHit hit in hits)
        //         {

        //             if (!playerData.detectedTargets.Contains(hit.collider)) {


        //                 // playerData.focusPoint = hit.point;
        //                 // playerData.focusDir = (playerData.focusPoint - moveData.origin).normalized;
        //                 // playerData.distanceFromFocus = (playerData.focusPoint - moveData.origin).magnitude;
        //                 // playerData.focusNormal = hit.normal;

        //                 if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {

        //                     playerData.detectedTargets.Add(hit.collider);

        //                     bool closest = (playerData.mainTarget.position - avatarLookTransform.position).magnitude > (hit.collider.transform.position - avatarLookTransform.position).magnitude;

        //                     bool fromCenter = Vector3.Dot(playerData.mainTarget.position, viewForward) < Vector3.Dot(hit.collider.transform.position, viewForward);

        //                     playerData.mainTarget.position = hit.collider.transform.position;
                            
        //                     // if (playerData.mainTarget.position != Vector3.zero) {
        //                         // playerData.mainTarget.position = hit.collider.transform.position;
        //                     // } else {
        //                     //     playerData.mainTarget.position = lookAtThis.position;
        //                     // }
        //                     // else if (closest && fromCenter) {
        //                     //     playerData.mainTarget.position = hit.collider.transform.position;
        //                     // }

        //                 }

        //                 // playerData.targetDir = (playerData.mainTargetPoint - avatarLookTransform.position).normalized;
        //                 // playerData.distanceFromTarget = (playerData.mainTargetPoint - avatarLookTransform.position).magnitude;
        //                 // currentTarget = hit.collider;
        //             }

        //             // if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {
        //             //     playerData.targetPoint = hit.collider.transform.position;
        //             //     playerData.targetDir = (playerData.targetPoint - moveData.origin).normalized;
        //             //     playerData.distanceFromTarget = (playerData.targetPoint - moveData.origin).magnitude;
        //             //     currentTarget = hit.collider;
        //             // } else {
        //             //     currentTarget = null;
        //             //     playerData.targetPoint = Vector3.zero;
        //             //     playerData.targetDir = Vector3.zero;
        //             //     playerData.distanceFromTarget = 0f;
        //             // }

        //             // break;
        //         } 
        //         // else {
        //         //     playerData.focusPoint = avatarLookTransform.position + avatarLookForward * moveConfig.maxDistance / 2f;
        //         // }

        //     }

        //     targetLength = playerData.detectedTargets.Count;


        //     if (playerData.mainTarget.position != Vector3.zero) {
        //         reticle.position = cam.WorldToScreenPoint(playerData.mainTarget.position);
        //     } else {
        //         reticle.anchoredPosition = new Vector2(0f, 0f);
        //         // playerData.mainTarget = lookAtThis;
        //         // reticle.position = cam.WorldToScreenPoint(playerData.mainTarget.position);
        //     }



        // }

        // quantum console syntax
        // [Command("float-prop")] // 
        // protected static float SetAimAssist { get; private set;}

        

        // public Quaternion FlatLookRotation(Vector3 forward) {
        //     return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, groundNormal).normalized, groundNormal);
        // }

        // public Quaternion FlatLookRotation(Vector3 forward, Vector3 normal) {
        //     return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, normal).normalized, normal);
        // }

        // private void DoBlendAnimations() {

        //     zVel = Vector3.Dot(Vector3.ProjectOnPlane(moveData.velocity, groundNormal), bodyForward);

        //     xVel = Vector3.Dot(Vector3.ProjectOnPlane(moveData.velocity, groundNormal), bodyRight);

            

        //     var turningDelta = xMovement * 30f;

        //     if (!playerData.grounded) {
        //         // smoke.SetVector3("position", moveData.origin - groundNormal);
        //         // smoke.SetFloat("force", moveData.velocity.magnitude / 10f);
        //         // smoke.SetFloat("spawnRate", 32f + moveData.velocity.magnitude);


        //         if (moveData.velocity.magnitude > moveConfig.walkSpeed) {

        //             if (playerData.grounded) {
        //                 // smoke.SetVector3("direction", -playerData.xzWishMove);
        //             } else {
        //                 // smoke.SetVector3("direction", -playerData.wishMove);
        //             }

        //         } else {
        //             // smoke.SetVector3("direction", Vector3.zero);
        //             // smoke.SetFloat("force", 0f);
        //         }

        //         if (playerData.wishTumbleDown && playerData.grounded && moveData.velocity.magnitude > moveConfig.walkSpeed) {
        //             // smoke.SetVector3("direction", moveData.velocity.normalized);
        //             // smoke.SetFloat("force", moveData.velocity.magnitude / 10f);

        //             xVel = Mathf.Lerp(xVel, -xVel, .3f);
        //             zVel = Mathf.Lerp(zVel, -zVel, .3f);
        //         }

        //     } else {

        //     }

        //     animator.SetFloat("xVel", xVel);
        //     animator.SetFloat("yVel", zVel);
        //     animator.SetFloat("turningDelta", turningDelta);

        //     // animator.SetBool("ChargePress", playerData.wishCrouchDown);
        //     // animator.SetBool("onGround", playerData.grounded);
            
        // }

        // private void CheckGrapplePress() {

        //     if (playerData.wishGrapplePress && !playerData.grappling) {
        //         ConnectGrapple(playerData.focusPoint);
                
        //     }


        // }

        // private void LateUpdate() {
        //     DrawRope();

        //     // ik.solver.axis = ik.solver.transform.InverseTransformVector(ik.transform.rotation * avatarLookForward);
        // }

        // public Vector3 CenteredSlerp(Vector3 start, Vector3 end, Vector3 centerPivot, float t) {

        //     Vector3 startRelativeCenter = start - centerPivot;
        //     Vector3 endRelativeCenter = end - centerPivot;

        //     return Vector3.Slerp(startRelativeCenter, endRelativeCenter, t) + centerPivot;
        // }

        // private void AimAssist() {

        //     string[] mask = new string[] { "Ground", "Ball", "Enemy" };

        //     Vector3 castPos = avatarLookTransform.position;

        //     RaycastHit hit;

        //     if (!playerData.attacking && !playerData.grappling) { // aim assist stuff

        //         for (float i = 1f; i <= moveConfig.maxDistance; i++) {

        //             castPos = avatarLookTransform.position + avatarLookForward * i / 2f;

        //             Ray r = new Ray(castPos, avatarLookForward);

        //             if (Physics.SphereCast (
        //                 ray: r,
        //                 radius: i / 2f,
        //                 hitInfo: out hit,
        //                 maxDistance: moveConfig.maxDistance - i,
        //                 layerMask: LayerMask.GetMask (mask),
        //                 queryTriggerInteraction: QueryTriggerInteraction.Ignore))
        //             {
                        
        //                 playerData.focusPoint = hit.point;
        //                 playerData.focusDir = (playerData.focusPoint - moveData.origin).normalized;
        //                 playerData.distanceFromFocus = (playerData.focusPoint - moveData.origin).magnitude;
        //                 playerData.focusNormal = hit.normal;

        //                 if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Enemy") {
        //                     playerData.mainTarget.position = hit.collider.transform.position;
        //                     playerData.targetDir = (playerData.mainTarget.position - moveData.origin).normalized;
        //                     playerData.distanceFromTarget = (playerData.mainTarget.position - moveData.origin).magnitude;
        //                     currentTarget = hit.collider;
        //                 } else {
        //                     currentTarget = null;
        //                     playerData.mainTarget.position = Vector3.zero;
        //                     playerData.targetDir = Vector3.zero;
        //                     playerData.distanceFromTarget = 0f;
        //                 }

        //                 break;
        //             } else {
        //                 playerData.focusPoint = avatarLookTransform.position + avatarLookForward * moveConfig.maxDistance / 2f;
        //             }

        //         }

        //     }

        // }

        // private void TransformRotation() {

        //     // AimAssist();

        //     focusOnThis.position = Vector3.Lerp(focusOnThis.position, playerData.focusPoint, Time.deltaTime * 10f);
            
        //     Vector3 combinedLookPosition = lookAtThis.position;
        //     Quaternion combinedLookRotation = Quaternion.LookRotation((combinedLookPosition - avatarLookTransform.position).normalized, groundNormal);

        //     if (moveData.velocity.magnitude > moveConfig.walkSpeed) { // TODO: make bodyTransform not this transform
        //         avatarLookRotation = Quaternion.Slerp(avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
        //         bodyRotation = Quaternion.Slerp(bodyRotation, FlatLookRotation(viewForward), Time.deltaTime * 5f);
        //         // bodyRotation = Quaternion.Slerp(bodyRotation, Quaternion.LookRotation(viewForward), Time.deltaTime * 5f);
        //         velocityRotation = Quaternion.LookRotation(moveData.velocity);
        //     } else {
        //         avatarLookRotation = Quaternion.Slerp(avatarLookRotation, combinedLookRotation, Time.deltaTime * 20f);
                
        //         if (firstPersonCam.Priority == 1) {
        //             avatarLookRotation = combinedLookRotation;
        //             bodyRotation = FlatLookRotation(avatarLookForward);
        //         } else {

        //             bodyRotation = Quaternion.Slerp(bodyRotation, FlatLookRotation(viewForward), Time.deltaTime * 5f);
                    
        //         }
        //         velocityRotation = bodyRotation;
        //     }

        //     // 28 46

        //     // Vector2 differenceInPixels = cam.WorldToScreenPoint(lookAtThis.position) - cam.WorldToScreenPoint(playerData.focusPoint);

        //     if (firstPersonCam.Priority == 1) {
        //         playerData.xAimDamp = Mathf.Lerp(playerData.xAimDamp, .5f, Time.deltaTime * 2f);
        //         playerData.yAimDamp = Mathf.Lerp(playerData.xAimDamp, .5f, Time.deltaTime * 2f);

                

        //     } else {
        //         playerData.xAimDamp = Mathf.Lerp(playerData.xAimDamp, .9f, Time.deltaTime * 2f);
        //         playerData.yAimDamp = Mathf.Lerp(playerData.yAimDamp, .9f, Time.deltaTime * 2f);

        //     }

        //     xMovement = Mathf.Clamp(playerData.mouseDelta.x * moveConfig.horizontalSensitivity * moveConfig.sensitivityMultiplier * playerData.xAimDamp, -2.5f, 2.5f);
        //     yMovement = Mathf.Clamp(-playerData.mouseDelta.y * moveConfig.verticalSensitivity  * moveConfig.sensitivityMultiplier * playerData.yAimDamp, -2.5f, 2.5f);

        //     // aim influence / virtual mouse

        //     // if (!(cam.WorldToViewportPoint(playerData.mainTarget.position).z < 0f)) {

        //     //     Vector3 toReticle = reticle.anchoredPosition.normalized;

        //     // }
            
        //     if (playerData.wishFireDown && playerData.mainTarget.position != Vector3.zero) { // hard lock on

        //         toTarget = reticle.anchoredPosition.normalized;

        //         xMovement += toTarget.x;
        //         yMovement += toTarget.y;

        //         xMovement = Mathf.Clamp(xMovement, -2f, 2f);
        //         yMovement = Mathf.Clamp(yMovement, -2f, 2f);
                
        //         viewTransformLookAt.x = Mathf.Clamp(viewTransformLookAt.x + yMovement, moveConfig.minYRotation, moveConfig.maxYRotation);
        //         viewTransformLookAt.y = viewTransformLookAt.y + xMovement;

        //         Vector3 influence = avatarLookRotation * new Vector3(Mathf.Clamp(xMovement, -5f, 5f), Mathf.Clamp(-yMovement, -5f, 5f), 0f);

                
        //         viewRotation = 
        //             Quaternion.AngleAxis(viewTransformLookAt.y, Vector3.up) *
        //             Quaternion.AngleAxis(viewTransformLookAt.z, Vector3.forward) *
        //             Quaternion.AngleAxis(viewTransformLookAt.x, Vector3.right);

        //         Vector3 vanishingPoint = cam.transform.position + cam.transform.forward * 20f;

        //         lookAtThis.position += (playerData.mainTarget.position - lookAtThis.position + influence) * Time.deltaTime * 5f;

        //     } else {

        //         viewTransformLookAt.x = Mathf.Clamp(viewTransformLookAt.x + yMovement, moveConfig.minYRotation, moveConfig.maxYRotation);
        //         viewTransformLookAt.y = viewTransformLookAt.y + xMovement;

        //         viewRotation = 
        //             Quaternion.AngleAxis(viewTransformLookAt.y, Vector3.up) *
        //             Quaternion.AngleAxis(viewTransformLookAt.z, Vector3.forward) *
        //             Quaternion.AngleAxis(viewTransformLookAt.x, Vector3.right);

        //         Vector3 vanishingPoint = avatarLookTransform.position + cam.transform.forward * 20f;

        //         lookAtThis.position = vanishingPoint;

        //     }

        //     // _vcam.SetActive(true);

        //     if (playerData.wishAimDown) {
        //         firstPersonCam.Priority = 1;
        //         // virtualFramingCam.Priority = 0;
        //         thirdPersonCam.Priority = 0;

        //         dither = Mathf.Lerp(dither, 1f, Time.deltaTime * 4f);

        //         for (int i = 0; i < characterMaterials.Length; i++) 
        //         {
        //             characterMaterials[i].SetFloat("_dither", dither);
        //         }

        //         Color color = cloakMat.color;
        //         color.a = 0f;
        //         cloakMat.color = Color.Lerp(cloakMat.color, color, Time.deltaTime * 2f);

        //         // cam.cullingMask &= ~(1 << LayerMask.NameToLayer("FirstPersonCull"));


        //     } else {
        //         firstPersonCam.Priority = 0;
        //         // virtualFramingCam.Priority = 1;
        //         thirdPersonCam.Priority = 1;

        //         dither = Mathf.Lerp(dither, 0f, Time.deltaTime * 4f);

        //         for (int i = 0; i < characterMaterials.Length; i++) 
        //         {
        //             characterMaterials[i].SetFloat("_dither", dither);
        //         }

        //         Color color = cloakMat.color;
        //         color.a = 1f;
        //         cloakMat.color = Color.Lerp(cloakMat.color, color, Time.deltaTime * 2f);

        //         // cam.cullingMask |= (1 << LayerMask.NameToLayer("FirstPersonCull"));
        //     }
            
        // }

        // private void CameraSettings() { // TODO:

        //     framingCam.m_UnlimitedSoftZone = false;

        //     if (playerData.detectWall && firstPersonCam.Priority != 1) {
        //         framingCam.m_ScreenX = Mathf.Lerp(framingCam.m_ScreenX, 0.5f + Vector3.Dot(playerData.wallNormal, -viewRight) / 3f, Time.deltaTime * 2f);
        //     } else {
        //         framingCam.m_ScreenX = Mathf.Lerp(framingCam.m_ScreenX, 0.5f, Time.deltaTime);
        //     }

        //     // push character from center

        //     Ray ray = new Ray(cam.transform.position, viewForward); // TODO:
        //     RaycastHit hit;

        //     // if (Physics.SphereCast(ray, .05f, out hit, framingCam.m_CameraDistance, LayerMask.GetMask (new string[] { "Player" }))) {
        //     // if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask (new string[] { "Player" }))) {
                
                
        //     //     cameraShift = cam.transform.InverseTransformDirection(hit.normal); // procedural off center camera stuff
        //     //     cameraShift.z = 0f;
        //     //     float cameraShiftMag = Mathf.Abs(cameraShift.x);
        //     //     cameraShift.y = 1f - cameraShiftMag;

        //     //     // cameraShift = cameraShift.normalized;
        //     //     // Debug.Log(cameraShift);
                
        //     //     // framingCam.m_ScreenX = Mathf.Lerp(framingCam.m_ScreenX, 0.5f + cameraShift.normalized, Time.deltaTime * 2f);
        //     //     // framingCam.m_ScreenY = Mathf.Lerp(framingCam.m_ScreenY, 0.5f + Vector3.Project(cameraShift.normalized, -viewUp).magnitude / 3f, Time.deltaTime);
                

        //     // }

        //     framingCam.m_TrackedObjectOffset = Vector3.Lerp(framingCam.m_TrackedObjectOffset, cameraShift * 1.5f, Time.deltaTime * 4f); // jitter

        //     // fov changes with speed

        //     float fov = Mathf.Lerp(90f, 60f, Mathf.Clamp01(moveData.velocity.magnitude / 15f));
        //     // virtualFramingCam.m_Lens.FieldOfView = Mathf.Lerp(virtualFramingCam.m_Lens.FieldOfView, 90f, Time.deltaTime * 2f);

        //     // if (playerData.mainTarget.position != Vector3.zero) {

        //     //     float cameraSideDamp = 2f;
        //     //     float toTargetCS = cam.WorldToViewportPoint(playerData.mainTarget.position).x - followCam.CameraSide;
        //     //     followCam.CameraSide += toTargetCS * Time.deltaTime * cameraSideDamp;

        //     // } else {

        //     //     float targetCS = 1f;

        //     //     // if (Vector3.Dot(velocityForward, viewRight) > .5f) targetCS = 0f;
        //     //     // if (Vector3.Dot(velocityForward, -viewRight) > .5f) targetCS = 1f;


        //     //     float cameraSideDamp = 1f;
        //     //     float toTargetCS = targetCS - followCam.CameraSide;
        //     //     followCam.CameraSide += toTargetCS * Time.deltaTime * cameraSideDamp;


        //     // }


        //     // no jerking camera motions
            
        //     framingCam.m_DeadZoneDepth = 0f;
        //     framingCam.m_DeadZoneHeight = 0f;
        //     framingCam.m_DeadZoneWidth = 0f;
        //     framingCam.m_LookaheadTime = 0f;

        //     // toggling between first and third person

        //     if (firstPersonCam.Priority == 1) {
        //         framingCam.m_CameraDistance = Mathf.Lerp(framingCam.m_CameraDistance, 0f, Time.deltaTime*2f);
        //     } else {
        //         framingCam.m_CameraDistance = Mathf.Lerp(framingCam.m_CameraDistance, Mathf.Max(Vector3.Dot(moveData.velocity, viewForward) / 4f, 2.5f), Time.deltaTime * 2f);
        //         followCam.CameraDistance = Mathf.Lerp(followCam.CameraDistance, Mathf.Max(moveData.velocity.magnitude / 4f, 2.5f), Time.deltaTime * 2f);
        //         followCam.ShoulderOffset.x = followCam.CameraDistance;
        //     }

        //     // rule of thirds camera framing, clamp at .666

        //     framingCam.m_SoftZoneHeight = Mathf.Lerp(framingCam.m_SoftZoneHeight, .5f, Time.deltaTime * 4f);
        //     framingCam.m_SoftZoneWidth = Mathf.Lerp(framingCam.m_SoftZoneWidth, .5f, Time.deltaTime * 4f);
        //     // framingCam.m_DeadZoneWidth = Mathf.Lerp(framingCam.m_DeadZoneWidth, .333f, Time.deltaTime * 4f);

        //     // no z damping, some xy damping
            
        //     framingCam.m_XDamping = Mathf.Lerp(framingCam.m_XDamping, 1f, Time.deltaTime * 4f);
        //     framingCam.m_YDamping = Mathf.Lerp(framingCam.m_YDamping, 1f, Time.deltaTime * 4f);
        //     framingCam.m_ZDamping = Mathf.Lerp(framingCam.m_ZDamping, 0f, Time.deltaTime * 4f);
            
            
        //     //focusAimBlend = Mathf.Lerp(focusAimBlend, .5f, Time.deltaTime * 8f);

        //     //aimCam.m_Damping = Mathf.Lerp(aimCam.m_Damping, 0f, Time.deltaTime * 2f);
        //     // aimCam.m_Damping = Mathf.Lerp(aimCam.m_Damping, Mathf.Clamp01(moveData.velocity.magnitude / moveConfig.runSpeed) * stability, Time.deltaTime * 2f);

        //     // framingCam.m_DeadZoneDepth = Mathf.Lerp(framingCam.m_DeadZoneDepth, 0f, Time.deltaTime * 4f);

        // }

        

        

        // private void ShootGrapple() {
        //     Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //     RaycastHit hit;

        //     Vector3 targetPoint;
        //     if (Physics.Raycast(ray, out hit))
        //         targetPoint = hit.point;
        //     else
        //         targetPoint = ray.GetPoint(75);

        //     if (!grapple) {

        //         grapple = Instantiate(_grapple, viewTransform.position + viewForward*2f, Quaternion.identity);

        //     }

        //     grapple.playerCollider = innerCollider;
        //     grapple.rb.useGravity = false;
        //     Physics.IgnoreCollision(grapple.gameObject.GetComponent<SphereCollider>(), grapple.playerCollider, true);
                
        //     grapple.gameObject.SetActive(true);

        //     float launchSpeed = moveConfig.maxDistance * 4f;

        //     // Debug.Log(Vector3.Dot(moveData.velocity, viewForward) * viewForward);
            
        //     grapple.transform.forward = viewForward;
        //     grapple.GetComponent<Rimovey>().velocity = viewForward * launchSpeed + Mathf.Max(Vector3.Dot(moveData.velocity, viewForward), 0f) * viewForward;
            // StartCoroutine(GrappleRoutine());
        // }

        // private void ShootGrapple(float distance) {
        //     Ray ray = new Ray(avatarLookTransform.position + avatarLookForward * moveConfig.castRadius * 2f, avatarLookForward);
        //     RaycastHit hit;

        //     if (Physics.SphereCast(ray, moveConfig.castRadius, out hit, distance, LayerMask.GetMask (new string[] { "Ground" })))
        //         ConnectGrapple(hit.point); // TODO: Startup animation
                
        //     playerData.grappleNormal = hit.normal;
        // }

        
        // private void DecrementTimers() {
        //     if (wallTouchTimer > 0f) {
        //         wallTouchTimer -= Time.deltaTime;
        //     }

        //     if (jumpTimer > 0f) {
        //         jumpTimer -= Time.deltaTime;
        //     }

        //     if (groundInputTimer > 0f) {
        //         groundInputTimer -= Time.deltaTime;
        //     }

        //     if (boostInputTimer > 0f) {
        //         boostInputTimer -= Time.deltaTime;
        //     }

        //     // if (grappleShootTimer > 0f) {
        //     //     grappleShootTimer -= Time.deltaTime;
        //     // }

        //     if (grappleZipTimer > 0f) {
        //         grappleZipTimer -= Time.deltaTime;
        //     }

        //     if (reduceGravityTimer > 0f) {
        //         reduceGravityTimer -= Time.deltaTime;
        //     }

        //     if (ignoreGravityTimer > 0f) {
        //         ignoreGravityTimer -= Time.deltaTime;
        //     }

        //     if (inputBufferTimer > 0f) {
        //         inputBufferTimer -= Time.deltaTime;
        //     }

        //     if (lungeCooldownTimer > 0f) {
        //         lungeCooldownTimer -= Time.deltaTime;
        //     }

        //     if (releaseTimer > 0f) {
        //         releaseTimer -= Time.deltaTime;
        //     }

        //     if (energySlider.value < 1f && playerData.grounded) {
        //         var meterGainSlowness = .05f;
        //         energySlider.value += (moveData.velocity.magnitude * meterGainSlowness) * Time.deltaTime;
        //     }
            
        // }
    
    // }
}
