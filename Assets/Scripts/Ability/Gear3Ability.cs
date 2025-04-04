using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Gear3Ability : AbilityBase
{
    [Header("refs")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animator;
    [SerializeField] private TriggerEvent trigger;
    
    [Header("Settings")]
    [SerializeField] private int  damage;
    [SerializeField] private float duration;


    private void OnEnable()
    {
        trigger.OnTriggerInter += DamagePlayer;
    }

    private void OnDisable()
    {
        trigger.OnTriggerInter -= DamagePlayer;
    }

    public override void UseAbility()
    {
        
        player.CanControlPlayer = false;
        animator.ResetTrigger("Jump");
        animator.CrossFadeInFixedTime("Attack3", .2f);
        player.ResetUltimate();
        Invoke(nameof(ReturnPlayerControls),duration);
    }

    public override bool CanUseAbility()
    {
        return player.IsGrounded() && player.CanControlPlayer && player.CanUseUlt();
    }  

    private void DamagePlayer(Collider2D collider)
    {
        if(collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }

    private void ReturnPlayerControls()
    {
        player.CanControlPlayer = true;
    }
    
}
