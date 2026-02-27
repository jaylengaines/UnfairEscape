using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DarkZoneActivator : MonoBehaviour
{
    [Tooltip("Optional: assign a DarkZone collider GameObject to enable when player touches this activator.")]
    public GameObject zoneToEnable;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneToEnable != null)
            zoneToEnable.SetActive(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasIceKey = true; // optional if you want dark zone gated behind key
            gameObject.SetActive(false);           // one-time pickup style
        }
    }
}