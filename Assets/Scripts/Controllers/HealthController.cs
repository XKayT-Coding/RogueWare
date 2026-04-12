using MoreMountains.Feedbacks;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers
{
    public class HealthController : MonoBehaviour
    {
        // Welcome to the Health Controller! This script will interact with the Health of our player
        // and enemies - the clue is in the name. Feel free to look through this script and figure out
        // how everything works!
        
        // First, let's set up our most important variable: health!
        public float health;
        [HideInInspector] public float maxHealth;
        
        // Then we set up a value for invulnerability time
        public float invulnTime;
        private float _timer;
        private bool _isInvuln;
        
        // This line reference a component that's attached to the object
        private SpriteRenderer _sprite;
        
        // This line references a component that's attached to something within the object
        private GameObject _damageNumUI;

        // This is another set of component references specific to the player
        private PlayerController _playerController;
        public PlayerHealthUI healthUI;
        private MMF_Player _mmf;
        
        // For ease of use, we set up a Unity Event for onDeath - this means that we can make things happen
        // when the player dies automatically
        public UnityEvent onDeath;

        private void Start()
        {
            // These lines link up our component references as soon as the game starts
            _sprite = GetComponent<SpriteRenderer>();
            _damageNumUI = transform.GetChild(1).gameObject;
            
            // These lines link up our player-specific component references
            if (gameObject.CompareTag("Player"))
            {
                _mmf = GetComponent<MMF_Player>();
                _playerController = GetComponent<PlayerController>();
            }
            
            // Hard force maxhealth to = health on Start
            maxHealth = health;
        }
        
        private void Update()
        {
            // As this game has invulnerability, these lines make sure that the object can't
            // take damage while that invulnerability is ongoing
            if (!_isInvuln) return;
            _timer += Time.deltaTime;
            if (!(_timer >= invulnTime)) return;
            _isInvuln = false;
            _sprite.color = Color.white;
        }
        
        // This method is important - it's the way that we deal damage
        // To make the player or an enemy take damage, you must find it's
        // HealthController via GetComponent<HealthController>() and fire
        // this TakeDamage() method, putting a float value in the brackets
        // detailing how much damage to give - ie TakeDamage(1)
        public void TakeDamage(float damageToTake)
        {
            // This lines makes sure that if they can't take damage, they won't
            if (_isInvuln) return;
            if (IsPlayer() && PlayerInputInvulnerability()) return;
            
            // If they can, they will then take damage
            // We change the player sprite colour to show the damage being taken
            _sprite.color = Color.grey;
            // If the enemy/player's health would be 0 or less, they die
            if (health - damageToTake <= 0) {Die();}
            // But if we can take damage and not die, then we take the damage
            else {health -= damageToTake;}
            
            // We then show the damage that we've taken in a cute little box
            _damageNumUI.GetComponent<TMP_Text>().text = "-" + damageToTake;
            _damageNumUI.GetComponent<MMF_Player>().PlayFeedbacks();
            
            // As the object has taken damage, we now trigger invulnerability
            _isInvuln = true;
            _timer = 0;
            
            // Player specific - Updates the Health UI (hearts) and triggers a Screen Shake
            if (IsPlayer())
            {
                healthUI.UpdateOnDamage();
                TriggerShake();
            }
        }

        public void RecoverHealth(float healthToRecover)
        {
            // This method controls recovering health, which is essentially the opposite of TakeDamage()
            // As with taking damage, we start by checking whether the amount we're recovering will max out our health
            if (health + healthToRecover >= maxHealth)
            {
                // If so, we can just set it to be the maximum, ignoring overspill
                health = maxHealth;
            }
            else
            {
                // Otherwise, we can make the addition
                health += healthToRecover;
            }
            
            // Player specific - Updates the Health UI (hearts)
            if (IsPlayer())
            {
                healthUI.UpdateOnDamage();
            }
        }
        
        private void Die()
        {
            // As mentioned earlier, if health is zero or below, the character dies
            // We trigger the Unity Event for onDeath - anything we've set up for this event in Unity will be triggered
            // In the example scene, you will see that the Game Over window is triggered to "SetActive(true)" in this event
            onDeath.Invoke();
            // We destroy the game object
            Destroy(gameObject);
        }
        
        
        // !! Below is some player-specific code !!
        
        // Checking if the object is a player, for player-only effects
        private bool IsPlayer()
        {
            return gameObject.CompareTag("Player");
        }
        
        // This triggers a screen shake using an external component
        private void TriggerShake()
        {
            _mmf.PlayFeedbacks();
        }

        // Checking if the player has initiated a jump, which makes them temporarily invulnerable
        private bool PlayerInputInvulnerability()
        {
            return _playerController.isJumping;
        }
    }
}
