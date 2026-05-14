using UnityEngine;

public class Door_Logic : MonoBehaviour
{
   public bool isOpen;

   private SpriteRenderer renderer;
   
   private void Start()
   {
      renderer = GetComponent<SpriteRenderer>();
   }
   
   private void OpenDoor()
   {
      if (isOpen) return;
      renderer.sprite = Resources.Load<Sprite>("Door_Open");
   }
   
}


