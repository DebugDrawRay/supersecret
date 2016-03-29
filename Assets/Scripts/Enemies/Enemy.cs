using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public AutoGridMovement movement;
    public Stats stats;

    protected Grid targetGrid;
    protected PlayerController target;
    protected bool initialized;
    protected bool enteredGrid;

    protected delegate void CollisionTrigger();
    protected CollisionTrigger Collision;

    public void Init()
    {
        targetGrid = Grid.instance;
        target = PlayerController.instance;
        movement.Init(targetGrid, stats);
        initialized = true;
    }

    protected void CheckGridPosition()
    {
        if (targetGrid.transform.position.z >= transform.position.z + 100 && !enteredGrid)
        {
            movement.EnterGrid();
            enteredGrid = true;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();
        if(isPlayer)
        {
            Collision();
            ContestSpace(isPlayer.GetComponent<Stats>());
        }
    }

    public void ContestSpace(Stats challenger)
    {
        float attack = stats.speed + stats.agility + stats.weight + stats.distanceTraveled;
        float defense = challenger.speed + challenger.agility + challenger.weight + challenger.distanceTraveled;

        if (attack < defense)
        {
            Debug.Log(name + " loses!");

            if (challenger.distanceTraveled > challenger.minRequiredDistanceTraveled)
            {
                movement.ForcedMove(challenger.transform.localPosition);
            }
        }
        else
        {
            movement.ForcedMove(targetGrid.GetClosestUnit(transform.localPosition));
            Debug.Log(name + " wins!");
        }
    }

}
