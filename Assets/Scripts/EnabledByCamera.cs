using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnabledByCamera : MonoBehaviour
{
    private Color fullColor;
    [SerializeField]
    public List<MonoBehaviour> components;
    public Renderer render;
    private Color trueColor;
    void Awake()
    {
        ChangeComponentState(false);
        trueColor = render.material.color;
        Color clearColor = trueColor;
        clearColor.a = 0;
        render.material.color = clearColor;
    }
    void ChangeComponentState(bool active)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].enabled = active;
        }
    }

    void Update()
    {
        if(render.isVisible)
        {
            ChangeComponentState(true);
            Color newColor = Color.Lerp(render.material.color, trueColor, .01f);
            render.material.color = newColor;

        }
    }
}
