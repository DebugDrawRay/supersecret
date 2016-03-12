using System.Collections;
using UnityEngine;
public class EventManager
{
    public delegate void Trigger();
    public static event Trigger CollisionReaction;
    public static event Trigger TopSpeedEvent;

    public static void TriggerCollision()
    {
        CollisionReaction();
    }

    public static void TopSpeed()
    {
        TopSpeedEvent();
    }
}
