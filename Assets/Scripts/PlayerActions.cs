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

    public static PlayerActions BindActionsWithJoystick()
    {
        PlayerActions newActions = new PlayerActions();

        newActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        newActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        newActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        newActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        return newActions;
    }

    public static PlayerActions BindActionsWithKeyboard()
    {
        PlayerActions newActions = new PlayerActions();

        newActions.Up.AddDefaultBinding(Key.W);
        newActions.Down.AddDefaultBinding(Key.S);
        newActions.Left.AddDefaultBinding(Key.A);
        newActions.Right.AddDefaultBinding(Key.D);

        return newActions;
    }

    public static PlayerActions BindKeyboardAndJoystick()
    {
        PlayerActions newActions = new PlayerActions();

        newActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        newActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        newActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        newActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        newActions.Up.AddDefaultBinding(Key.W);
        newActions.Down.AddDefaultBinding(Key.S);
        newActions.Left.AddDefaultBinding(Key.A);
        newActions.Right.AddDefaultBinding(Key.D);

        return newActions;
    }
}
