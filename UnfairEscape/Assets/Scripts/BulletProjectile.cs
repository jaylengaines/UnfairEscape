using UnityEngine; // Unity core types
using UnityEngine.SceneManagement;
public class BulletProjectile : MonoBehaviour // Script placed on bullet prefab
{
    public float speed = 20f; // Movement speed (can be overridden by shooter)
    public float lifeTime = 5f; // Auto-destroy timer so bullets don't live forever
    public int damage = 1; // Optional damage amount

    void Start() // Called when bullet is spawned
    {
        Destroy(gameObject, lifeTime); // Cleanup bullet after lifetime
    }

    void Update() // Called every frame
    {
        // Move bullet forward in its own local forward direction
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSpeed(float newSpeed) // Called by shooter after Instantiate
    {
        speed = newSpeed; // Apply shooter-defined bullet speed
    }

    // We intentionally ignore trigger colliders so zone/room triggers do not delete bullets.
    void OnTriggerEnter(Collider other) { }

    void OnCollisionEnter(Collision collision){ // Destroy bullet on physical collision only
        if (collision.gameObject.CompareTag("Player")){
            SceneLoader.ApplyCursorForScene("GameOverScene");
            SceneManager.LoadScene("GameOverScene");
        }

        Destroy(gameObject); // Destroy on any non-trigger collision (walls, props, player)
    }
}