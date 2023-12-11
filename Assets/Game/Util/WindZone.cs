using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        
        
        if (player) {
            Debug.Log(player);
            float accelAmount= Vector3.Dot(player.moveData.velocity, transform.forward);
            float speed = 20f;

            if (accelAmount < speed) {
                player.preUpdateEnvironmentForces = transform.forward * (speed - accelAmount);
                player.ignoreGravityTimer = .1f;
                player.playerData.grounded = false;
            }

            // EventManager.Knockback(player, Vector3.forward, 15f);
        }

    }
}
