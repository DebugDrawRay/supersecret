using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public AutoGridMovement movement;
    public Stats stat;

    private PlayerController target;

    protected bool initialized;

    void Init(PlayerController player)
    {
        if (movement && stat)
        {
            target = player;
            initialized = true;
        }
        else
        {
            Debug.LogError("No components found, check GameObject references");
        }
    }

}
