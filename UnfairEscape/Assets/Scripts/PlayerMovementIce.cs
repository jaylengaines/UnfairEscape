using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovementIce : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // The speed of the player

    public float jumpForce; // The force of the jump
    public float jumpCooldown; // The cooldown of the jump
    public float airMultiplier; // The multiplier of the air movement   
    public bool readyToJump; // Whether the player is ready to jump

    [Header("Ground Check")]
    public float groundDrag; // The drag of the ground
    public float playerHeight; // The height of the player
    // LayerMask is a bitmask that represents the layers that the player can collide with
    public LayerMask whatIsGround; // The layer of the ground
    bool grounded; // Whether the player is grounded
    public Transform orientation; // The orientation of the player
    float horizontalInput; // Horizontal input for the player
    float verticalInput; // Vertical input for the player
    Vector3 moveDirection; // The direction of the player

    [Header("Movement Check")]
    public bool inputLocked; // Whether the input is locked
    public Vector2 lockedInput; // The locked input
    public float stopSpeedThreshold; // The threshold at which the player stops moving
    public float inputDeadZone; // The dead zone of the input
    

    Rigidbody rb; // The rigidbody of the player to apply forces to the player
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component
        rb.freezeRotation = true; // Freeze the rotation of the player

        inputLocked = false;
        lockedInput = Vector2.zero; // The locked input is set to zero
        stopSpeedThreshold = .15f; // The threshold at which the player stops moving is set to .15
        inputDeadZone = .1f; // The dead zone of the input is set to .1
    }

    private void Update() // Update is called every frame, if the MonoBehaviour is enabled
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.25f, whatIsGround); // Checks if the player is grounded
        if (grounded)
            rb.linearDamping = groundDrag; // slows down the player when they are grounded
        else
            rb.linearDamping = 0; // doesn't slow down the player when they are not grounded
        MyInput(); // Get the input
        SpeedControl(); // Speed control
    }
    private void FixedUpdate() // FixedUpdate is called every fixed frame-rate frame, if the MonoBehaviour is enabled
    {
        MovePlayer(); // Move the player
    }
    private void MyInput() // MyInput is called every frame, if the MonoBehaviour is enabled
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);// gets the velocity of the player
        if(inputLocked){
            // Keep using the comitted direction while sliding
            horizontalInput = lockedInput.x;
            verticalInput = lockedInput.y;
            // Unlock only after player has stopped
            if(flatVel.magnitude < stopSpeedThreshold){
                inputLocked = false;
                lockedInput = Vector2.zero;
                horizontalInput = 0f;
                verticalInput = 0f;
            }
        }
        else{
        // read fresh input only when not locked
        float rawxX = 0f;
        float rawY = 0f;
        if(Keyboard.current.dKey.isPressed) rawxX += 1f;
        if(Keyboard.current.aKey.isPressed) rawxX -= 1f;
        if(Keyboard.current.wKey.isPressed) rawY += 1f;
        if(Keyboard.current.sKey.isPressed) rawY -= 1f;
        Vector2 rawInput = new Vector2(rawxX, rawY); // creates a new vector2 with the raw input
        // Commit one direction and lcok input 
        if(rawInput.magnitude > inputDeadZone){
            lockedInput = rawInput.normalized;
            inputLocked = true;
            horizontalInput = lockedInput.x;
            verticalInput = lockedInput.y;
        }
        else{
            horizontalInput = 0f;
            verticalInput = 0f;
        }
        if (Keyboard.current.spaceKey.isPressed && readyToJump && grounded){
            Jump(); // jumps the player
            readyToJump = false; // sets readyToJump to false so you can't jump again until the jumpCooldown is over
            Invoke(nameof(ResetJump), jumpCooldown); // Invokes the ResetJump method after the jumpCooldown seconds
        }
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
