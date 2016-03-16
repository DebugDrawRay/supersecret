using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public AutoGridMovement movement;
    public Stats stat;

    protected Grid targetGrid;
    protected PlayerController target;
    protected bool initialized;
    protected bool enteredGrid;

    public void Init()
    {
        targetGrid = Grid.instance;
        target = PlayerController.instance;
        movement.Init(targetGrid);
        initialized = true;
    }

    protected void CheckGridPosition()
    {
        if (targetGrid.transform.position.z >= transform.position.z + 50 && !enteredGrid)
        {
            movement.EnterGrid();
            enteredGrid = true;
        }
    }
}
