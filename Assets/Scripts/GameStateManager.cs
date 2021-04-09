using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject golfballPrefab;
    
    public GameState CurrentState { get; private set; }

    public static GameStateManager Instance { get; private set; }

    public GameObject Planet { get; private set; }

    public GameObject GolfBall { get; private set; }

    public PlayerController Controller { get; private set; }


    public enum GameState
    {
        Input,
        BallMovement,
        LevelComplete
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Controller = GetComponent<PlayerController>();
        
        ONBallMovementEnter += initVelo => CurrentState = GameState.BallMovement;
        ONInputEnter += () => CurrentState = GameState.Input;
        ONLevelStart += () =>
        {
            Debug.Log("GameStateManager");
            var originPosition = GameObject.FindGameObjectWithTag("GolfBallOrigin").transform.position;

            GolfBall = Instantiate(golfballPrefab, originPosition, Quaternion.identity);

            Planet = GameObject.FindGameObjectWithTag("Planet");

            StartInput();
        };

    }

    private void Start()
    {
        StartLevel();
    }

    public event Action<Vector3> ONBallMovementEnter;

    public event Action ONInputEnter;

    public event Action ONLevelStart;

    public event Action ONLevelStartExit;
    
    public void StartBallMovement(Vector3 initVelocity) => ONBallMovementEnter?.Invoke(initVelocity);
    
    public void StartInput() => ONInputEnter?.Invoke();

    public void StartLevel()
    {
        ONLevelStart?.Invoke();

        ONLevelStartExit?.Invoke();
    }
}


    
