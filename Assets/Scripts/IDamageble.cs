using System;

public interface IDamageable
{
    void TakeDamage(int damage);
    void Heal(int amount);
    int CurrentHealth { get; }
    int MaxHealth { get; }
    event Action OnDeath;
    event Action<int> OnHealthChanged;
}