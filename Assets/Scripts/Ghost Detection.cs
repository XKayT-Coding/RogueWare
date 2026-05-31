using UnityEngine;

public class GhostDetection : MonoBehaviour
{
    private GhostScript _ghost;

    private void Start()
    {
        _ghost = GetComponentInParent<GhostScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _ghost.StartChase(other.transform);
        Debug.Log("Player detected");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _ghost.StopChase();
        Debug.Log("Player lost");
        _ghost.StartReturn();
    }
}
