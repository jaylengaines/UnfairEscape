using UnityEngine;

/// <summary>
/// Put this on the GameObject with the IceKey trigger collider (sphere).
/// When the player enters the trigger, movement switches to ice; when they leave, it switches back.
/// </summary>
[RequireComponent(typeof(Collider))]
public class IceKeyZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null)
            GameManager.Instance.SwitchToIceMovement();

        
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null)
            GameManager.Instance.SwitchToNormalMovement();
    }
}
