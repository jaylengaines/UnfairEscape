using UnityEngine;

public class ParkourZone : MonoBehaviour
{
    // Resets the player back to original logic (normal movement) normal lightning
    public GameObject player;
    public void EnableParkourMovement(){
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerMovementIce>().enabled = false;
        player.GetComponent<PlayerMovementParkour>().enabled = true;
        GameManager.Instance.hasParkourKey = true;
    }
    public void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            EnableParkourMovement();
        }
    }
}
