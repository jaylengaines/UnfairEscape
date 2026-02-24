using UnityEngine;

public class EnemyStun : MonoBehaviour
{
public bool isStunned;
public float stunDuration;
public AudioSource stunAudio;
    void Update()
    {
        if (isStunned) // if the enemy is stunned
        {
            stunDuration -= Time.deltaTime; // decrease the stun duration
            if (stunDuration <= 0f) // if the stun duration is less than or equal to 0
            {
                isStunned = false; // set the enemy to not stunned
            }
        }
    }

    public void Stun(float duration) // stun the enemy for the amount of the duration
    {
        isStunned = true; // set the enemy to stunned
        stunDuration = duration; // set the stun duration to the duration
        stunAudio.Play(); // play the stun audio
    }
}
