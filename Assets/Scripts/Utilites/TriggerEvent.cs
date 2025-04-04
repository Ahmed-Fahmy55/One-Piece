using System;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{

    public event Action<Collider2D> OnTriggerInter;

    [SerializeField] string cillisionTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(cillisionTag)) return;
        OnTriggerInter?.Invoke(collision);
    }
}
