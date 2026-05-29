using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        // Welcome to the PlayerController! This script takes player input and turns it into action
        // Feel free to look through this script and figure out how everything works!

        // References to components on the Player object that we'll use later
        // We assign these references in the Start() method.
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        
        // Then we set up our movement variables
        public float speed;

        // and some values related to Jumping
        [HideInInspector] public bool isJumping = false;
        public float jumpDuration = 1f;
        public float jumpHeight = 1f;
        private float _jumpTimer = 0;
        private Vector3 _jumpOrigin;  
        private Vector3 _jumpVelocity; // Stores movement velocity during jump
        
        // Ignore these, but this is a selection of bools to check if the player is moving
        // so that our animator knows when to stop. A fun trick but not required!
        private bool _downA, _downW, _downS, _downD;

        public Transform torch; // Reference for the Torch
        public Vector3 torchRightPosition;
        public Vector3 torchLeftPosition;

        public Transform light;
        
        void Start()
        {
            // These lines link up our component references as soon as the game starts
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
    
        void Update()
        {
            // This section captures Player input (keyboard only) and translates it into
            // movement, and turns the animator on.
            
            if (GetKeyDown(A)) 
            {
                // The line below flips the Sprite if the player moves in the opposite direction
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                _downA = true;
            }
        
            if (GetKeyDown(W)) {_downW = true;}
            if (GetKeyDown(S)) {_downS = true;}
            if (GetKeyDown(D))
            {
                _downD = true;
                // This line flips the Sprite back if the player swaps direction
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (GetKeyUp(A)) {_downA = false;}
            if (GetKeyUp(W)) {_downW = false;}
            if (GetKeyUp(S)) {_downS = false;}
            if (GetKeyUp(D)) {_downD = false;}
        
            // The below line starts the Animator if the Player is pressing buttons (remember the bools?)
            if (!_downA && !_downW && !_downS && !_downD)
            {
                _animator.enabled = false;
            }
            else
            {
                _animator.enabled = true;
            }

            // These lines control the direction of movement
            float horizontal = GetAxis("HorizontalMove");
            float vertical = GetAxis("VerticalMove");

            // These lines control normal movement while not jumping
            if (!isJumping)
            {
                _rigidbody2D.linearVelocity = new Vector3(horizontal * speed, vertical * speed, 0);
            }
            
            // These lines control the movement while jumping
            if (Input.GetKeyDown("space") && !isJumping)
            {
                isJumping = true;
                _jumpTimer = jumpDuration;

                // These lines capture the current position and velocity as the origin for the jump
                _jumpOrigin = transform.localPosition;
                _jumpVelocity = _rigidbody2D.linearVelocity; // This stores velocity to continue moving during jump
            }

            // These lines control air movement while jumping
            if (isJumping)
            {
                _jumpTimer -= Time.deltaTime;

                // Create a jump arc using a sine wave - don't worry about it
                float verticalOffset = Mathf.Sin((1 - _jumpTimer / jumpDuration) * Mathf.PI) * jumpHeight;

                // Apply the jump motion with forward movement
                transform.localPosition = _jumpOrigin + _jumpVelocity * (jumpDuration - _jumpTimer) + new Vector3(0, verticalOffset, 0);

                if (_jumpTimer <= 0)
                {
                    isJumping = false;
                }
            }
            
            // My Torch Sprite code that will flip the direction of the torch if the Player moves left or right
            if (horizontal > 0)
            {
                torch.localPosition = torchRightPosition;
                torch.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (horizontal < 0)
            {
                torch.localPosition = torchLeftPosition;
                torch.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }

        // This bool checks if any movement key is pressed by looking at all of the bools
        public bool AnyKeyPressed()
        {
            return _downA || _downW || _downS || _downD;
        }
    }
}
