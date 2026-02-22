using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerCam : MonoBehaviour
{
    public float sensx; // Horizontal mouse sensitivity
    public float sensy; // Vertical mouse sensitivity
    public Transform orientation; // The orientation of the player
    float xRotation; // Vertical rotation
    float yRotation; // Horizontal rotation
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }
    private void Update()
    {
        // Get mouse input
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * Time.deltaTime * sensx; // Horizontal mouse input
        float mouseY = mouseDelta.y * Time.deltaTime * sensy; // Vertical mouse input
        yRotation += mouseX; // Rotate the camera horizontally
        xRotation -= mouseY; // Rotate the camera vertically
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation to avoid gimbal lock
        // Rotate the camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // Rotate the camera
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // Rotate the orientation
    }

}
