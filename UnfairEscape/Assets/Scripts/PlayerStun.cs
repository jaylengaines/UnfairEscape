using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerStun : MonoBehaviour
{
    // Stun mechanics
    public float stunDuration; // The duration of the stun
    public float stunCooldown;
    public Collider stunCollider; // The collider of the stun
    private float nextStunTime;
    public float colliderActiveTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     // gets the stun collider and disables it    
     stunCollider = GetComponent<Collider>();
     stunCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
     // if player presses right click then activates the collider only while the player presses the key when they let go it is disabled for the amount of the cooldown
     if (Mouse.current.rightButton.isPressed)
     {
        nextStunTime = Time.time + stunCooldown; // sets the next stun time to the current time + the stun cooldown
        StartCoroutine(Stun()); // starts the stun coroutine and disables the collider for the amount of the stun duration
     } 
    

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // stun the enemy
            other.gameObject.GetComponent<EnemyStun>().Stun(stunDuration);
        }
    }
    private IEnumerator Stun(){ // stun the enemy for the amount of the collider active time
        stunCollider.enabled = true;
        // wait for the stun duration
        yield return new WaitForSeconds(colliderActiveTime);
        // disable the collider
        stunCollider.enabled = false;
    }
}
