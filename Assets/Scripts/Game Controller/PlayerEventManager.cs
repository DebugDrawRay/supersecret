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
    public static event Trigger HitReaction;

    public static event Trigger ObjectCollision;
    public static event Trigger EnemyHit;
    public static event Collision EnemyCollision;

    public static void TriggerObjectCollision()
    {
        ObjectCollision();
    }
    public static void TriggerEnemyCollision(Vector3 from)
    {
        EnemyCollision(from);
        EnemyHit();
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
