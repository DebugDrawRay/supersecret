using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerTwoAxisAction Move;

    public PlayerActions()
    {
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    } 
}
