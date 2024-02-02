using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Controllers;

public class WindZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {

        SkateCharacterController ctx = other.GetComponent<SkateCharacterController>();
        
        
        if (ctx) {
            float accelAmount= Vector3.Dot(ctx.characterData.moveData.velocity, transform.forward);
            float speed = 20f;

            if (accelAmount < speed) {
                ctx.preUpdateEnvironmentForces = transform.forward * (speed - accelAmount);
                ctx.timerController.ignoreGravityTimer = .1f;
                ctx.characterData.playerData.grounded = false;
            }

            // EventManager.Knockback(player, Vector3.forward, 15f);
        }

    }
}
