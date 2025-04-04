using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCollector : MonoBehaviour
{

    public event Action OnFullCharge;
    public event Action<float> OnCollectMeat;


    [SerializeField] private int maxMeatCountToUlt;

    private int _currentMeatCount;


    public void CollectMeat()
    {
        if(_currentMeatCount == maxMeatCountToUlt) return;

        _currentMeatCount = Mathf.Min(++_currentMeatCount, maxMeatCountToUlt);
        OnCollectMeat?.Invoke((float) _currentMeatCount / maxMeatCountToUlt);

        if ( _currentMeatCount == maxMeatCountToUlt) OnFullCharge?.Invoke();
    }
    public void ResetCount()
    {
        _currentMeatCount = 0;
        OnCollectMeat?.Invoke(0);
    }

}
