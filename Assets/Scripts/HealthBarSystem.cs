using UnityEngine;
using System;

public class HealthBarSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    private HealthBar _healthBar;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _healthBar = GetComponentInChildren<HealthBar>();

        if (_healthBar != null)
        {
            OnHealthChanged += _healthBar.SetHealth;
            _healthBar.SetHealth(_currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        Debug.Log($"Healed! Current health: {_currentHealth}"); 
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth);
    }
}
