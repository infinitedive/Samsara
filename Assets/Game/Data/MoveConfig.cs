using UnityEngine;
[System.Serializable]
public class MoveConfig: MonoBehaviour {

    [Header ("Jumping and gravity")]
    public float gravity = 7.5f;
    public float jumpForce = 15f;
    public float boostForce = 10f;
    
    [Header ("Velocity Clamp")]
    public float maxVelocity = 40f;
    public float terminalVelocity = 40f;

    [Header ("Dev Tools")]
    public bool flyMode = false;

    [Header ("Ground movement")]
    public float walkSpeed = 7.5f;
    public float runSpeed = 15f;
    public float maxCharge = 2f;

    [Header ("Grappling")]

    public float maxDistance = 100f;
    public float minDistance = 10f;
    [ColorUsage(true, true)]
    public Color grappleColor = Color.black;
    [ColorUsage(true, true)]
    public Color accelColor = Color.green;
    [ColorUsage(true, true)]
    public Color normalColor = Color.cyan;
    public float castRadius = 1f;

    [Header ("Aiming")]
    public float sensitivityMultiplier = .5f;
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;
    public float minYRotation = -75f;
    public float maxYRotation = 75f;
    public float minXRotation = -180f;
    public float maxXRotation = 180f;
    
}

