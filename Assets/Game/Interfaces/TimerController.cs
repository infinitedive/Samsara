using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

using Game.Data;
using Game.StateMachine;

namespace Game.Controllers {
    public class TimerController : MonoBehaviour
    {

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

        public void DecrementTimers() {
            if (wallTouchTimer > 0f) {
                wallTouchTimer -= Time.deltaTime;
            }

            if (jumpTimer > 0f) {
                jumpTimer -= Time.deltaTime;
                // Debug.Log(jumpTimer);
            }

            if (groundInputTimer > 0f) {
                groundInputTimer -= Time.deltaTime;
            }

            if (boostInputTimer > 0f) {
                boostInputTimer -= Time.deltaTime;
            }

            // if (grappleShootTimer > 0f) {
            //     grappleShootTimer -= Time.deltaTime;
            // }

            if (grappleZipTimer > 0f) {
                grappleZipTimer -= Time.deltaTime;
            }

            if (reduceGravityTimer > 0f) {
                reduceGravityTimer -= Time.deltaTime;
            }

            if (ignoreGravityTimer > 0f) {
                ignoreGravityTimer -= Time.deltaTime;
            }

            if (inputBufferTimer > 0f) {
                inputBufferTimer -= Time.deltaTime;
            }

            if (lungeCooldownTimer > 0f) {
                lungeCooldownTimer -= Time.deltaTime;
            }

            if (releaseTimer > 0f) {
                releaseTimer -= Time.deltaTime;
            }

            // if (energySlider.value < 1f && characterData.playerData.grounded) {
            //     var meterGainSlowness = .05f;
            //     energySlider.value += (characterData.moveData.velocity.magnitude * meterGainSlowness) * Time.deltaTime;
            // }
            
        }

    }

}