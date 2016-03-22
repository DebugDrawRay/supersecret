using UnityEngine;

public class PlayerAttachmentController : MonoBehaviour
{
    public enum state
    {
        Idle,
        Collision,
        Destroyed
    }

    public state currentState;

    private bool initialized;

    void Init()
    {
        initialized = true;
    }
    void Update()
    {
        if(initialized)
        {
            RunStates();
        }
    }
    void RunStates()
    {
        switch(currentState)
        {
            case state.Idle:
                break;
            case state.Collision:
                break;
            case state.Destroyed:
                break;
        }
    }
}
