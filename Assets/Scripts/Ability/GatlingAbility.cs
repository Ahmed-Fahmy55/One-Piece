using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingAbility : AbilityBase
{
    [SerializeField] private Animator animator;
    [SerializeField] PlayerController player;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] LayerMask enemyesLayer;
    [SerializeField] private int  damage;
    [SerializeField] private float range;
    [SerializeField] private float duration;


    private RaycastHit2D[] _hits;



    public override void UseAbility()
    {
        player.CanControlPlayer = false;
        animator.CrossFadeInFixedTime("Attack1", .2f);
        animator.ResetTrigger("Jump");

        Vector2 direc = player.IsPlayerFacingLeft()? Vector2.left : Vector2.right;
        _hits = Physics2D.CapsuleCastAll(capsuleCollider.bounds.center, capsuleCollider.size, 
            capsuleCollider.direction, 0, direc, range, enemyesLayer);

        foreach (var hit in _hits)
        {
            if (!hit.collider.TryGetComponent(out Health health)) continue;
            health.TakeDamage(damage);
        }

        Invoke(nameof(ReturnPlayerControls),duration);
    }

    public override bool CanUseAbility()
    {
        return player.IsGrounded() && player.CanControlPlayer;
    }

    private void ReturnPlayerControls()
    {
        player.CanControlPlayer = true;
        audioSource.Stop();
    }
}
