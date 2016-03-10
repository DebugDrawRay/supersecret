using UnityEngine;
using System.Collections;

[System.Serializable]
public class BiomeSet : ScriptableObject
{
    public string name = "New Biome Set";
    public Material roadMaterial;
    public Material wallMaterial;
    public Material groundMaterial;

    public GameObject horizonObject;

    public GameObject[] wallProps;
    public GameObject[] groundProps;
    public GameObject[] obstacles;
    public GameObject[] enemies;
}
