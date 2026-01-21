using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputSystem inputSystem;

    private void Awake()
    {
        inputSystem=new InputSystem();
        inputSystem.Enable();
    }
}
