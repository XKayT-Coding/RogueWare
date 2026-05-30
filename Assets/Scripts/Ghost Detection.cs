using UnityEngine;

public class GhostDetection : MonoBehaviour
{
    public Transform ghostRoot;
    public float moveSpeed;

    private Transform _target;

    private void Start()
    {
        if (ghostRoot == null)
        {
            ghostRoot = transform.root;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Detection hit: " + other.name);
        
        if (other.CompareTag("Player"))
        {
            _target = other.transform;
            Debug.Log("Player detected");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       if (!other.CompareTag("Player")) return;
       
       _target = null;
       Debug.Log("Player lost");
    }
    
    private void Update(){
        if (_target == null) return;
        ghostRoot.position = Vector2.MoveTowards(ghostRoot.position,
            _target.position,
            moveSpeed * Time.deltaTime);
    }
}
