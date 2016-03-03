using UnityEngine;
using System.Collections;

public class GridUnit : MonoBehaviour
{
    [Header("Currently Occupied Properties")]
    public SpriteRenderer glow;
    public Color unoccupiedColor;
    public Color occupiedColor;

    void Start()
    {
        glow.color = unoccupiedColor;
    }
    void OnTriggerStay(Collider hit)
    {
        GridUnit isUnit = hit.GetComponent<GridUnit>();
        if(!isUnit)
        {
            glow.color = occupiedColor;
        }
    }
    void OnTriggerExit(Collider hit)
    {
        glow.color = unoccupiedColor;
    }
}
