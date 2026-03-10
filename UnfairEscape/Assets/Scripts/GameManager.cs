using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get; private set;}
    [Header("Player")]
    public GameObject player;
    private PlayerMovement normalMovement;
    private PlayerMovementIce iceMovement;

    public bool hasIceKey = false;

    public GameObject darkZoneDoor; // the door to the dark zone

    public GameObject OtherCellDoor;

    public GameObject parkourDoor;

    public GameObject finalDoor;

    public GameObject batonUi;

    public bool hasDarkZoneKey = false;

    public bool hasParkourKey = false;
    public GameObject darkZone;
    public GameObject parkourZone;

    [Header("Pause Logic")]
    // 1) Add these fields near your other variables
    public bool IsPaused { get; private set; } = false; // is paused is false by default
    public Key pauseKey = Key.Escape; // pause key is escape (Input System)
    public GameObject pauseMenuUI; // pause menu ui is optional, can be null
   
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
    void Update(){
        if (Keyboard.current != null && Keyboard.current[pauseKey].wasPressedThisFrame){
            TogglePause();
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
    // Disable specific door based of key grabbed
    public void DisableDarkZoneDoor(){
        darkZoneDoor.SetActive(false);
    }
    public void DisableOtherCellDoor(){
        OtherCellDoor.SetActive(false);
    }
    public void DisableParkourDoor(){
        parkourDoor.SetActive(false);
    }
    public void DisableFinalDoor(){
        finalDoor.SetActive(false);
    }
    public void EnableBatonUi(){
        batonUi.SetActive(true);
    }
    public void DisableBatonUi(){
        batonUi.SetActive(false);
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
    public void TogglePause(){
        if(IsPaused) ResumeGame();
        else PauseGame();
    }
    public void PauseGame(){
    IsPaused = true;
    Time.timeScale = 0f;
    if(pauseMenuUI != null){
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    }
    public void ResumeGame(){
        IsPaused = false;
        Time.timeScale = 1f;
        if(pauseMenuUI != null){
            pauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}