using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Controllers;

public class EventManager : MonoBehaviour
{

    public static event Action<SkateCharacterController, Vector3, float> OnKnockbackReceived;
    public static event Action<SkateCharacterController, float> OnDamageReceived;
    public static event Action<SkateCharacterController, float> OnStaggerReceived;

    public static void Knockback(SkateCharacterController target, Vector3 direction, float knockbackAmount) {
        OnKnockbackReceived?.Invoke(target, direction, knockbackAmount);
    }

    public static void Damage(SkateCharacterController target, float damageAmount) {
        
    }

    public static void Stagger(SkateCharacterController target, float duration) {
        
    }
}
