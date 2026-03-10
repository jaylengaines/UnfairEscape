using UnityEngine;

public class StunPickup : MonoBehaviour
{
    public float stunBonus = 5f; // the amount of stun bonus to add to the player

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // if the object is not the player, return

        PlayerStun playerStun = null;

        if (GameManager.Instance != null && GameManager.Instance.player != null)
            playerStun = GameManager.Instance.player.GetComponentInChildren<PlayerStun>();

        if (playerStun == null)
            playerStun = other.GetComponentInParent<PlayerStun>();

        if (playerStun != null)
        {
            playerStun.IncreaseStunDuration(stunBonus); // increase the stun duration
            gameObject.SetActive(false); // disable pickup after collecting (same pattern as key)
        }
    }
}
