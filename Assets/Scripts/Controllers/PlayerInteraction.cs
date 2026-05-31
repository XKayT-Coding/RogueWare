using System;
using Interactables;
using TMPro;
using UI;
using UnityEngine;
using static UnityEngine.Input;

namespace Controllers
{
    public class PlayerInteraction : MonoBehaviour
    {
        // This script controls player interaction of in-game objects
        
        // First we need reference to an external object - a text UI that tells the player
        // to interact. This is created elsewhere and linked via a clever UIController
        public TMP_Text interactableUI;

        // Then we create a bool that tracks if we're near an interactable so we can give
        // special options if so
        private bool _isNearInteractable;
        
        // We store a copy of the interactable that we're near. It's important to not let
        // the player be near more than one or this won't work properly!
        private Interactable _interactableObject;
        
        // My Battery variables and drain system variables
        public int maxBatteries = 3;
        public int batteryCount = 3;
        public TMP_Text batteryText;
        public float batteryDrainTime = 10f;
        private float _batteryTimer;
        
        // My Light variables
        public Transform lightObject;
        public CircleCollider2D lightCollider;

        private Vector3 _originalLightScale;
        private float _originalColliderRadius;
        

        private void Start()
        {
            // Turn off the UI object as the game begins if its been left on
            interactableUI.gameObject.SetActive(false);
            
            // Setting the UI to display the number of batteries on screen + implementing drain timer
            batteryText.text = batteryCount + " / " + maxBatteries;
            _batteryTimer = batteryDrainTime;
            
            //Caching original values 
            _originalLightScale = lightObject.localScale;
            _originalColliderRadius = lightCollider.radius;
            
            UpdateLightSystem();
        }

        // On trigger, we check if the collision object has the tag "Interactable". Make sure that it does!!!
        // It should also have the "Interactable" component, otherwise none of this will work 
        private void OnTriggerEnter2D(Collider2D col)
        {
            // For battery pickups
            if (col.CompareTag("Battery"))
            {
                if (batteryCount < maxBatteries) 
                {
                    batteryCount = Mathf.Clamp(batteryCount + 1, 0, maxBatteries);
                    batteryText.text = batteryCount + " / 3";
                    _batteryTimer = batteryDrainTime;
                    Destroy(col.gameObject);
                    UpdateLightSystem();
                }
                return;
            }
            
            // If it doesn't have either the tag or the component, then it won't work
            if (!col.CompareTag("Interactable") || col.GetComponent<Interactable>().hasInteracted) return;

            // Otherwise it tells the script that we're near an interactable object
            _isNearInteractable = true;
            
            // And then sets our internal copy of the interactable that it's found
            _interactableObject = col.GetComponent<Interactable>();
            
            // Sets the UI interactable text to active and changes the text
            interactableUI.gameObject.SetActive(true);
            interactableUI.text = "Press " + _interactableObject.requiredInput + " to Interact"; 
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // When we leave the trigger, it sets everything back to the way it was before
            // we found the interactable object
            if(interactableUI!=null)interactableUI.gameObject.SetActive(false);
            _isNearInteractable = false;
            _interactableObject = null;
        }
        
        // Core Light System
        private void UpdateLightSystem()
        {
            float batteryPercent = (float) batteryCount / maxBatteries;
            
            batteryPercent = Mathf.Pow(batteryPercent, 0.5f);
            batteryPercent = Mathf.Clamp(batteryPercent, 0, 1);

            bool hasBattery = batteryCount > 0;

            lightObject.gameObject.SetActive(hasBattery);
            lightCollider.enabled = hasBattery;

            if (!hasBattery)
            {
                return;
            }
            
            lightObject.localScale = _originalLightScale * batteryPercent;
            lightCollider.radius = _originalColliderRadius * batteryPercent;
        }

        private void Update()
        {
            if (batteryCount <= 0)
            {
                _batteryTimer = 0f;
                return;
            }
            
            // Drain Logic
            _batteryTimer -= Time.deltaTime;
            
            if (_batteryTimer <= 0f && batteryCount > 0)
            {
                Debug.Log("Battery Drained");
                Debug.Log("Battery Count: " + batteryCount);
                batteryCount = Mathf.Max(0, batteryCount - 1);
                batteryText.text = batteryCount + " / " + maxBatteries;
                _batteryTimer = batteryDrainTime;
                UpdateLightSystem();

                if (batteryCount <= 0)
                {
                    batteryText.text = "Find more batteries!";
                }
                else
                {
                    batteryText.text = batteryCount + " / " + maxBatteries; 
                }
            }
            
            // This is where our bool comes in - if we don't have an interactable object nearby
            // then the update won't do anything at all
            if (!_isNearInteractable) return;

            // But if we're near something, and after we check that the interactable hasn't broken somehow,
            // once the player presses the required button, they will tell the object to interact!
            if (GetKeyDown(_interactableObject.requiredInput) && _interactableObject != null && !DialogueSystem.IsActive)
            {
                _interactableObject.Interact();
            }
        }
    }
}
