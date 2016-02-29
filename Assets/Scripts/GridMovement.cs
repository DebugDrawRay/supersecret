using UnityEngine;
using InControl;
using Spine;
public class GridMovement : MonoBehaviour
{
    public Vector2 startPosition;

    private Vector2 currentPosition;

    private Grid targetGrid;
    private bool initialized;

    private PlayerActions input;
    private bool directionHeld;

    private SkeletonAnimation anim;
    private TrackEntry lean;

    void Start()
    {
        targetGrid = Grid.instance;
        currentPosition = startPosition;

        if (targetGrid)
        {
            transform.position = GridPostion(targetGrid, currentPosition.x, currentPosition.y);
            initialized = true;
        }
        else
        {
            Debug.LogError("No Grid Found, Was It Initialized?");
        }
    }

    Vector3 GridPostion(Grid grid, float xPos, float yPos)
    {
        int newX = Mathf.RoundToInt(xPos);
        int newY = Mathf.RoundToInt(yPos);

        return grid.gridUnits[newX, newY].position;
    }

    public void MovementListener(Vector2 axis)
    {
        float x = axis.x;
        float y = axis.y;

        if (!directionHeld)
        {
            if (x > .2f)
            {
                currentPosition.x += 1;
                directionHeld = true;

            }
            if (x < -.2f)
            {
                currentPosition.x -= 1;
                directionHeld = true;
            }
            if (y > .2f)
            {
                currentPosition.y += 1;
                directionHeld = true;
            }
            if (y < -.2f)
            {
                currentPosition.y -= 1;
                directionHeld = true;
            }
        }
        else
        {
            if(x < .2f && x > -.2f && y < .2f && y > -.2f)
            {
                directionHeld = false;
            }
        }

        if(currentPosition.x < 0)
        {
            currentPosition.x = 0;
        }
        if(currentPosition.x >= targetGrid.xUnits)
        {
            currentPosition.x = targetGrid.xUnits - 1;
        }
        if (currentPosition.y < 0)
        {
            currentPosition.y = 0;
        }
        if (currentPosition.y >= targetGrid.yUnits)
        {
            currentPosition.y = targetGrid.yUnits - 1;
        }

        Vector3 newPosition = GridPostion(targetGrid, currentPosition.x, currentPosition.y);
        if(transform.localPosition != newPosition)
        {
            transform.localPosition = newPosition;
        }

    }

}
