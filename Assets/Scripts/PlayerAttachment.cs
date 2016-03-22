using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAttachment : ScriptableObject
{
    public enum slots
    {
        Head,
        Body,
        Bike,
        Left,
        Right
    }

    public slots slotType;
    public Material[] spriteMaterials;

    [System.Serializable]
    public class InteractionProperties
    {
        public Stats.stats statToAffect;
        [Range(-1, 1)]
        public float affectAmount; 
    }

    [SerializeField]
    public InteractionProperties[] interactions;
}
