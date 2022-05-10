using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EctoRat : MonoBehaviour
{
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private Collider2D wallCheck;
    [SerializeField] private GameObject sprite;
    private bool isFacingRight;
    private bool isGrounded;

    private Transform player;

    private bool mustTurn;
    private float horizontalDirection;

    [SerializeField] private float speed;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask wall;

    private void Awake() {
        player = FindObjectOfType<PlayerController>().transform;
        isFacingRight = true;
        mustTurn = false;
        isGrounded = true;
    }
    private void Start() {
        horizontalDirection = 1f;
    }

    private void OnEnable() {
        TurnCheck.turn += ChangeDirections;
    }

    private void OnDisable() {
        TurnCheck.turn -= ChangeDirections;
    }

    private void Update() {

        Vector3 currentPosition = transform.position;
        currentPosition.x += horizontalDirection * speed * Time.deltaTime;
        transform.position = currentPosition;

        if(GroundCheck()) {
            ChangeDirections();
        }

    }

    private bool GroundCheck() {
        if(!groundCheck.IsTouchingLayers(ground)) {
            mustTurn = true;
        }
        else mustTurn = false;

        return mustTurn;
    }

    private void ChangeDirections() {
        isFacingRight = !isFacingRight;
        horizontalDirection = -horizontalDirection;
        sprite.transform.localScale = new Vector3(horizontalDirection, 1f, 1f);
    }
}
