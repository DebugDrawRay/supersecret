using UnityEngine;
using System.Collections;

public class PlayerHUDController : MonoBehaviour
{
    public GameObject RestartPanel;

    void Awake()
    {
        RestartPanel.SetActive(false);
        PlayerEventManager.DeathEvent += ShowRestartPanel;
    }

    void ShowRestartPanel()
    {
        RestartPanel.SetActive(true);
    }
}
