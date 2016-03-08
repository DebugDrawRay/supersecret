using UnityEngine;
using System.Collections;

public interface IActionController
{
    void Action(Vector2 axis);
    void Action(bool trigger);
}
