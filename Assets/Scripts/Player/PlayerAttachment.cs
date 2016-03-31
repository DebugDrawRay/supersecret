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

    [SerializeField]
    public InteractionProperty[] interactions;

    [SerializeField]
    public StatsCollection collection;
}
