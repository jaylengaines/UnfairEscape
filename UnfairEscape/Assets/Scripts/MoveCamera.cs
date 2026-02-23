using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; // The position of the camera

    private void Update()
    {
        transform.position = cameraPosition.position; // Move the camera to the position of the camera position
    }
}
