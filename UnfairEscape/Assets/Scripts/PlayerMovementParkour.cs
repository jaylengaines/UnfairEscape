using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovementParkour : MonoBehaviour
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
    bool wasGroundedLastFrame;
    public Transform orientation; // The orientation of the player
    float horizontalInput; // Horizontal input for the player
    float verticalInput; // Vertical input for the player
    Vector3 moveDirection; // The direction of the player

    [Header("Fall Damage")]
    public float fallDamageThreshold = 15f; // Minimum fall distance before respawn
    public Transform fallRespawnPoint; // Assign an empty object as respawn location

    float fallStartY;
    float lowestYWhileAirborne;
    public AudioSource walkSound;

    Rigidbody rb; // The rigidbody of the player to apply forces to the player
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component
        rb.freezeRotation = true; // Freeze the rotation of the player
        wasGroundedLastFrame = true;
        fallStartY = transform.position.y;
        lowestYWhileAirborne = transform.position.y;
    }

    private void Update() // Update is called every frame, if the MonoBehaviour is enabled
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.25f, whatIsGround); // Checks if the player is grounded
        HandleFallDamage();
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
        horizontalInput = 0f; // Horizontal input
        verticalInput = 0f; // Vertical input
        if (Keyboard.current.aKey.isPressed) horizontalInput += 1f; // Horizontal input
        if (Keyboard.current.dKey.isPressed) horizontalInput -= 1f; // Horizontal input
        if (Keyboard.current.sKey.isPressed) verticalInput += 1f; // Vertical input
        if (Keyboard.current.wKey.isPressed) verticalInput -= 1f; // Vertical input
        bool isMoving = horizontalInput != 0f || verticalInput != 0f;
        if (walkSound != null)
        {
            if (isMoving && !walkSound.isPlaying)
            {
                walkSound.Play();
            }
            else if (!isMoving && walkSound.isPlaying)
            {
                walkSound.Stop();
            }
        }
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

    private void HandleFallDamage()
    {
        // Started falling (or jumped) this frame.
        if (!grounded && wasGroundedLastFrame)
        {
            fallStartY = transform.position.y;
            lowestYWhileAirborne = transform.position.y;
        }

        // Track the lowest Y while airborne.
        if (!grounded)
        {
            lowestYWhileAirborne = Mathf.Min(lowestYWhileAirborne, transform.position.y);
        }

        // Landed this frame: compare start height to lowest airborne point.
        if (grounded && !wasGroundedLastFrame)
        {
            float fallDistance = fallStartY - lowestYWhileAirborne;
            if (fallDistance >= fallDamageThreshold)
            {
                RespawnAfterFall();
            }
        }

        wasGroundedLastFrame = grounded;
    }

    private void RespawnAfterFall()
    {
        if (fallRespawnPoint == null)
        {
            Debug.LogWarning("Fall respawn point is not assigned on PlayerMovementParkour.");
            return;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = fallRespawnPoint.position;
        transform.rotation = fallRespawnPoint.rotation;
    }
}
