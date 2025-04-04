using DG.Tweening;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    public event Action OnDie;
    public event Action<int> OnLifeLost;

    public event Action<float> OnTakeDamage;


    [SerializeField] private int maxHealth = 180;
    [SerializeField] int maxLifes;

    private int _currentHealth;
    private int _currentLifes;

    private float _lifePercentage;

    bool _isDead;


    private void Start()
    {
        _currentHealth = maxHealth;
        _currentLifes = maxLifes;

        _lifePercentage = 1 / (float)maxLifes;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        float remainingHealthPercentage = (float)_currentHealth / maxHealth;
        OnTakeDamage?.Invoke(remainingHealthPercentage);

        if ((_currentLifes / (float)maxLifes) - remainingHealthPercentage >= _lifePercentage)
        {
            _currentLifes--;
            OnLifeLost?.Invoke(_currentLifes);
        }

        if (_currentHealth == 0)
        {
            _isDead = true;
            OnDie?.Invoke();
        }
    }

    public void TakeDamage(int damage, float damageTime)
    {
        if (_isDead) return;

        DOVirtual.Int(_currentHealth, Mathf.Max(0, _currentHealth - 20), 2, (x) =>
        {
            _currentHealth = x;
            OnTakeDamage?.Invoke((float)_currentHealth / maxHealth);
        }).
        OnComplete(() =>
        {
            if (_currentHealth == 0)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        });
    }

    /*    [Button("takeDamage")]
        public void TakeInstantDamage()
        {
            if (_isDead) return;

            _currentHealth = Mathf.Max(0, _currentHealth - 20);
            OnTakeDamage?.Invoke((float)_currentHealth / maxHealth);

            if (_currentHealth == 0)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
        [Button]
        public void TakeTimeInstantDamage()
        {
            if (_isDead) return;

            DOVirtual.Int(_currentHealth, Mathf.Max(0, _currentHealth - 20), 2, (x) =>
            {
                _currentHealth = x;
                OnTakeDamage?.Invoke((float)_currentHealth / maxHealth);
            }).
            OnComplete(() =>
            {
                if (_currentHealth == 0)
                {
                    _isDead = true;
                    OnDie?.Invoke();
                }
            });
        }*/

    public int GetMaxLifes()
    {
        return maxHealth;
    }
    public bool IsDead()
    {
        return _isDead;
    }

}
