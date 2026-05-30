using UnityEngine;
using Controllers;

public class GhostDamageZone : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponentInParent<HealthController>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}
