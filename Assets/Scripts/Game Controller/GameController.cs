﻿using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum State
    {
        StartGame,
        InGame,
        Pause,
        NextLevel,
        GameOver
    }
    [Header("Game State")]
    public State currentState;

    private GameSettings settings;

    [Header("Grid Setup")]
    public GameObject gridSystem;
    private Grid grid;
    public Vector3 startPosition;

    [Header("Player Setup")]
    public GameObject playerCharacter;
    public GameObject mainCamera;

    private PlayerController player;
    private QuadCam camera;

    [Header("Level Setup")]
    public LevelBuilder builder;

    void Awake()
    {
        Setup();
    }

    void Setup()
    {
        grid = Instantiate(gridSystem).GetComponent<Grid>();
        grid.transform.position = startPosition;

        player = Instantiate(playerCharacter).GetComponent<PlayerController>();
        player.Init(grid);

        camera = Instantiate(mainCamera).GetComponent<QuadCam>();
        camera.Init(grid.transform, player.transform);

        builder.Init();

    }

    void Update()
    {
        RunStates();
    }

    void RunStates()
    {
        switch(currentState)
        {
            case State.StartGame:
                currentState = State.InGame;
                break;
            case State.InGame:
                player.currentState = PlayerController.State.InGame;
                break;
            case State.Pause:
                break;
            case State.NextLevel:
                break;
            case State.GameOver:
                break;
        }
    }
}
