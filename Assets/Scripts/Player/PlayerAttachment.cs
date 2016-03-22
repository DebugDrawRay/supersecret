using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAttachment : ScriptableObject
{
    public PlayerSkins.Bike bikeSkin;
    public PlayerSkins.Body bodySkin;
    public PlayerSkins.Head headSkin;
    public PlayerSkins.Left leftSkin;
    public PlayerSkins.Right rightSkin;

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
