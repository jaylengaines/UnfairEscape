using UnityEngine;
using System.Collections.Generic;

public class ParkourZone : MonoBehaviour
{
    // Resets the player back to original logic (normal movement) normal lightning
    public GameObject player;
    public List<GameObject> shootersToEnable = new List<GameObject>();

    public void EnableParkourMovement(GameObject playerObject){
        if (playerObject == null)
        {
            Debug.LogWarning("ParkourZone: playerObject is null, cannot switch movement.");
            return;
        }

        PlayerMovement normal = playerObject.GetComponentInParent<PlayerMovement>();
        PlayerMovementIce ice = playerObject.GetComponentInParent<PlayerMovementIce>();
        PlayerMovementParkour parkour = playerObject.GetComponentInParent<PlayerMovementParkour>();

        if (normal != null) normal.enabled = false;
        if (ice != null) ice.enabled = false;
        if (parkour != null) parkour.enabled = true;

        if (GameManager.Instance != null)
            GameManager.Instance.hasParkourKey = true;
    }

    public void EnableShooters()
    {
        foreach (GameObject shooter in shootersToEnable)
        {
            if (shooter != null)
                shooter.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            GameObject playerObject = player != null ? player : other.gameObject;
            EnableParkourMovement(playerObject);
            EnableShooters();
        }
    }
}
