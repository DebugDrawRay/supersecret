﻿using UnityEngine;
using System.Collections;

public static class EventManager
{
    public delegate void Trigger();
    public static event Trigger CollisionReaction;

    public static void TriggerCollision()
    {
        CollisionReaction();
    }

}