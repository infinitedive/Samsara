using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    public static event Action<PlayerCharacter, Vector3, float> OnKnockbackReceived;
    public static event Action<PlayerCharacter, float> OnDamageReceived;
    public static event Action<PlayerCharacter, float> OnStaggerReceived;

    public static void Knockback(PlayerCharacter target, Vector3 direction, float knockbackAmount) {
        OnKnockbackReceived?.Invoke(target, direction, knockbackAmount);
    }

    public static void Damage(PlayerCharacter target, float damageAmount) {
        
    }

    public static void Stagger(PlayerCharacter target, float duration) {
        
    }
}
