using System;

using UnityEngine;

public class Catcher : MonoBehaviour, IHealth
{
    public int MaxHealth { get; private set; }

    public int CurrentHealth { get; private set; }

    public static event Action<int> OnHealthChanged;
    public static event Action OnHealthDepleted;

    public void AddHealth(int healAmount)
    {
        CurrentHealth += healAmount;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void RemoveHealth(int damageAmount)
    {
        CurrentHealth -= damageAmount;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnHealthDepleted?.Invoke();
        }
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICatchable>(out ICatchable interactable))
        {
            interactable.Catch();
        }
    }
}
