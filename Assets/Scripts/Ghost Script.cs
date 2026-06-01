using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public Animator animator;
    
    public float moveSpeed = 2f;
    
    private Transform _player;
    private bool _chasing;
    private bool _returning;
    private Vector3 _startPos;
    
    void Start()
    {
        _startPos = transform.position;
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    
    public void StartChase(Transform player)
    {
        _player = player;
        _chasing = true;
        
        if (animator != null)
        {
            animator.enabled = false;
        }
        
    }
    
    public void StopChase()
    {
        _player = null;
        _chasing = false;

        if (animator != null)
        {
            animator.enabled = true;
        }
        
    }

    public void StartReturn()
    {
        _chasing = false;
        _returning = true;
    }
    
    public void GhostDie()
    {
        animator.Play("Ghost_Die");
    }
    private void Update()
    {
        if (_chasing && _player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else if (_returning)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _startPos,
                moveSpeed * Time.deltaTime
            );

            // stop when close enough
            if (Vector2.Distance(transform.position, _startPos) < 0.05f)
            {
                _returning = false;
            }
        }
        
    }
}
