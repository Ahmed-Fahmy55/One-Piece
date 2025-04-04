using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesHealthUI : MonoBehaviour
{

    [SerializeField] Image lifesImage;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Sprite[] lifesSprites;
    [SerializeField] private Health health;
    [SerializeField] private Transform deathParticle;
    

    private void OnEnable()
    {
        health.OnLifeLost += OnLostLife;
        health.OnDie += OnDie; ;
    }

    private void OnDie()
    {
        if(deathParticle)Instantiate(deathParticle,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        health.OnLifeLost -= OnLostLife;
        health.OnDie -= OnDie; ;
    }

    private void Start()
    {
        OnLostLife(3);
    }

    private void OnLostLife(int remainingLifes)
    {
        if (remainingLifes >= lifesSprites.Length) return;
        if(lifesImage)lifesImage.sprite = lifesSprites[Mathf.Max(0, remainingLifes - 1)];
        if (SpriteRenderer) SpriteRenderer.sprite = lifesSprites[Mathf.Max(0, remainingLifes - 1)];
    }
}
