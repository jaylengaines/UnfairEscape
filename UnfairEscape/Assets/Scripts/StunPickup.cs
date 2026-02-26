using UnityEngine;

public class StunPickup : MonoBehaviour
{
    public float stunBonus = .5f;// the amount of stun bonus to add to the player
    private void OnTriggerEnter(Collider other){
        if(!other.CompareTag("Player")) return; // if the object is not the player, return
        PlayerStun playerStun = other.GetComponentInParent<Transform>().GetComponentInChildren<PlayerStun>();
        if (playerStun != null)
        {
            playerStun.IncreaseStunDuration(stunBonus); // increase the stun duration
            Destroy(gameObject); // destroy the pickup
        }
    }
}
