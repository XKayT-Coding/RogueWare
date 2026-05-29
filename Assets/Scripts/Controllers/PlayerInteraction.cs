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

        public int batteryCount = 0; 

        private void Start()
        {
            // Turn off the UI object as the game begins if its been left on
            interactableUI.gameObject.SetActive(false);
        }

        // On trigger, we check if the collision object has the tag "Interactable". Make sure that it does!!!
        // It should also have the "Interactable" component, otherwise none of this will work 
        private void OnTriggerEnter2D(Collider2D col)
        {
            // For battery pickups
            if (col.CompareTag("Battery"))
            {
                batteryCount++;
                Destroy(col.gameObject);
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

        private void Update()
        {
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
