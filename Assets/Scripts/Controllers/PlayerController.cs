using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isInputBlocked = false;

    public bool IsInputBlocked => isInputBlocked;

    public void BlockInput(bool block)
    {
        isInputBlocked = block;
    }

    public void ExecuteCommand(IGameplayCommand command, System.Action onComplete = null)
    {
        if (isInputBlocked)
            return;

        isInputBlocked = true;

        command.Execute();

        Invoke(nameof(UnblockInput), 0.3f);

        onComplete?.Invoke();
    }

    void UnblockInput()
    {
        isInputBlocked = false;
    }
}