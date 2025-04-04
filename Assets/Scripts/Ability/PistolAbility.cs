using UnityEngine;

public class PistolAbility : AbilityBase
{
    [Header("refs")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animator;
    [SerializeField] private TriggerEvent trigger;

    [Header("Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float duration;


    private bool alreadyHitEnemy;

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
        animator.CrossFadeInFixedTime("Attack2", .2f);

        Invoke(nameof(ReturnPlayerControls), duration);
    }

    public override bool CanUseAbility()
    {
        return player.IsGrounded() && player.CanControlPlayer;
    }

    private void DamagePlayer(Collider2D collider)
    {
        if (alreadyHitEnemy) { return; }
        if (collider.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            alreadyHitEnemy = true;
        }
    }

    private void ReturnPlayerControls()
    {
        player.CanControlPlayer = true;
        alreadyHitEnemy = false;
        audioSource.Stop();
    }

}
