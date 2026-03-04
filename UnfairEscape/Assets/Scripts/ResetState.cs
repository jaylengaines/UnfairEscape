using UnityEngine;

public class ResetStae : MonoBehaviour
{
    // Resets the player back to original logic (normal movement) normal lightning
    public GameObject player;
    public void ResetState(){
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovementIce>().enabled = false;
    }
    public void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            ResetState();
        }
    }
}
