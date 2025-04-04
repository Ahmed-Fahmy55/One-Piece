using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltUI : MonoBehaviour
{
    [SerializeField] private Image ultImage;


    private MeatCollector meatCollector;



    private void Awake()
    {
        meatCollector = FindObjectOfType<MeatCollector>();
    }

    private void OnEnable()
    {
        if(meatCollector) meatCollector.OnCollectMeat += OnCollectMeat;
    }

    private void OnDisable()
    {
        if (meatCollector) meatCollector.OnCollectMeat -= OnCollectMeat;

    }
    private void Start()
    {
        OnCollectMeat(0);
    }

    private void OnCollectMeat(float percentage)
    {
        ultImage.fillAmount = percentage;
    }
}
