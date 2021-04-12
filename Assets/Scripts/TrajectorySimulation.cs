using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR.WSA;

public class TrajectorySimulation : MonoBehaviour
{
    // how many steps should be pre calculated
    [SerializeField] private int steps = 10;

    // reference to the scene where trajectory is simulated in
    private PhysicsScene _trajSimScene;
    private Scene _scene;
    private Rigidbody _simTarget;
    private LineRenderer _renderer;

    private Rigidbody simGolfableRb;

    // the prefab for the simulated golfball
    [SerializeField] private GameObject golfBallSimPrefab;

    public void CreateTrajectory()
    {
        // match simulated golfball pos with actual golfball pos
        simGolfableRb.transform.SetPositionAndRotation(GameManager.Instance.GolfBall.transform.position, GameManager.Instance.GolfBall.transform.rotation);

        // apply initvelocity to simulated golfball
        simGolfableRb.velocity = GameManager.Instance.Controller.InitVelocity;

        // simulate trajectory 
        for (int i = 0; i < steps; i++)
        {
            
            // Calculate vector between golfball and planet
            Vector3 between = _simTarget.transform.position - simGolfableRb.transform.position;
            
            // Calculate the acceleration that needs to be applied to golfball
            Vector3 acceleration = GameManager.Instance.GravitationalConstant  * _simTarget.mass /
                between.sqrMagnitude * between.normalized;

            // Apply acceleration to rigidbody
            simGolfableRb.AddForce(acceleration, ForceMode.Acceleration);

            // run simulation for one fixed update
            _trajSimScene.Simulate(Time.fixedDeltaTime);

            // add pos to linerenderer every 5 simulated steps
            if (i % 5 == 0)
            {
                _renderer.SetPosition(i / 5, simGolfableRb.transform.position);
            }
        }
        
        // stop simulated golfball
        simGolfableRb.velocity = Vector3.zero;
        
    }

    public void ResetTrajectory()
    {
        // delete all points from line renderer
        _renderer.positionCount = 0;
        _renderer.positionCount = steps / 5;
    }

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
        _renderer.positionCount = steps / 5;
        // initalize simulation scene
        GameManager.Instance.ONLevelStart += levelName =>
        {
            // load simulation scene and get physics scene
            var param = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
            _scene = SceneManager.LoadScene("Scenes/TrajectorySimulationScene", param);
            _trajSimScene = _scene.GetPhysicsScene();

            // instantiate simulated version of planet and remove render and tag
            _simTarget = Instantiate(GameManager.Instance.Planet).GetComponent<Rigidbody>();
            Destroy(_simTarget.GetComponent<MeshRenderer>());
            Destroy(_simTarget.GetComponent<MeshFilter>());
            _simTarget.tag = "Untagged";

            // deactivate render for all simulated children of planet
            foreach (var meshRenderer in _simTarget.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = false;
            }

            // move simulated planet to simulated scene
            SceneManager.MoveGameObjectToScene(_simTarget.gameObject, _scene);

            // instantiate and move simulated golfball
            simGolfableRb = Instantiate(golfBallSimPrefab).GetComponent<Rigidbody>();

            SceneManager.MoveGameObjectToScene(simGolfableRb.gameObject, _scene);
        };

        // reset trajectory when ballmovement starts
        GameManager.Instance.ONBallMovementEnter += velocity => ResetTrajectory();

        // unload simulation scene when level is completed
        GameManager.Instance.ONLevelCompleted += () => SceneManager.UnloadSceneAsync(2);
        
    }

    private void Update()
    {
        // only create trajectory when in input state
        if (GameManager.Instance.CurrentState != GameManager.GameState.Input) return;

        CreateTrajectory();
    }
}
