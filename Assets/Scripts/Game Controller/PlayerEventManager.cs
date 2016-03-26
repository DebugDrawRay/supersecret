using System.Collections;
using UnityEngine;
public class PlayerEventManager
{
    public delegate void Trigger();
    public delegate void Collision(Vector3 from);
    public static event Collision CollisionReaction;
    public static event Trigger TopSpeedEvent;
    public static event Trigger DeathEvent;
    public static event Trigger StunReaction;

    public static void TriggerCollision(Vector3 from)
    {
        CollisionReaction(from);
    }

    public static void TriggerStun()
    {
        StunReaction();
    }

    public static void TopSpeed()
    {
        TopSpeedEvent();
    }

    public static void PlayerDeath()
    {
        DeathEvent();
    }
}
