using System.Collections.Generic;
using UnityEngine;

public class CanonBullet : MonoBehaviour
{
    [SerializeField] private int damage;


    private ParticleSystem _ps;

    private List<ParticleCollisionEvent> collidedParticlesEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int hitNumbs = _ps.GetCollisionEvents(other, collidedParticlesEvents);
        /*if (!other.TryGetComponent(out Health Health)) return;
        //Health.TakeDamage(damage);*/
        for (int i = 0; i < hitNumbs; i++)
        {
            if (!other.TryGetComponent(out Health Health)) continue;
            Health.TakeDamage(damage);
        }
    }
}
