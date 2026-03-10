using UnityEngine;
using UnityEngine.InputSystem;
public class FlashlLight : MonoBehaviour
{
    public Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.fKey.isPressed)
        {
            light.enabled = !light.enabled;
        }
    }
}
