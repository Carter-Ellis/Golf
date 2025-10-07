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

    public void abilityButton()
    {
        PlayerInput.sendInput(PlayerInput.Axis.Fire3);
    }

    public void swapUpButton()
    {
        PlayerInput.sendInput(PlayerInput.Axis.SwapUp);
    }

    public void swapDownButton()
    {
        PlayerInput.sendInput(PlayerInput.Axis.SwapDown);
    }

}
