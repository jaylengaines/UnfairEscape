using UnityEngine;

/// <summary>
/// Put this on an object with a trigger collider. When the player runs into it,
/// ice movement is activated (and optionally a specific ice zone GameObject is enabled).
/// </summary>
[RequireComponent(typeof(Collider))]
public class IceZoneActivator : MonoBehaviour
{
    [Tooltip("Optional: assign an IceKeyZone GameObject to enable when the player touches this activator.")]
    public GameObject zoneToEnable;

    void OnTriggerEnter(Collider other) // OnTriggerEnter is called when the player enters the trigger and disables the ice key zone
    {
        if (!other.CompareTag("Player")) return;

        if (zoneToEnable != null) // If the ice zone is not null, set it to active
            zoneToEnable.SetActive(true); // Set the ice zone to active

        if (GameManager.Instance != null){
            GameManager.Instance.SwitchToIceMovement(); // Switch to ice movement
            GameManager.Instance.hasKey = true; // Set the hasKey to true
            gameObject.SetActive(false); // Disable the game object
        }
    }
}
