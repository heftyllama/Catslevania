using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnCheck : MonoBehaviour
{
  public static event Action turn;

  private void OnTriggerEnter2D(Collider2D other) {
      if(other.GetType() == typeof(CompositeCollider2D)) {
          turn();
      }  
  }
}
