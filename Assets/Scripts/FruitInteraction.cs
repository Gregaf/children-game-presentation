using UnityEngine;

public class FruitInteraction : MonoBehaviour, ICatchable
{
    [SerializeField] private FruitSO _fruitData;
    [SerializeField] private LayerMask _collisionMask;

    public void Catch()
    {
        // Invoke event to notify listeners of heal amount, and score value
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        var layerBits = 1 << other.gameObject.layer;

        if ((_collisionMask.value & layerBits) != 0)
        {
            Destroy(gameObject);
        }
    }
}
