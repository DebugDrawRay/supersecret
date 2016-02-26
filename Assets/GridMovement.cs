using UnityEngine;
using InControl;
using Spine;
public class GridMovement : MonoBehaviour
{
    private Grid targetGrid;

    private bool initialized;

    private PlayerActions input;
    private bool directionHeld;

    public Vector2 gridPosition;

    private SkeletonAnimation anim;
    private TrackEntry lean;
    public void Init(Grid grid, Vector2 position)
    {
        anim = GetComponent<SkeletonAnimation>();
        anim.state.SetAnimation(0, "driving", true);
        lean = anim.state.AddAnimation(1, "lean_right", false, 0);
        targetGrid = grid;
        gridPosition = position;

        transform.position = targetGrid.gridUnits[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].position;

        input = new PlayerActions();

        input.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        input.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        input.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        input.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        input.Up.AddDefaultBinding(Key.W);
        input.Down.AddDefaultBinding(Key.S);
        input.Left.AddDefaultBinding(Key.A);
        input.Right.AddDefaultBinding(Key.D);

        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            MovementListener();
        }
        transform.localPosition = targetGrid.gridUnits[Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y)].position;
    }

    void MovementListener()
    {
        float x = input.Move.X;
        float y = input.Move.Y;

        float move = Mathf.Clamp(x, 0, 1);
        float time = Mathf.Clamp(lean.endTime, 0, 1f);
        Debug.Log(time * move);
        lean.time = lean.endTime * move;

        if (!directionHeld)
        {
            if (x > .2f)
            {
                gridPosition.x += 1;
                directionHeld = true;

            }
            if (x < -.2f)
            {
                gridPosition.x -= 1;
                directionHeld = true;
            }
            if (y > .2f)
            {
                gridPosition.y += 1;
                directionHeld = true;
            }
            if (y < -.2f)
            {
                gridPosition.y -= 1;
                directionHeld = true;
            }
        }
        else
        {
            if(x < .5f && x > -.5f && y < .5f && y > -.5f)
            {
                directionHeld = false;
            }
        }

        if(gridPosition.x < 0)
        {
            gridPosition.x = 0;
        }
        if(gridPosition.x >= targetGrid.xUnits)
        {
            gridPosition.x = targetGrid.xUnits - 1;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = 0;
        }
        if (gridPosition.y >= targetGrid.yUnits)
        {
            gridPosition.y = targetGrid.yUnits - 1;
        }
    }

}
