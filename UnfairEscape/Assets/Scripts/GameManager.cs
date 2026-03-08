using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get; private set;}
    [Header("Player")]
    public GameObject player;
    private PlayerMovement normalMovement;
    private PlayerMovementIce iceMovement;

    public bool hasIceKey = false;
    public bool hasDarkZoneKey = false;

    public bool hasParkourKey = false;
    public GameObject darkZone;
    public GameObject parkourZone;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start(){
        if (player == null){
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player != null){
            normalMovement = player.GetComponent<PlayerMovement>();
            iceMovement = player.GetComponent<PlayerMovementIce>();
        }
    }

    public void SwitchToIceMovement(){
        normalMovement.enabled = false;
        iceMovement.enabled = true;
    }
    public void SwitchToNormalMovement(){
        normalMovement.enabled = true;
        iceMovement.enabled = false;
    }

    public void WinGame(){
        Debug.Log("You win!");
        SceneManager.LoadScene("WinScreen");
    }
    public void EnableDarkZone(){
        hasDarkZoneKey = true;
        // enable the dark zone
        darkZone.SetActive(true);
    }
    public void DisableDarkZone(){
        // disable the dark zone
        darkZone.SetActive(false);
    }
}