using UnityEngine;
using Controllers;

public class LightDamage : MonoBehaviour
{
    private GameObject _playerRoot;

    private void Start()
    {
        _playerRoot = transform.root.gameObject;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger hit " + other.name);

        if (other.transform.root.gameObject == _playerRoot)
        {
            return;
        }

        if (!other.CompareTag("EnemyHurtbox")) return;
        
        var health = other.GetComponentInParent<HealthController>();

        if (health == null)
        {
            Debug.Log("No Health Controller found on: " + other.name);
            return;
        }
        
        Debug.Log("Damaging: " + health.gameObject.name);
        health.TakeDamage(1f * Time.deltaTime);
    }
}
