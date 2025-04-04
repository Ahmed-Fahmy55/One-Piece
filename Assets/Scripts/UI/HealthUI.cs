using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Health health;




    private void OnEnable()
    {
        health.OnTakeDamage += OnTakeDamage;
    }
    private void OnDisable()
    {
        health.OnTakeDamage -= OnTakeDamage;
    }

    private void OnTakeDamage(float healthPercentage)
    {
        healthBarImage.fillAmount = healthPercentage;
    }
}
