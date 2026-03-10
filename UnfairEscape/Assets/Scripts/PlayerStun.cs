using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerStun : MonoBehaviour
{
    [Header("Stun")]
    public float stunDuration = 1f;
    public float stunCooldown = 1f;
    public float colliderActiveTime = 0.2f;
    public Collider stunCollider;

    private float nextStunTime = 0f;

    void Start()
    {
        if (stunCollider == null)
            stunCollider = GetComponent<Collider>();

        if (stunCollider != null)
            stunCollider.enabled = false;
    }

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame &&
            Time.time >= nextStunTime)
        {
            nextStunTime = Time.time + stunCooldown;
            StartCoroutine(Stun());
            Debug.Log("Stunned");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyStun enemyStun = other.GetComponent<EnemyStun>(); // get the enemy stun component
        if (enemyStun != null) // if the enemy stun component is not found, return 
            enemyStun.Stun(stunDuration); // stun the enemy
    }

    private IEnumerator Stun()
    {
        if (stunCollider == null) yield break; // if the stun collider is not found, return

        stunCollider.enabled = true; // enable the stun collider
        yield return new WaitForSeconds(colliderActiveTime);
        stunCollider.enabled = false; // disable the stun collider
    }

    public void IncreaseStunDuration(float amount)
    {
        stunDuration += amount; // increase the stun duration
    }
}