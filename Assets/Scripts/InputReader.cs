using UnityEngine;

public class InputReader : IInputReader
{
    public bool IsInverted { get; set; } = false;
    public float GetVerticalInput()
    {
        float input = Input.GetAxis("Vertical");
        return IsInverted ? -input : input;
    }
}