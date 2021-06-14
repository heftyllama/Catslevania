using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(Input.GetKeyDown(KeyCode.T)) {
            Dash();
        }
        else {
            moveSpeed = 2f;
        }
    }

    void Dash() {
        moveSpeed = 5f;
    }
}
