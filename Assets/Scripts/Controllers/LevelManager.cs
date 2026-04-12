using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class LevelManager : MonoBehaviour
    {
        // This script manages the changing of levels, mostly when the player reaches the
        // end ladder. It just picks the next level in the build settings for now, but in the
        // future it might look to pick a random level from the list
        
        // Reference to the blackout, to trigger once we change scene
        public BlackOut blackOut;
        
        public void NextLevel()
        {
            //This will trigger the next level, but we need to make sure there's a level to go to
            // We do that by seeing whether a scene exists in the next slot of the build settings
            // Which we do a little backwards - just checking if the current scene + 1 is bigger than
            // the total amount of scenes available!
            if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCount)
            {
                // If a scene doesn't exist, it will just reload the current scene for now
                ReloadLevel();
            }
            else
            {
                // Triggers our FadeIn to black - check out the script to see more!
                blackOut.FadeIn();
                // If a scene does exist, then we'll go there!
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        // This logic loads in the same scene that the player is already on. This is used for the game over screen,
        // but also in the case of trying to move to a scene that doesn't exist.
        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
