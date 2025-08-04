using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using System;

using Game.Data;
using Game.StateMachine;
using Unity.Entities;

namespace Game.Controllers {
    public class VFXController : MonoBehaviour
    {

        private CharacterData characterData;


        [SerializeField] public GameObject grappleGun;
        [SerializeField] public GameObject smokeObj;
        [SerializeField] public GameObject smokeLandObj;
        [SerializeField] public GameObject sonicBoomObj;
        [SerializeField] public GameObject airHikeObj;
        [SerializeField] public GameObject sphereLinesObj;
        [SerializeField] public GameObject ballObj;
        
        public Volume globalVolume;
        public VisualEffect grappleArc;
        [HideInInspector] public VisualEffect slash;
        [HideInInspector] public VisualEffect smoke;
        [HideInInspector] public VisualEffect smokeLand;
        [HideInInspector] public VisualEffect sonicBoom;
        [HideInInspector] public VisualEffect airHike;
        [HideInInspector] public VisualEffect sphereLines;
        public ParticleSystem speedTrails;

        public float dither;

        protected SkinnedMeshRenderer[] skinnedMeshRenderers;
        public Material mat;

        public VisualEffect _grappleArc { get { return grappleArc; } }

        public GameObject _projectile;
        private GameObject projectile;

        /*
        Do not fear mistakes—there are none. - Miles Davis
        */

        public void Awake() {

            // grappleGun = transform.GetChild(3).transform.GetChild(1).gameObject;
            // characterData.moveConfig.grappleColor = characterData.moveConfig.normalColor;

            grappleArc = grappleGun.GetComponent<VisualEffect>();
            smoke = smokeObj.GetComponent<VisualEffect>();
            smokeLand = smokeLandObj.GetComponent<VisualEffect>();
            sonicBoom = sonicBoomObj.GetComponent<VisualEffect>();
            airHike = airHikeObj.GetComponent<VisualEffect>();
            sphereLines = sphereLinesObj.GetComponent<VisualEffect>();
    
            dither = 0f;
    
            sonicBoomObj.SetActive(true);
    
            sonicBoom.Stop();
            sphereLines.Stop();

        }

        public void GrappleVisuals() {


            _grappleArc.SetVector3("Pos0", characterData.moveData.origin + characterData.avatarLookForward);
            _grappleArc.SetVector3("Pos1", Vector3.Lerp(characterData.moveData.origin, characterData.playerData.grapplePoint, .33f));
            _grappleArc.SetVector3("Pos2", Vector3.Lerp(characterData.moveData.origin, characterData.playerData.grapplePoint, .66f));
            _grappleArc.SetVector3("Pos3", characterData.playerData.grapplePoint);
            _grappleArc.SetVector4("Color", characterData.moveConfig.grappleColor);

            DrawRope();
        }

        // if (vfxController._projectile != null)
        // {
        //     vfxController.fireball = Instantiate(vfxController._projectile, characterData.avatarLookTransform.position + characterData.avatarLookForward*2f, Quaternion.identity);
        //     vfxController.fireball.gameObject.SetActive(true);
        //     vfxController.fireball.transform.forward = characterData.viewForward;
        //     vfxController.fireball.GetComponent<MoveData>().velocity = characterData.viewForward * 10f;

        //     Debug.Log("fire");

        //     StartCoroutine(FireballRoutine(characterData.playerData.mainTarget));
        // }

        public void FireHomingProjectile() {
            // if (_projectile != null)
            // {
            //     Debug.Log(_projectile);
            //     projectile = Instantiate(_projectile, characterData.avatarLookTransform.position + characterData.avatarLookForward*2f, Quaternion.identity);
            //     projectile.gameObject.SetActive(true);
            //     projectile.transform.forward = characterData.viewForward;
    
            //     Debug.Log("fire");
    
            //     StartCoroutine(ProjectileRoutine(characterData.lookAtThis, 10f));
            // }
        }

        IEnumerator<CharacterController> ProjectileRoutine(Transform target, float speed) {
    
            Vector3 v = projectile.GetComponent<MoveData>().velocity;
            projectile.GetComponent<MoveData>().velocity = characterData.avatarLookForward * 10f;
    
            float timer = 0f;
            while (timer < 2f)
            {
                Vector3 toTarget = target.position - projectile.transform.position;
                target.position += toTarget * speed * Time.deltaTime;
                // projectile.GetComponent<MoveData>().velocity = Vector3.Slerp(projectile.GetComponent<MoveData>().velocity, toTarget, Time.deltaTime * 2f);
                timer += Time.deltaTime;
                yield return null;
            }
    
            Destroy(projectile);
    
        }

        public void DrawRope() {
    
            if (!characterData.playerData.grappling) return;
    
            var lr = grappleGun.GetComponent<LineRenderer>();
    
            lr.positionCount = 2;
    
            lr.useWorldSpace = true;
    
            lr.SetPosition(0, grappleGun.transform.position);
            lr.SetPosition(1, characterData.playerData.focusPoint);
    
            lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);
            
            grappleArc.enabled = true;
        }

        IEnumerator ActivateTrail(float timeActive) {

            while (timeActive > 0) {
                timeActive -= .1f;

                if (skinnedMeshRenderers == null) {
                    skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                }

                for( int i=0; i<skinnedMeshRenderers.Length; i++) {
                    GameObject gObj = new GameObject();
                    gObj.transform.position = characterData.moveData.origin - Vector3.up;
                    gObj.transform.rotation = characterData.avatarLookRotation;

                    MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                    MeshFilter mf = gObj.AddComponent<MeshFilter>();

                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);

                    mf.mesh = mesh;
                    mr.material = mat;

                    Destroy(gObj, 2f);
                }

                yield return new WaitForSeconds(.1f);
            }

        }


    }

}