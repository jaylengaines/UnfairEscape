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

        if (hasAllKeys && !isOpen && !isOpening)
        {
            isOpening = true;
            playOnce = true;
        }

        if (!isOpening) return;

        if (playOnce && gateAudio != null)
        {
            gateAudio.Play();
            playOnce = false;
        }

        gateVisual.position = Vector3.MoveTowards(
            gateVisual.position,
            openPos,
            openSpeed * Time.deltaTime
        );

        if (Vector3.Distance(gateVisual.position, openPos) < 0.01f)
        {
            gateVisual.position = openPos;
            isOpening = false;
            isOpen = true;
        }
    }
}