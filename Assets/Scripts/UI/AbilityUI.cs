using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] AbilityBase ability;
    [SerializeField] Image calldownImage;



    private void OnEnable()
    {
        ability.OnAbilityUse += OnAbilityUse;
    }

    private void OnDisable()
    {
        ability.OnAbilityUse -= OnAbilityUse;
    }

    private void OnAbilityUse(float calldown)
    {
        calldownImage.fillAmount = 1;
        DOVirtual.Float(calldownImage.fillAmount, 0, calldown,x => calldownImage.fillAmount = x);
    }
}
