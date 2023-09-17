public interface IHealth
{
    int MaxHealth { get; }
    int CurrentHealth { get; }

    void RemoveHealth(int damageAmount);
    void AddHealth(int healAmount);
}