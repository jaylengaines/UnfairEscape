using UnityEngine;

public class Win : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.hasKey){
            GameManager.Instance.WinGame();
        }
    }
}
