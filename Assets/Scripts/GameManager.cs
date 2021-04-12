using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // the prefab that should be used for the golfball
    [SerializeField] private GameObject golfballPrefab;

    // the current state the game is in
    public GameState CurrentState { get; private set; }

    // singleton instance to access gamemanager from any behaviour
    public static GameManager Instance { get; private set; }

    // reference to the planet
    public GameObject Planet { get; private set; }

    // reference to the golfball
    public GameObject GolfBall { get; private set; }

    // reference to the player controller
    public PlayerController Controller { get; private set; }

    // how strong gravity should be
    [SerializeField] private float gravitationalConstant;

    // getter property for grav constant
    public float GravitationalConstant => gravitationalConstant;

    // how many times the player has hit the ball
    public int NumberOfHits { get; private set; }

    // enumeration of all possible game states
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
        // initialize singleton instance if its null
        if (Instance == null)
        {
            Instance = this;
        }

        // get player controller
        Controller = GetComponent<PlayerController>();

        // when ballmovement starts enter ball movement state and increase hit counter
        ONBallMovementEnter += initVelo =>
        {
            CurrentState = GameState.BallMovement;
            NumberOfHits += 1;
        };
        // when input starts enter input state
        ONInputEnter += () =>
        {
            CurrentState = GameState.Input;
            
        };
        // when main menu starts enter main menu state
        ONMainMenuEnter += () => CurrentState = GameState.MainMenu;
        // when level selection starts enter level selection state
        ONLevelSelectEnter += () =>
        {
            CurrentState = GameState.LevelSelection;
        };
        // load level and start level when done loading
        ONLevelLoadEnter += id =>
        {
            var loadParams = new LoadSceneParameters(LoadSceneMode.Additive);
            var scene = SceneManager.LoadScene(id, loadParams);
            SceneManager.sceneLoaded += (loadedScene, loadSceneMode) =>
            {
                if (loadedScene == scene) StartLevel(scene);
            };
        };
        // when level completed enter level complete state
        ONLevelCompleted += () => CurrentState = GameState.LevelComplete;

        // when level is started set active scene, instantiate golfball and find planet
        ONLevelStart += scene =>
        {
            SceneManager.SetActiveScene(scene);

            var originPosition = GameObject.FindGameObjectWithTag("GolfBallOrigin").transform.position;

            GolfBall = Instantiate(golfballPrefab, originPosition, Quaternion.identity);

            Planet = GameObject.FindGameObjectWithTag("Planet");

            NumberOfHits = 0;
        };

        // when level start is complete start input
        ONLevelStartExit += () => StartInput();

        // when going back to main menu unload level and switch back to main menu scene
        ONBackToMainMenu += () =>
        {
            var scene = SceneManager.GetActiveScene();

            SceneManager.UnloadSceneAsync(scene);

            var main = SceneManager.GetSceneByBuildIndex(0);
            SceneManager.SetActiveScene(main);

            StartMainMenu();
        };

    }

    private void Start()
    {
        // start the game by going into main menu
        StartMainMenu();
    }

    // all events that can be listened to by other behaviours
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

    // invoke ONMainMenuEnter event
    public void StartMainMenu() => ONMainMenuEnter?.Invoke();

    // invoke ONBallMovementEnter event
    public void StartBallMovement(Vector3 initVelocity) => ONBallMovementEnter?.Invoke(initVelocity);

    // invoke ONInputEnter event
    public void StartInput() => ONInputEnter?.Invoke();

    // invoke ONLevelLoadEnter event
    public void LoadLevel(int id) => ONLevelLoadEnter?.Invoke(id);

    // invoke ONLevelCompleted event
    public void CompleteLevel() => ONLevelCompleted?.Invoke();

    // invoke ONBackToMainMenu event
    public void BackToMainMenu() => ONBackToMainMenu?.Invoke();

    // invoke level start events
    public void StartLevel(Scene scene)
    {
        ONLevelStart?.Invoke(scene);

        ONLevelStartExit?.Invoke();
    }

    // invoke ONLevelSelectEnter event
    public void StartLevelSelection() => ONLevelSelectEnter?.Invoke();

    // invoke ONHowToEnter event
    public void StartHowTo() => ONHowToEnter?.Invoke();

    // method to quit game
    public void QuitGame() => Application.Quit();

}


    
