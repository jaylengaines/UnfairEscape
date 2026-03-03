using System.Collections.Generic; // Lets us use List<Transform> so we can store multiple hallway chunks.
using UnityEngine; // Imports Unity types like MonoBehaviour, Transform, Vector3, Quaternion, Time, Collider.

[RequireComponent(typeof(Collider))] // Unity auto-adds/keeps a Collider on this GameObject so OnTriggerEnter/Exit can fire.
public class EndlessHallWay : MonoBehaviour // Inheriting MonoBehaviour lets Unity call lifecycle methods (Start, Update, trigger callbacks).
{
    [Header("References")] // Just a visual Inspector label; no runtime effect.
    public Transform player; // Player Transform: we read player.position each frame.
    public Transform winCollider; // Win trigger Transform: this is the object we move away from player.
    public Transform finalWinPoint; // Optional marker Transform: where winCollider returns when all keys are collected.
    public List<Transform> hallwaySegments = new List<Transform>(); // Ordered list of hallway chunks (near -> far) for recycling.

    [Header("Tuning")] // Another Inspector label for tweak values.
    public float followDistance = 20f; // Target distance (in world units) the winCollider stays in front of player.
    public float followSpeed = 8f; // Interpolation rate; higher value = faster catch-up in Lerp/Slerp.
    public float segmentLength = 20f; // Physical length of one hallway segment (must match your prefab length).
    public float recycleThreshold = 5f; // Buffer before segment end to trigger recycle early (reduces visible popping).

    public bool isPlayerInside = false; // Gate flag: true when player is inside this hallway trigger volume.
    private Vector3 hallwayForward; // Cached forward direction of hallway root (used as movement axis).
    private Vector3 originalWinPos; // Start position of winCollider, used as fallback return location.
    private Quaternion originalWinRot; // Start rotation of winCollider, used as fallback return rotation.

    void Start() // Unity calls this once when the script becomes active.
    {
        // If player isn't manually assigned in Inspector, try auto-linking from GameManager singleton.
        if (player == null && GameManager.Instance != null && GameManager.Instance.player != null)
            player = GameManager.Instance.player.transform; // Convert GameObject reference to Transform reference.

        hallwayForward = transform.right.normalized; // normalized => vector magnitude is 1, so distance math stays accurate.

        // Cache initial win collider transform so we can restore it when puzzle is solved.
        if (winCollider != null)
        {
            originalWinPos = winCollider.position; // Save initial position for later restore.
            originalWinRot = winCollider.rotation; // Save initial rotation for later restore.
        }
    }

    void Update() // Runs once per frame.
    {
        if (!isPlayerInside) return; // Early-out pattern: skip all work unless the player is currently in hallway.
        if (player == null || winCollider == null) return; // Safety guard: avoid NullReferenceException if refs are missing.

        if (HasAllKeys()) // If all keys are owned, stop endless behavior and return win target to true end.
        {
            // Ternary operator (condition ? valueIfTrue : valueIfFalse):
            // If finalWinPoint exists, use it; otherwise use original start transform as fallback.
            Vector3 targetPos = finalWinPoint != null ? finalWinPoint.position : originalWinPos;
            Quaternion targetRot = finalWinPoint != null ? finalWinPoint.rotation : originalWinRot;

            // Vector3.Lerp(current, target, t):
            // - current = where object is now
            // - target = where object should go
            // - t = fraction to move this frame (followSpeed * deltaTime)
            // Repeating this every frame creates smooth motion.
            winCollider.position = Vector3.Lerp(winCollider.position, targetPos, followSpeed * Time.deltaTime);

            // Quaternion.Slerp(currentRot, targetRot, t):
            // Same idea as Lerp but for rotation, using spherical interpolation for smooth angle change.
            // Must assign result back to winCollider.rotation or it has no effect.
            winCollider.rotation = Quaternion.Slerp(winCollider.rotation, targetRot, followSpeed * Time.deltaTime);

            return; // Important: exits Update now so "endless" section below does NOT run once solved.
        }

        // targetWinPos = player position + (hallway axis * distance ahead)
        // This keeps the winCollider a fixed distance in front of player while keys are missing.
        Vector3 targetWinPos = player.position + hallwayForward * followDistance;

        // Smoothly move winCollider toward that target each frame.
        winCollider.position = Vector3.Lerp(winCollider.position, targetWinPos, followSpeed * Time.deltaTime);

        RecycleSegmentsIfNeeded(); // Move passed segments to the front to fake an "endless" corridor.
    }

    void OnTriggerEnter(Collider other) // Unity callback when any collider enters this trigger collider.
    {
        if (!other.CompareTag("Player")) return; // CompareTag is faster/safer than string tag equality checks.
        isPlayerInside = true; // Enable hallway logic.
    }

    void OnTriggerExit(Collider other) // Unity callback when any collider exits this trigger collider.
    {
        if (!other.CompareTag("Player")) return; // Only react to player.
        isPlayerInside = false; // Disable hallway logic outside the trigger.
    }

    bool HasAllKeys() // Helper method centralizes key requirements in one place.
    {
        if (GameManager.Instance == null) return false; // Defensive programming: fail safe if singleton is missing.
        return GameManager.Instance.hasIceKey // Logical AND: all three booleans must be true.
            && GameManager.Instance.hasDarkZoneKey
            && GameManager.Instance.hasParkourKey;
    }

    void RecycleSegmentsIfNeeded() // Reposition front segment to back after player passes it.
    {
        // Need at least 2 segments to recycle; with 0/1 segments there is nothing meaningful to shift.
        if (hallwaySegments == null || hallwaySegments.Count < 2 || player == null) return;

        Transform first = hallwaySegments[0]; // First element in list = oldest/nearest segment.
        Transform last = hallwaySegments[hallwaySegments.Count - 1]; // Last element in list = farthest segment.

        // Dot product projects player offset onto hallwayForward axis.
        // In this use-case it answers: "How far has player advanced along hallway direction from first segment?"
        float alongFirst = Vector3.Dot(player.position - first.position, hallwayForward);

        // If player is near/past the end of first segment, recycle it to front.
        if (alongFirst > segmentLength - recycleThreshold)
        {
            // Place recycled segment one segment-length after current last segment.
            first.position = last.position + hallwayForward * segmentLength;
            first.rotation = last.rotation; // Copy rotation so seams stay aligned.

            hallwaySegments.RemoveAt(0); // Remove old index 0 from list order.
            hallwaySegments.Add(first); // Append recycled segment as new tail.
        }
    }
}
