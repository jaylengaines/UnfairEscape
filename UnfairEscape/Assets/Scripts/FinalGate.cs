using UnityEngine;

public class FinalGate : MonoBehaviour
{
    public Transform gateVisual;
    public AudioSource gateAudio;
    public float openHeight = 3f;
    public float openSpeed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpening = false;
    private bool isOpen = false;
    private bool playOnce = false;

    void Start()
    {
        if (gateVisual == null) gateVisual = transform;
        if (gateAudio == null) gateAudio = GetComponent<AudioSource>();

        closedPos = gateVisual.position;
        openPos = closedPos + Vector3.up * openHeight;
    }

    void Update()
    {
        bool hasAllKeys = GameManager.Instance != null &&
                          GameManager.Instance.hasIceKey &&
                          GameManager.Instance.hasDarkZoneKey &&
                          GameManager.Instance.hasParkourKey;

        if (hasAllKeys && !isOpen && !isOpening) // if the gate has all keys and is not open and is not opening
        {
            isOpening = true; // set the is opening flag to true
            playOnce = true; // set the play once flag to true
        }

        if (!isOpening) return; // if the gate is not opening, return

        if (playOnce && gateAudio != null) // if the gate audio is not null and play once
        {
            gateAudio.Play(); // play the gate audio
            playOnce = false; // set the play once flag to false
        }

        gateVisual.position = Vector3.MoveTowards( // move the gate to the open position
            gateVisual.position, // the current position of the gate
            openPos, // the open position of the gate
            openSpeed * Time.deltaTime // the speed of the gate
        );

        if (Vector3.Distance(gateVisual.position, openPos) < 0.01f) // if the gate is at the open position
        {
            gateVisual.position = openPos; // set the position of the gate to the open position
            isOpening = false; // set the is opening flag to false
            isOpen = true; // set the is open flag to true
        }
    }
}