using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject golfballPrefab;

    public GameState CurrentState { get; private set; }

    public static GameManager Instance { get; private set; }

    public GameObject Planet { get; private set; }

    public GameObject GolfBall { get; private set; }

    public PlayerController Controller { get; private set; }

    [SerializeField] private float gravitationalConstant;

    public float GravitationalConstant => gravitationalConstant;

    public int NumberOfHits { get; private set; }


    public enum GameState
    {
        Input,
        BallMovement,
        LevelComplete,
        MainMenu,
        LoadingLevel,
        LevelSelection
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Controller = GetComponent<PlayerController>();

        ONBallMovementEnter += initVelo =>
        {
            CurrentState = GameState.BallMovement;
            NumberOfHits += 1;
        };
        ONInputEnter += () =>
        {
            CurrentState = GameState.Input;
            
        };
        ONMainMenuEnter += () => CurrentState = GameState.MainMenu;
        ONLevelSelectEnter += () =>
        {
            CurrentState = GameState.LevelSelection;
        };
        ONLevelLoadEnter += id =>
        {
            var loadParams = new LoadSceneParameters(LoadSceneMode.Additive);
            var scene = SceneManager.LoadScene(id, loadParams);
            SceneManager.sceneLoaded += (loadedScene, loadSceneMode) =>
            {
                if (loadedScene == scene) StartLevel(scene);
            };
        };

        ONLevelCompleted += () => CurrentState = GameState.LevelComplete;

        ONLevelStart += scene =>
        {
            SceneManager.SetActiveScene(scene);

            var originPosition = GameObject.FindGameObjectWithTag("GolfBallOrigin").transform.position;

            GolfBall = Instantiate(golfballPrefab, originPosition, Quaternion.identity);

            Planet = GameObject.FindGameObjectWithTag("Planet");

            NumberOfHits = 0;
        };

        ONLevelStartExit += () => StartInput();

        ONBackToMainMenu += () =>
        {
            var scene = SceneManager.GetActiveScene();

            SceneManager.UnloadSceneAsync(scene);

            var loadParams = new LoadSceneParameters(LoadSceneMode.Single);
            var main = SceneManager.GetSceneByBuildIndex(0);
            SceneManager.SetActiveScene(main);

            StartMainMenu();
        };

    }

    private void Start()
    {
        Debug.Log("Start game");
        StartMainMenu();
    }

    public event Action<Vector3> ONBallMovementEnter;

    public event Action ONInputEnter;

    public event Action<Scene> ONLevelStart;

    public event Action ONLevelStartExit;

    public event Action ONMainMenuEnter;

    public event Action ONHowToEnter;

    public event Action ONLevelSelectEnter;

    public event Action<int>  ONLevelLoadEnter;

    public event Action ONLevelCompleted;

    public event Action ONBackToMainMenu;

    public void StartMainMenu() => ONMainMenuEnter?.Invoke();

    public void StartBallMovement(Vector3 initVelocity) => ONBallMovementEnter?.Invoke(initVelocity);

    public void StartInput() => ONInputEnter?.Invoke();

    public void LoadLevel(int id)
    {
        ONLevelLoadEnter?.Invoke(id);
    }

    public void CompleteLevel()
    {
        ONLevelCompleted?.Invoke();
    }

    public void BackToMainMenu()
    {
        Debug.Log("Back");
        ONBackToMainMenu?.Invoke();
    }


    public void StartLevel(Scene scene)
    {
        ONLevelStart?.Invoke(scene);

        ONLevelStartExit?.Invoke();
    }

    public void StartLevelSelection()
    {
        ONLevelSelectEnter?.Invoke();
    }

    public void StartHowTo()
    {
        ONHowToEnter?.Invoke();
    }

    public void QuitGame() => Application.Quit();

}


    
