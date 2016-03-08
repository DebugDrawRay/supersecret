using System.Collections;
using UnityEngine;
public class EventManager
{
    public delegate void Trigger();
    public static event Trigger CollisionReaction;

    public static void TriggerCollision()
    {
        CollisionReaction();
    }
}
