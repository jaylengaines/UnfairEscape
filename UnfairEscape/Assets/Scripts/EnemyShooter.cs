using UnityEngine; // Unity core types (Transform, GameObject, MonoBehaviour, etc.)

public class EnemyShooter : MonoBehaviour // Script for a stationary enemy turret
{
    [Header("Targeting")] // Inspector section label
    public Transform player; // Reference to player transform so turret can look at player
    public float rotationSpeed = 5f; // How fast turret rotates toward player

    [Header("Shooting")] // Inspector section label
    public GameObject bulletPrefab; // Prefab to spawn when firing
    public Transform firePoint; // Position/rotation where bullets spawn
    public float fireRate = 1f; // Shots per second (editable in Inspector)
    public float bulletSpeed = 20f; // Speed passed to bullet movement script

    private float nextFireTime = 0f; // Time gate so we fire based on fireRate

    void Start() // Called once when object is enabled
    {
        // Optional auto-find in case player is not assigned manually
        if (player == null && GameManager.Instance != null && GameManager.Instance.player != null)
            player = GameManager.Instance.player.transform; // Pull player transform from GameManager
    }

    void Update() // Called every frame
    {
        if (player == null) return; // Safety check so script doesn't null error

        // Direction from turret to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Build desired rotation looking at player direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate from current to target (no movement, only rotation)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Fire if current time passed our cooldown gate
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate); // Convert shots/sec into delay per shot
            Shoot(); // Spawn a bullet
        }
    }

    void Shoot() // Handles bullet spawn
    {
        if (bulletPrefab == null || firePoint == null) return; // Safety guard if refs missing

        // Instantiate bullet at fire point position and orientation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Grab bullet script and pass speed (if your bullet has this script)
        BulletProjectile projectile = bullet.GetComponent<BulletProjectile>();
        if (projectile != null)
            projectile.SetSpeed(bulletSpeed); // Set bullet speed from turret inspector value
    }
}