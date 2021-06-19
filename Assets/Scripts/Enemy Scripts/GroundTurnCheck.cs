using System;
using UnityEngine;

public class GroundTurnCheck : MonoBehaviour
{
    public static event Action turn;

  private void OnTriggerExit2D(Collider2D other) {
      if(other.GetType() == typeof(CompositeCollider2D)) {
          turn();
      }  
  }
}
