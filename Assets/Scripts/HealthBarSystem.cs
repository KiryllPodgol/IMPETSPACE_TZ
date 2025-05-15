using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HealthBarSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    [Header("Invincibility Settings")] [SerializeField]
    private float iFramesDuration = 0.5f;
    [SerializeField, Min(1)] private int _numberOfFlashes = 3;
    private HealthBar _healthBar;
    private SpriteRenderer _characterSpriteRenderer;
    private Color _originalColor;
    private bool _isInvincible;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _healthBar = GetComponentInChildren<HealthBar>();
        _characterSpriteRenderer = GetComponentInParent<Character>()?.GetComponent<SpriteRenderer>();

        if (_characterSpriteRenderer != null)
        {
            _originalColor = _characterSpriteRenderer.color;
        }

        if (_healthBar != null)
        {
            OnHealthChanged += _healthBar.SetHealth;
            _healthBar.SetHealth(_currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible || _currentHealth <= 0) return;
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        OnHealthChanged?.Invoke(_currentHealth);


        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
        else
        {
            _isInvincible = true;
            StartCoroutine(Invulnerability());
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

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);

        if (_characterSpriteRenderer != null && iFramesDuration > 0)
        {
            float flashInterval = iFramesDuration / (_numberOfFlashes * 2);

            for (int i = 0; i < _numberOfFlashes; i++)
            {
                _characterSpriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
                yield return new WaitForSeconds(flashInterval);
                _characterSpriteRenderer.color = _originalColor;
                yield return new WaitForSeconds(flashInterval);
            }
        }
        else
        {
            yield return new WaitForSeconds(iFramesDuration);
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        _isInvincible = false;
    }
}