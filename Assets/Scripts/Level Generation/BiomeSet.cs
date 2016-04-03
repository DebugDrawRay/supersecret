using UnityEngine;

[System.Serializable]
public class BiomeSet : ScriptableObject
{
    public GameObject roadObject;
    public GameObject wallObject;
    public GameObject groundObject;
    public GameObject horizonObject;
    public GameObject[] wallProps;
    public GameObject[] archProps;
    public GameObject[] groundProps;
    public GameObject[] obstacles;
    public GameObject[] enemies;
}
