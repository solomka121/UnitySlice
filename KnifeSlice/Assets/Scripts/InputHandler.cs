using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event System.Action OnScreenTouchDown; 
    public event System.Action OnScreenTouchUp;

    public void Init()
    {
        
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnScreenTouchDown?.Invoke();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            OnScreenTouchUp?.Invoke();
        }
    }
}
