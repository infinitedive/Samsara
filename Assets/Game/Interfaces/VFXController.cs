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