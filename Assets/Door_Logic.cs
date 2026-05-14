using UnityEngine;

public class Door_Logic : MonoBehaviour
{
   public bool isOpen;
   public Sprite doorOpen, doorClose;
   private SpriteRenderer _renderer;
   private BoxCollider2D _boxCollider2D;
   
   private void Start()
   {
      _renderer = GetComponent<SpriteRenderer>();
      _boxCollider2D = GetComponent<BoxCollider2D>();
   }
   
   public void OpenDoor()
   {
      if (isOpen) return;
      _renderer.sprite = doorOpen;
      _boxCollider2D.enabled = false;
      isOpen = true;
   }

   public void CloseDoor()
   {
      if (!isOpen) return;
      _renderer.sprite = doorClose;
      _boxCollider2D.enabled = true;
      isOpen = false;
   }
}


