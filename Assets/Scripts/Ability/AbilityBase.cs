using System;
using System.Collections;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{

    public event Action<float> OnAbilityUse;

    [Header("Refs")]
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip sound;
    [SerializeField] protected AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private string title;
    [SerializeField] private float cooldownTime = 1;

    private bool onCalldown = false;


    public void TriggerAbility()
    {
        if (!CanUseAbility()) return;

        if (!onCalldown)
        {
            OnAbilityUse?.Invoke(cooldownTime);
            PlayAbilitySound();
            UseAbility();
            StartCooldown();
        }
    }

    private void PlayAbilitySound()
    {
        if (audioSource != null && sound != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    public abstract void UseAbility();
    public abstract bool CanUseAbility();


    private void StartCooldown()
    {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        onCalldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCalldown = false;
    }
}
