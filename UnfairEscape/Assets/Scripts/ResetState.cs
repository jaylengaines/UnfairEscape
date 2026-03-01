using UnityEngine;

public class ResetStae : MonoBehaviour
{
    // Resets the player back to original logic (normal movement) normal lightning
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null){
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
