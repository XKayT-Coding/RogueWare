using UnityEngine;

public class GhostCounter : MonoBehaviour
{
    public int ghostsRemaining = 3;
    public Door_Logic door;

    public void GhostDefeated()
    {
        ghostsRemaining--;

        if (ghostsRemaining <= 0)
        {
            door.OpenDoor();
        }
    }
}
