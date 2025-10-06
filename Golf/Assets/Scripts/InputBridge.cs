using UnityEngine;

public class InputBridge : MonoBehaviour
{
    
    public void resetButton()
    {
        PlayerInput.sendInput(PlayerInput.Axis.Reset);
    }

    public void menuButton()
    {
        PlayerInput.sendInput(PlayerInput.Axis.Cancel);
    }

}
