using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovementIce : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // The speed of the player

    float lastHorizontalInput;
    float lastVerticalInput;
    public float jumpForce; // The force of the jump
    public float jumpCooldown; // The cooldown of the jump
    public float airMultiplier; // The multiplier of the air movement   
    public bool readyToJump; // Whether the player is ready to jump
    public float playerHeight; // The height of the player
    // LayerMask is a bitmask that represents the layers that the player can collide with
    public LayerMask whatIsGround; // The layer of the ground
    bool grounded; // Whether the player is grounded
    public Transform orientation; // The orientation of the player
    float horizontalInput; // Horizontal input
    float verticalInput; // Vertical input
    Vector3 moveDirection; // The direction of the player
    Rigidbody rb; // The rigidbody of the player
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component
        rb.freezeRotation = true; // Freeze the rotation of the player
        rb.linearDamping = 0f; // no drag on the ground
    }

    private void Update() // Update is called every frame, if the MonoBehaviour is enabled
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.25f, whatIsGround); // Checks if the player is grounded
        rb.linearDamping = 0f; // no drag on the ground
        MyInput(); // Get the input
        //SpeedControl();
    }
    private void FixedUpdate() // FixedUpdate is called every fixed frame-rate frame, if the MonoBehaviour is enabled
    {
        MovePlayer(); // Move the player
    }
    private void MyInput() // MyInput is called every frame, if the MonoBehaviour is enabled
    {
        horizontalInput = 0f; // Horizontal input
        verticalInput = 0f; // Vertical input
        if (Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed && !Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed) horizontalInput += 1f;
        if (Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed && !Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed) horizontalInput -= 1f;
        if (Keyboard.current.wKey.isPressed && !Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed && !Keyboard.current.sKey.isPressed) verticalInput += 1f;
        if (Keyboard.current.sKey.isPressed && !Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed && !Keyboard.current.wKey.isPressed) verticalInput -= 1f;
        if (Keyboard.current.spaceKey.isPressed && readyToJump && grounded){
            Jump(); // jumps the player
            readyToJump = false; // sets readyToJump to false so you can't jump again until the jumpCooldown is over
            Invoke(nameof(ResetJump), jumpCooldown); // Invokes the ResetJump method after the jumpCooldown seconds
        }
    }
    private void MovePlayer() // MovePlayer is called every fixed frame-rate frame, if the MonoBehaviour is enabled
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // lets you walk in the direction that your moving
        if(grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); // adds force to the player
        }
        else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // adds force to the player
        }

    }
    private void SpeedControl(){ // checks if the player is moving faster than the speed and limits the speed
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // gets the velocity of the player
        if(flatVel.magnitude > moveSpeed){ // checks if the player is moving faster than the speed
            Vector3 limitedVel = flatVel.normalized * moveSpeed; // limits the speed of the player
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z); // sets the velocity of the player
        }
    }

    private void Jump(){
        // reset y velocity so you alwasy jump the same height
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // adds force to the player Impulse that is applied instantly once

    }
    private void ResetJump(){
        readyToJump = true;
    }

}
