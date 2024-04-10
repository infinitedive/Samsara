using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.VFX;
using System;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class VFXController : MonoBehaviour
    {

        private CharacterData characterData;

        public GameObject _fireball;
        public GameObject fireball;

        [SerializeField] public GameObject grappleGun;
        [SerializeField] public GameObject smokeObj;
        [SerializeField] public GameObject smokeLandObj;
        [SerializeField] public GameObject sonicBoomObj;
        [SerializeField] public GameObject airHikeObj;
        [SerializeField] public GameObject sphereLinesObj;
        [SerializeField] public GameObject ballObj;
        
        public Volume globalVolume;
        [HideInInspector] public VisualEffect grappleArc;
        [HideInInspector] public VisualEffect slash;
        [HideInInspector] public VisualEffect smoke;
        [HideInInspector] public VisualEffect smokeLand;
        [HideInInspector] public VisualEffect sonicBoom;
        [HideInInspector] public VisualEffect airHike;
        [HideInInspector] public VisualEffect sphereLines;
        public ParticleSystem speedTrails;

        public float dither;
        public float meshRefreshRate = .1f;

        protected SkinnedMeshRenderer[] skinnedMeshRenderers;
        public Material mat;

        public VisualEffect _grappleArc { get { return grappleArc; } }

        public void Awake() {

            grappleGun = transform.GetChild(3).transform.GetChild(1).gameObject;

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

        public void DrawRope() {
    
            if (!characterData.playerData.grappling) return;
    
            var _lr = grappleGun.GetComponent<LineRenderer>();
    
            _lr.positionCount = 2;
    
            _lr.useWorldSpace = true;
    
            _lr.SetPosition(0, grappleGun.transform.position);
            _lr.SetPosition(1, characterData.playerData.focusPoint);
    
            _lr.materials[0].mainTextureOffset += new Vector2(-Time.deltaTime, 0f);
            
            grappleArc.enabled = true;
        }

        

        IEnumerator ActivateTrail(float timeActive) {

            while (timeActive > 0) {
                timeActive -= meshRefreshRate;

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

                yield return new WaitForSeconds(meshRefreshRate);
            }

        }


    }

}