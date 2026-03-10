using UnityEngine;

public class ParkourZoneActivator : MonoBehaviour
{
    public GameObject zoneToEnable;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneToEnable != null)
            zoneToEnable.SetActive(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasParkourKey = true; // optional if you want dark zone gated behind key
            gameObject.SetActive(false);           // one-time pickup style
            if(GameManager.Instance.hasParkourKey) GameManager.Instance.DisableFinalDoor();
        }
    }
}
