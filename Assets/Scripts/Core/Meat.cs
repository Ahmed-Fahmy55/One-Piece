using UnityEngine;

public class Meat : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!collision.TryGetComponent(out MeatCollector collector)) return;
        collector.CollectMeat();
        Destroy(gameObject);
    }
}
